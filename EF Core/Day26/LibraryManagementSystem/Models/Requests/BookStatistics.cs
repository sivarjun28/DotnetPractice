namespace LibraryManagementSystem.Models.Requests
{
    public class BookSearchCriteria
    {
        public string? Title { get; set; }
        public string? Publisher { get; set; }
        public int? MinPages { get; set; }
        public int? MaxPages { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateTime? PublishedAfter { get; set; }
        public DateTime? PublishedBefore { get; set; }
        public bool? OnlyAvailable { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Title";
        public bool SortDescending { get; set; }
    }
}