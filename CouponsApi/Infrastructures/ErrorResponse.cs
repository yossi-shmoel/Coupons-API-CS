using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponsApi.Infrastructures
{
    public class ErrorResponse
    {
        //TODO: expected error message format should be:  {"errorCode":1, "errorMessage":"string"}
        public ErrorResponse(int errorCode, string message)
        {
            this.Message = message;
            this.ErrorCode = errorCode;
        }

        public int ErrorCode { get; set; }

        public string Message { get; set; }
    }
}