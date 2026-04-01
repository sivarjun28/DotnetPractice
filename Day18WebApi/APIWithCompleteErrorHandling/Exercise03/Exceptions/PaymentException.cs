namespace Exercise03.Exceptions
{
    public class PaymentException : Exception
    {
        // Constructor to pass a custom message
        public PaymentException(string message) : base(message) { }

        // Constructor to pass both message and inner exception
        public PaymentException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}