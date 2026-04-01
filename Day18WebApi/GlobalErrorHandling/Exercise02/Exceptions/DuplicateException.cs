namespace Exercise02.Exceptions
{
    public class DuplicateException : Exception
    {
        public DuplicateException(string entityName, object key)
            :base($"{entityName} with {key} already Exists.")
        {
            
        }
    }
}