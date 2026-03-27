using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Exercise02.ModelBinders
{
    public class CommaSeparatedModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName).FirstValue;

            if (string.IsNullOrEmpty(value))
                return Task.CompletedTask;

            var result = value.Split(',')
                              .Select(x => int.Parse(x.Trim()))
                              .ToArray();

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}