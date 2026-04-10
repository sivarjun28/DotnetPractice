namespace Exercise03.Models
{
    public class BulkOperationResult
    {
        public int Successful { get; set; }
        public int Failed { get; set; }
        public List<OrderResult> Results { get; set; } = new();
    }
}
