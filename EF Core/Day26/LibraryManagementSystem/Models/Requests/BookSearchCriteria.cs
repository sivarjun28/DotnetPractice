namespace LibraryManagementSystem.Models.Requests
{
    public class BookStatistics
    {
        public int TotalBooks { get; set; }
        public int AvailableBooks { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }
        public int TotalPages { get; set; }
        public string MostExpensiveBook { get; set; } = string.Empty;
        public string LongestBook { get; set; } = string.Empty;
    }
}