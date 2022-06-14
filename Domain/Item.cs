namespace Domain;

public class Item
{
    public Guid Id { get; protected set; }
    public string Name { get; protected set; }
    public decimal Price { get; protected set; }
    public virtual Category Category { get; protected set; } = null!;
    public Guid CategoryId { get; protected set; }

    protected Item(Guid id, string name, decimal price, Guid categoryId)
    {
        Id = id;
        CategoryId = categoryId;
        UpdateBasicData(name, price);
    }

    public static Item Create(Guid id, string name, decimal price, Category category)
    {
        var item = new Item(id, name, price, category.Id)
        {
            Category = category
        };
        return item;
    }

    public void UpdateBasicData(string name, decimal price)
    {
        Name = name.Trim();
        Price = price;
    }

    public void MoveToCategory(Category category)
    {
        Category = category;
    }
}