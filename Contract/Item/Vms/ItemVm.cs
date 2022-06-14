namespace Contract.Item.Vms;

using Category.Vms;

public class ItemVm : ItemListEntryVm
{
    public CategorySimpleVm Category { get; set; } = null!;
}