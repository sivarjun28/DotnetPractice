namespace Exercise03.Exceptions
{
    public class NotFoundException : Exception
    {
        // Constructor to pass a custom message
        public NotFoundException(string message) : base(message) { }

        // Constructor to pass both message and inner exception
        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}