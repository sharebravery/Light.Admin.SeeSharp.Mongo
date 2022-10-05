using Microsoft.AspNetCore.Mvc;

namespace Light.Admin.Models.Basics
{
    public class CustomExceptionResult : ObjectResult
    {
        public CustomExceptionResult(int? code, Exception exception)
                : base(new CustomExceptionResultModel(code, exception))
        {
            StatusCode = code;
        }
    }
}
