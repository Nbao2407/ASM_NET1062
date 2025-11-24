namespace ASM.DTOs;

public class OrderResponse
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? PaymentMethod { get; set; }
    public string? PaymentStatus { get; set; }
    public string? DeliveryAddress { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? StatusUpdatedAt { get; set; }
    public List<OrderItemResponse> Items { get; set; } = new();
}

public class OrderItemResponse
{
    public int OrderItemId { get; set; }
    public int? FoodItemId { get; set; }
    public string? FoodItemName { get; set; }
    public int? ComboId { get; set; }
    public string? ComboName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}
