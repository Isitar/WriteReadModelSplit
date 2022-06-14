namespace Contract.Category.Requests;

public class CreateItemRequest
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}