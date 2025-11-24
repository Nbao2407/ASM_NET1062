using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASM.Data;
using ASM.DTOs;
using ASM.Models;
using System.Security.Claims;

namespace ASM.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(ApplicationDbContext context, ILogger<OrdersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // POST: api/orders
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        try
        {
            // Get current user ID from claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user authentication" });
            }

            // Validate request
            if (request.Items == null || request.Items.Count == 0)
            {
                return BadRequest(new { message = "Order must contain at least one item" });
            }

            if (string.IsNullOrWhiteSpace(request.DeliveryAddress))
            {
                return BadRequest(new { message = "Delivery address is required" });
            }

            if (string.IsNullOrWhiteSpace(request.PaymentMethod))
            {
                return BadRequest(new { message = "Payment method is required" });
            }

            // Validate and calculate order items
            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                {
                    return BadRequest(new { message = "Item quantity must be greater than zero" });
                }

                if (item.FoodItemId.HasValue && item.ComboId.HasValue)
                {
                    return BadRequest(new { message = "Order item cannot have both FoodItemId and ComboId" });
                }

                if (!item.FoodItemId.HasValue && !item.ComboId.HasValue)
                {
                    return BadRequest(new { message = "Order item must have either FoodItemId or ComboId" });
                }

                decimal unitPrice = 0;

                if (item.FoodItemId.HasValue)
                {
                    var foodItem = await _context.FoodItems
                        .FirstOrDefaultAsync(f => f.FoodItemId == item.FoodItemId.Value && f.IsActive);
                    
                    if (foodItem == null)
                    {
                        return BadRequest(new { message = $"Food item with ID {item.FoodItemId.Value} not found or inactive" });
                    }

                    unitPrice = foodItem.Price;
                }
                else if (item.ComboId.HasValue)
                {
                    var combo = await _context.Combos
                        .FirstOrDefaultAsync(c => c.ComboId == item.ComboId.Value && c.IsActive);
                    
                    if (combo == null)
                    {
                        return BadRequest(new { message = $"Combo with ID {item.ComboId.Value} not found or inactive" });
                    }

                    unitPrice = combo.Price;
                }

                var subtotal = unitPrice * item.Quantity;
                totalAmount += subtotal;

                orderItems.Add(new OrderItem
                {
                    FoodItemId = item.FoodItemId,
                    ComboId = item.ComboId,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    Subtotal = subtotal
                });
            }

            // Generate unique order number
            var orderNumber = await GenerateUniqueOrderNumber();

            // Create order
            var order = new Order
            {
                UserId = userId,
                OrderNumber = orderNumber,
                TotalAmount = totalAmount,
                Status = "NotDelivered",
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = "Completed", // Simplified - assume payment is processed
                DeliveryAddress = request.DeliveryAddress,
                OrderDate = DateTime.UtcNow,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Create invoice
            var invoiceNumber = await GenerateUniqueInvoiceNumber();
            var invoice = new Invoice
            {
                OrderId = order.OrderId,
                InvoiceNumber = invoiceNumber,
                InvoiceDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                TaxAmount = 0,
                DiscountAmount = 0
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // Load related data for response
            await _context.Entry(order)
                .Collection(o => o.OrderItems)
                .Query()
                .Include(oi => oi.FoodItem)
                .Include(oi => oi.Combo)
                .LoadAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, MapToOrderResponse(order));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order");
            return StatusCode(500, new { message = "An error occurred while creating the order" });
        }
    }

    // GET: api/orders
    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetOrders([FromQuery] string? status = null)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user authentication" });
            }

            IQueryable<Order> query = _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.FoodItem)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Combo);

            // If user is customer, only show their orders
            if (userRole != "Admin")
            {
                query = query.Where(o => o.UserId == userId);
            }

            // Filter by status if provided
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(o => o.Status == status);
            }

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return Ok(orders.Select(MapToOrderResponse).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving orders");
            return StatusCode(500, new { message = "An error occurred while retrieving orders" });
        }
    }

    // GET: api/orders/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetOrderById(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user authentication" });
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.FoodItem)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Combo)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            // If user is customer, only allow access to their own orders
            if (userRole != "Admin" && order.UserId != userId)
            {
                return Forbid();
            }

            return Ok(MapToOrderResponse(order));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving order {OrderId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the order" });
        }
    }

    // PUT: api/orders/{id}/status
    [HttpPut("{id}/status")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<OrderResponse>> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequest request)
    {
        try
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.FoodItem)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Combo)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            // Validate status
            var validStatuses = new[] { "NotDelivered", "BeingDelivered", "Delivered" };
            if (!validStatuses.Contains(request.Status))
            {
                return BadRequest(new { message = "Invalid status. Must be one of: NotDelivered, BeingDelivered, Delivered" });
            }

            order.Status = request.Status;
            order.StatusUpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(MapToOrderResponse(order));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order status for order {OrderId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the order status" });
        }
    }

    // GET: api/orders/invoices
    [HttpGet("invoices")]
    public async Task<ActionResult<List<InvoiceResponse>>> GetInvoices(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user authentication" });
            }

            var query = _context.Invoices
                .Include(i => i.Order)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.FoodItem)
                .Include(i => i.Order)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.Combo)
                .Where(i => i.Order.UserId == userId);

            // Apply date range filtering
            if (startDate.HasValue)
            {
                query = query.Where(i => i.InvoiceDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(i => i.InvoiceDate <= endDate.Value);
            }

            var invoices = await query
                .OrderByDescending(i => i.InvoiceDate)
                .ToListAsync();

            return Ok(invoices.Select(MapToInvoiceResponse).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving invoices");
            return StatusCode(500, new { message = "An error occurred while retrieving invoices" });
        }
    }

    // GET: api/orders/invoices/{id}
    [HttpGet("invoices/{id}")]
    public async Task<ActionResult<InvoiceResponse>> GetInvoiceById(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user authentication" });
            }

            var invoice = await _context.Invoices
                .Include(i => i.Order)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.FoodItem)
                .Include(i => i.Order)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.Combo)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (invoice == null)
            {
                return NotFound(new { message = "Invoice not found" });
            }

            // If user is customer, only allow access to their own invoices
            if (userRole != "Admin" && invoice.Order.UserId != userId)
            {
                return Forbid();
            }

            return Ok(MapToInvoiceResponse(invoice));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving invoice {InvoiceId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the invoice" });
        }
    }

    // Helper methods
    private async Task<string> GenerateUniqueOrderNumber()
    {
        string orderNumber;
        bool exists;

        do
        {
            // Generate order number in format: ORD-YYYYMMDD-XXXXX
            var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            var randomPart = new Random().Next(10000, 99999);
            orderNumber = $"ORD-{datePart}-{randomPart}";

            exists = await _context.Orders.AnyAsync(o => o.OrderNumber == orderNumber);
        } while (exists);

        return orderNumber;
    }

    private async Task<string> GenerateUniqueInvoiceNumber()
    {
        string invoiceNumber;
        bool exists;

        do
        {
            // Generate invoice number in format: INV-YYYYMMDD-XXXXX
            var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            var randomPart = new Random().Next(10000, 99999);
            invoiceNumber = $"INV-{datePart}-{randomPart}";

            exists = await _context.Invoices.AnyAsync(i => i.InvoiceNumber == invoiceNumber);
        } while (exists);

        return invoiceNumber;
    }

    private OrderResponse MapToOrderResponse(Order order)
    {
        return new OrderResponse
        {
            OrderId = order.OrderId,
            OrderNumber = order.OrderNumber,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            PaymentMethod = order.PaymentMethod,
            PaymentStatus = order.PaymentStatus,
            DeliveryAddress = order.DeliveryAddress,
            OrderDate = order.OrderDate,
            StatusUpdatedAt = order.StatusUpdatedAt,
            Items = order.OrderItems.Select(oi => new OrderItemResponse
            {
                OrderItemId = oi.OrderItemId,
                FoodItemId = oi.FoodItemId,
                FoodItemName = oi.FoodItem?.Name,
                ComboId = oi.ComboId,
                ComboName = oi.Combo?.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                Subtotal = oi.Subtotal
            }).ToList()
        };
    }

    private InvoiceResponse MapToInvoiceResponse(Invoice invoice)
    {
        return new InvoiceResponse
        {
            InvoiceId = invoice.InvoiceId,
            InvoiceNumber = invoice.InvoiceNumber,
            InvoiceDate = invoice.InvoiceDate,
            TotalAmount = invoice.TotalAmount,
            TaxAmount = invoice.TaxAmount,
            DiscountAmount = invoice.DiscountAmount,
            Order = MapToOrderResponse(invoice.Order)
        };
    }
}
