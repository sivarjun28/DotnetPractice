namespace Exercise03.Exceptions
{
    public class BusinessRuleException : Exception
    {
        // Constructor to pass a custom message
        public BusinessRuleException(string message) : base(message) { }

        // Constructor to pass both message and inner exception
        public BusinessRuleException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}