namespace Contract.Item.Requests;

public class UpdateItemRequest
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}