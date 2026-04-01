namespace Exercise02.Exceptions
{
    public class InsufficientStockException : Exception
    {
        public int RequestedQuantity { get; }
        public int AvailableQuantity { get; }

        public InsufficientStockException(int requested, int available)
            : base($"INSUFFICIENT_STOCK Requested {requested}, but only {available} available.")
        {
            RequestedQuantity = requested;
            AvailableQuantity = available;
        }
    }
}