using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc.ModelBinding.Binders
{
    public class ObjectIdBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                if(bindingContext.ModelType == typeof(ObjectId?))
                {
                    bindingContext.Result = ModelBindingResult.Success((ObjectId?)null);
                }
                else
                {
                    bindingContext.ModelState.TryAddModelError(
                        modelName, $"required.");
                }
                return Task.CompletedTask;
            }

            if (!ObjectId.TryParse(value, out var id))
            {
                bindingContext.ModelState.TryAddModelError(
                    modelName, $"“{value}” is an invalid ObjectId format.");

                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(id);
            return Task.CompletedTask;
        }
    }
}
