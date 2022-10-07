using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Light.Admin.Mongo.Basics
{
    public class ValidationFailedResultModel : BaseResultModel
    {

        public ValidationFailedResultModel(ResultExecutingContext context)
        {
            Code = StatusCodes.Status422UnprocessableEntity;
            Message = "参数不合法";
            Success = false;
            Path = context.HttpContext.Request.Path;

            var Result = context.ModelState.Keys
                              .SelectMany(key => context.ModelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                              .ToList();
            Data = Result;
        }
    }
    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }
        public string Message { get; }
        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }
    }
}
