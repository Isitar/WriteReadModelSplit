namespace Contract.Category.Requests;

public class CreateCategoryRequest
{
    public string Name { get; set; } = null!;
    public string? Color { get; set; }
    public string? Description { get; set; }
}