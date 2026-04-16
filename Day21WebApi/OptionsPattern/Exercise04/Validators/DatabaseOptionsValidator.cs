using System.Security.Cryptography.X509Certificates;
using Exercise04.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Validation;

namespace Exercise04.Validators
{
    public class DatabaseOptionsValidator : IValidateOptions<DatabaseOptions>
    {
        public ValidateOptionsResult Validate(string? name, DatabaseOptions options)
        {
            if (string.IsNullOrEmpty(options.ConnectionString))
                return ValidateOptionsResult.Fail("Connectionstring is required");
            if (options.MaxRetries < 0 || options.MaxRetries > 10)
                return ValidateOptionsResult.Fail("MaxRetries must be between 0 and 10");

            if (options.CommandTimeOut <= 0)
                return ValidateOptionsResult.Fail("CommandTimeout must be greater than 0");

            return ValidateOptionsResult.Success;
        }
    }
}