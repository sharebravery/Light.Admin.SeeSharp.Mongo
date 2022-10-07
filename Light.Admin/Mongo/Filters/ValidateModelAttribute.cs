using Light.Admin.Mongo.Basics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.Admin.Mongo.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    if (!context.ModelState.IsValid)
        //    {
        //        //context.Result = new ValidationFailedResult(context);
        //    }
        //}
    }

}
