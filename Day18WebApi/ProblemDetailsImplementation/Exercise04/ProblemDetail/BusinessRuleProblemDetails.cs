using Microsoft.AspNetCore.Mvc;

namespace Exercise04.ProblemDetail
{
    public class BusinessRuleProblemDetails : ProblemDetails
    {
        public string RuleCode{get; set;}
        public Dictionary<string, object> RuleData{get; set;}

        
    }
}