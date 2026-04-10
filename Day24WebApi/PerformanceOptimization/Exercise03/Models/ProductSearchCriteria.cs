namespace Exercise03.Models
{
    public class ProductSearchCriteria
    {
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Category { get; set; }

        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}