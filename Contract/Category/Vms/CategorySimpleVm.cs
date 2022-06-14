namespace Contract.Category.Vms;

using Common;

public class CategorySimpleVm : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}