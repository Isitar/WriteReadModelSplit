namespace Contract.Item.Vms;

using Common;

public class ItemSimpleVm : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}