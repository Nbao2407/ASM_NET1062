namespace ASM.Models;

public class Order
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "NotDelivered"; // "NotDelivered", "BeingDelivered", "Delivered"
    public string? PaymentMethod { get; set; }
    public string? PaymentStatus { get; set; }
    public string? DeliveryAddress { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? StatusUpdatedAt { get; set; }
    
    public User User { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public Invoice? Invoice { get; set; }
}
