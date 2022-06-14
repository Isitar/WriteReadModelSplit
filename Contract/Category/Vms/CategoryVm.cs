namespace Contract.Category.Vms;

using Item.Vms;

public class CategoryVm : CategoryListEntryVm
{
    public string? Description { get; set; }
    public IEnumerable<ItemListEntryVm> Items { get; set; } = new List<ItemListEntryVm>();
}