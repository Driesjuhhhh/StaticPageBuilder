namespace EKO.StaticPageBuilder.Models;

/// <summary>
/// Config file for the page builder
/// </summary>
internal sealed class PageBuilderConfig
{
    public HomeConfig Home { get; set; } = null!;
    public ArticleConfig Article { get; set; } = null!;
    public PagingConfig Paging { get; set; } = null!;
    public ConnectFourConfig ConnectFour { get; set; } = null!;
    public ShoppingListConfig ShoppingList { get; set; } = null!;
    public List<PageVar> PageVars { get; set; } = new();
}
