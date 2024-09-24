namespace SQLicious_ASP.NET_MVC.Helpers
{
    public class PagedResult<T> where T : class
    {
        public IEnumerable<T> Results { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public PagedResult(IEnumerable<T> results, int currentPage, int totalPages, int pageSize)
        {
            Results = results;
            CurrentPage = currentPage;
            TotalPages = totalPages;
            PageSize = pageSize;
        }

        public static PagedResult<T> Create(IEnumerable<T> source, int page, int pageSize)
        {
            var totalItems = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResult<T>(items, page, (int)Math.Ceiling(totalItems / (double)pageSize), pageSize);
        }
    }
}