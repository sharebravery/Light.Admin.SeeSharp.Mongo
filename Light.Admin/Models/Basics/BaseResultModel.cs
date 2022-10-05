using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Light.Admin.Models.Basics
{
    public class BaseResultModel
    {
        public BaseResultModel(int? code = null, string message = null,
       object data = null, bool success = true, DateTime? time = null)
        {
            Code = code;
            Data = data;
            Message = message;
            Success = success;
            Time = time ?? DateTime.Now;
        }
        public int? Code { get; set; }

        public string Message { get; set; }

        public string Path { get; set; }

        public object Data { get; set; }

        public bool Success { get; set; }

        public DateTime? Time { get; set; }

    }
}
