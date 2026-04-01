using FluentValidation.Results;

namespace Exercise06.Exceptions

{
    public class CustomValidationException : Exception
    {
        public List<ValidationFailure> Errors { get; }

        public CustomValidationException(List<ValidationFailure> errors)
        {
            Errors = errors;
        }
    }
}