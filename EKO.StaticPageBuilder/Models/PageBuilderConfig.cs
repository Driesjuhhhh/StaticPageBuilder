namespace EKO.StaticPageBuilder.Models;

internal sealed class PageBuilderConfig
{
    public HomeConfig Home { get; set; } = null!;
    public ArticleConfig Article { get; set; } = null!;
    public PagingConfig Paging { get; set; } = null!;
    public List<PageVar> PageVars { get; set; } = new();
}
