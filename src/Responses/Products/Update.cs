namespace BugStore.Responses.Products;

public class Update
{
    public string OldTitle { get; set; } = string.Empty;
    public string OldDescription { get; set; } = string.Empty;
    public string OldSlug { get; set; } = string.Empty;
    public decimal OldPrice { get; set; } = 0;
    public string NewTitle { get; set; } = string.Empty;
    public string NewDescription { get; set; } = string.Empty;
    public string NewSlug { get; set; } = string.Empty;
    public decimal NewPrice { get; set; } = 0;
}