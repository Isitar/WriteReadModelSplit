namespace Domain;

public class Category
{
    public Guid Id { get; protected set; }
    public string Name { get; protected set; }
    public string? Description { get; protected set; }
    public string? Color { get; protected set; }

    public virtual ICollection<Item> Items { get; protected set; } = new HashSet<Item>();

    public Category(Guid id, string name, string? description, string? color)
    {
        Id = id;
        Name = name;
        Description = description;
        Color = color;
    }

    public void UpdateBasicData(string name, string? description, string? color)
    {
        Name = name.Trim();
        Description = description?.Trim();
        Color = color?.Trim();
    }
}