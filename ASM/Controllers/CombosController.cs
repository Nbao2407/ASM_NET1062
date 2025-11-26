using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASM.Data;
using ASM.DTOs;
using ASM.Models;

using ASM.Services;

namespace ASM.Controllers;

[ApiController]
[Route("api/combos")]
public class CombosController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ISanitizationService _sanitizationService;
    private readonly ILogger<CombosController> _logger;

    public CombosController(
        ApplicationDbContext context,
        ISanitizationService sanitizationService,
        ILogger<CombosController> logger)
    {
        _context = context;
        _sanitizationService = sanitizationService;
        _logger = logger;
    }

    /// <summary>
    /// Get all combos
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ComboResponse>>> GetCombos()
    {
        var combos = await _context.Combos
            .Where(c => c.IsActive)
            .Include(c => c.ComboItems)
                .ThenInclude(ci => ci.FoodItem)
            .OrderBy(c => c.Name)
            .ToListAsync();

        var response = combos.Select(c => new ComboResponse
        {
            ComboId = c.ComboId,
            Name = c.Name,
            Description = c.Description,
            Price = c.Price,
            ImageUrl = c.ImageUrl,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            ComboItems = c.ComboItems.Select(ci => new ComboItemResponse
            {
                FoodItemId = ci.FoodItemId,
                FoodItemName = ci.FoodItem.Name,
                FoodItemPrice = ci.FoodItem.Price,
                Quantity = ci.Quantity
            }).ToList()
        });

        return Ok(response);
    }

    /// <summary>
    /// Get a specific combo by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ComboResponse>> GetCombo(int id)
    {
        var combo = await _context.Combos
            .Include(c => c.ComboItems)
                .ThenInclude(ci => ci.FoodItem)
            .FirstOrDefaultAsync(c => c.ComboId == id);

        if (combo == null || !combo.IsActive)
        {
            return NotFound(new { message = "Combo not found" });
        }

        var response = new ComboResponse
        {
            ComboId = combo.ComboId,
            Name = combo.Name,
            Description = combo.Description,
            Price = combo.Price,
            ImageUrl = combo.ImageUrl,
            IsActive = combo.IsActive,
            CreatedAt = combo.CreatedAt,
            UpdatedAt = combo.UpdatedAt,
            ComboItems = combo.ComboItems.Select(ci => new ComboItemResponse
            {
                FoodItemId = ci.FoodItemId,
                FoodItemName = ci.FoodItem.Name,
                FoodItemPrice = ci.FoodItem.Price,
                Quantity = ci.Quantity
            }).ToList()
        };

        return Ok(response);
    }

    /// <summary>
    /// Create a new combo (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<ComboResponse>> CreateCombo([FromBody] CreateComboRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Sanitize inputs
        request.Name = _sanitizationService.Sanitize(request.Name);
        request.Description = _sanitizationService.Sanitize(request.Description);

        // Validate that all food items exist and are active
        var foodItemIds = request.ComboItems.Select(ci => ci.FoodItemId).Distinct().ToList();
        var foodItems = await _context.FoodItems
            .Where(f => foodItemIds.Contains(f.FoodItemId) && f.IsActive)
            .ToListAsync();

        if (foodItems.Count != foodItemIds.Count)
        {
            return BadRequest(new { message = "One or more food items not found or inactive" });
        }

        // Calculate sum of individual item prices
        decimal sumOfItemPrices = 0;
        foreach (var comboItem in request.ComboItems)
        {
            var foodItem = foodItems.First(f => f.FoodItemId == comboItem.FoodItemId);
            sumOfItemPrices += foodItem.Price * comboItem.Quantity;
        }

        // Validate combo price is less than sum of item prices
        if (request.Price >= sumOfItemPrices)
        {
            return BadRequest(new 
            { 
                message = "Combo price must be less than the sum of individual item prices",
                comboPrice = request.Price,
                sumOfItemPrices = sumOfItemPrices
            });
        }

        var combo = new Combo
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            ImageUrl = request.ImageUrl,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Combos.Add(combo);
        await _context.SaveChangesAsync();

        // Add combo items
        foreach (var comboItemRequest in request.ComboItems)
        {
            var comboItem = new ComboItem
            {
                ComboId = combo.ComboId,
                FoodItemId = comboItemRequest.FoodItemId,
                Quantity = comboItemRequest.Quantity
            };
            _context.ComboItems.Add(comboItem);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Combo created: {Name} (ID: {Id})", combo.Name, combo.ComboId);

        // Reload combo with items for response
        var createdCombo = await _context.Combos
            .Include(c => c.ComboItems)
                .ThenInclude(ci => ci.FoodItem)
            .FirstAsync(c => c.ComboId == combo.ComboId);

        var response = new ComboResponse
        {
            ComboId = createdCombo.ComboId,
            Name = createdCombo.Name,
            Description = createdCombo.Description,
            Price = createdCombo.Price,
            ImageUrl = createdCombo.ImageUrl,
            IsActive = createdCombo.IsActive,
            CreatedAt = createdCombo.CreatedAt,
            UpdatedAt = createdCombo.UpdatedAt,
            ComboItems = createdCombo.ComboItems.Select(ci => new ComboItemResponse
            {
                FoodItemId = ci.FoodItemId,
                FoodItemName = ci.FoodItem.Name,
                FoodItemPrice = ci.FoodItem.Price,
                Quantity = ci.Quantity
            }).ToList()
        };

        return CreatedAtAction(nameof(GetCombo), new { id = combo.ComboId }, response);
    }

    /// <summary>
    /// Update an existing combo (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<ComboResponse>> UpdateCombo(int id, [FromBody] UpdateComboRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var combo = await _context.Combos
            .Include(c => c.ComboItems)
            .FirstOrDefaultAsync(c => c.ComboId == id);

        if (combo == null)
        {
            return NotFound(new { message = "Combo not found" });
        }

        // Sanitize inputs
        request.Name = _sanitizationService.Sanitize(request.Name);
        request.Description = _sanitizationService.Sanitize(request.Description);

        // Validate that all food items exist and are active
        var foodItemIds = request.ComboItems.Select(ci => ci.FoodItemId).Distinct().ToList();
        var foodItems = await _context.FoodItems
            .Where(f => foodItemIds.Contains(f.FoodItemId) && f.IsActive)
            .ToListAsync();

        if (foodItems.Count != foodItemIds.Count)
        {
            return BadRequest(new { message = "One or more food items not found or inactive" });
        }

        // Calculate sum of individual item prices
        decimal sumOfItemPrices = 0;
        foreach (var comboItem in request.ComboItems)
        {
            var foodItem = foodItems.First(f => f.FoodItemId == comboItem.FoodItemId);
            sumOfItemPrices += foodItem.Price * comboItem.Quantity;
        }

        // Validate combo price is less than sum of item prices
        if (request.Price >= sumOfItemPrices)
        {
            return BadRequest(new 
            { 
                message = "Combo price must be less than the sum of individual item prices",
                comboPrice = request.Price,
                sumOfItemPrices = sumOfItemPrices
            });
        }

        // Update combo properties
        combo.Name = request.Name;
        combo.Description = request.Description;
        combo.Price = request.Price;
        combo.ImageUrl = request.ImageUrl;
        combo.IsActive = request.IsActive;
        combo.UpdatedAt = DateTime.UtcNow;

        // Remove existing combo items
        _context.ComboItems.RemoveRange(combo.ComboItems);

        // Add new combo items
        foreach (var comboItemRequest in request.ComboItems)
        {
            var comboItem = new ComboItem
            {
                ComboId = combo.ComboId,
                FoodItemId = comboItemRequest.FoodItemId,
                Quantity = comboItemRequest.Quantity
            };
            _context.ComboItems.Add(comboItem);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Combo updated: {Name} (ID: {Id})", combo.Name, combo.ComboId);

        // Reload combo with items for response
        var updatedCombo = await _context.Combos
            .Include(c => c.ComboItems)
                .ThenInclude(ci => ci.FoodItem)
            .FirstAsync(c => c.ComboId == combo.ComboId);

        var response = new ComboResponse
        {
            ComboId = updatedCombo.ComboId,
            Name = updatedCombo.Name,
            Description = updatedCombo.Description,
            Price = updatedCombo.Price,
            ImageUrl = updatedCombo.ImageUrl,
            IsActive = updatedCombo.IsActive,
            CreatedAt = updatedCombo.CreatedAt,
            UpdatedAt = updatedCombo.UpdatedAt,
            ComboItems = updatedCombo.ComboItems.Select(ci => new ComboItemResponse
            {
                FoodItemId = ci.FoodItemId,
                FoodItemName = ci.FoodItem.Name,
                FoodItemPrice = ci.FoodItem.Price,
                Quantity = ci.Quantity
            }).ToList()
        };

        return Ok(response);
    }

    /// <summary>
    /// Delete a combo (Admin only, soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteCombo(int id)
    {
        var combo = await _context.Combos.FindAsync(id);

        if (combo == null)
        {
            return NotFound(new { message = "Combo not found" });
        }

        // Soft delete - set IsActive to false
        combo.IsActive = false;
        combo.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Combo soft deleted: {Name} (ID: {Id})", combo.Name, combo.ComboId);

        return Ok(new { message = "Combo deleted successfully" });
    }
}
