﻿using Light.Admin.Mongo.Basics;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Light.Admin.Mongo.Filters
{
    public class CustomExceptionAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;

            //处理各种异常

            context.ExceptionHandled = true;
            context.Result = new CustomExceptionResult((int)status, context.Exception);
        }
    }
}
