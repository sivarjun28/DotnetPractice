namespace ECommerceAPI.Models.Entities
{
    public class ProductSearchCriteria
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}