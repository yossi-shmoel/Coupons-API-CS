using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponsApi.Infrastructures
{
    public class ActionResult
    {
        public bool IsSuccess { get; set; }

        public ApiStatusCodes? ErrorCode { get; set; }
        public string Message { get; set; }

        public ActionResultModel<T> ToActionResultModel<T>(T result = default(T))
        {
            return new ActionResultModel<T>
            {
                ErrorCode = ErrorCode,
                IsSuccess = IsSuccess,
                Message = Message,
                Result = result
            };
        }
    }
}