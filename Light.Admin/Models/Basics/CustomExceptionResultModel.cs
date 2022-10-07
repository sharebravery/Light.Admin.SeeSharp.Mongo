using Microsoft.AspNetCore.Mvc;

namespace Light.Admin.Mongo.Basics
{
    public class CustomExceptionResultModel : BaseResultModel
    {
        public CustomExceptionResultModel(int? code, Exception exception)
        {
            Code = code;
            Success = false;
            Message = exception.InnerException != null ?
                exception.InnerException.Message :
                exception.Message;
            Data = new ExceptionResultModel(exception);
        }

        public class ExceptionResultModel
        {
            public ExceptionResultModel(Exception exception)
            {
                Message = exception.Message;
                StackTrace = exception.StackTrace;
                TargetSite = exception.TargetSite!.ToString();
                Source = exception.Source;
                HResult = exception.HResult;
            }

            public string? Message { get; set; }
            public string? StackTrace { get; set; }
            public string? TargetSite { get; set; }
            public string? Source { get; set; }
            public int? HResult { get; set; }

        }
    }
}
