namespace ASM.DTOs;

public class CreateOrderRequest
{
    public List<OrderItemRequest> Items { get; set; } = new();
    public string DeliveryAddress { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
}

public class OrderItemRequest
{
    public int? FoodItemId { get; set; }
    public int? ComboId { get; set; }
    public int Quantity { get; set; }
}
