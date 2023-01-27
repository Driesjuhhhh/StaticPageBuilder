namespace EKO.StaticPageBuilder.Helpers;

internal static class ListHelper
{
    /// <summary>
    /// Split a list into smaller chunks that allow for paging.
    /// </summary>
    /// <param name="source">List that will be split.</param>
    /// <param name="pageSize">Maximum amount of items per List</param>
    /// <returns>Paged list.</returns>
    public static List<IEnumerable<Page>> PageList(IEnumerable<Page> source, int pageSize)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / pageSize)
            .Select(x => x.Select(v => v.Value))
            .ToList();
    }
}
