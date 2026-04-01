namespace Exercise02.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public string RuleCode{get; }

        public BusinessRuleException(string ruleCode, string message)
                : base(message)
        {
            RuleCode = ruleCode;
        }
    }
}