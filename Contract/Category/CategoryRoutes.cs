namespace Contract.Category;

public class CategoryRoutes
{
    public const string CreateCategory = "/categories";
    public const string UpdateCategory = "/categories/{id:guid}";
    public const string DeleteCategory = "/categories/{id:guid}";
    public const string ListCategories = "/categories";
    public const string CategoryDetail = "/categories/{id:guid}";
    public const string CategoryItems = "/categories/{id:guid}/items";
    public const string CreateItem = "/categories/{id:guid}/items";
}