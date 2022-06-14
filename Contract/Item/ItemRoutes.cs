namespace Contract.Item;

public class ItemRoutes
{
    public const string UpdateItem = "/items/{id:guid}";
    public const string DeleteItem = "/items/{id:guid}";
    public const string ListItems = "/items";
    public const string ItemDetail = "/items/{id:guid}";
}