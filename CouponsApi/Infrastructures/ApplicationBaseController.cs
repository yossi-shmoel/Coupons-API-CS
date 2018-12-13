using System;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Linq;

namespace CouponsApi.Infrastructures
{
    public class ApplicationBaseController : ApiController
    {
        protected HttpResponseMessage CreateErrorResponse(HttpStatusCode httpStatus, ApiStatusCodes errorCode, string message)
        {
            return CreateErrorResponse(httpStatus, (int)errorCode, message);
        }

        protected virtual HttpResponseMessage CreateErrorResponse(HttpStatusCode httpStatus, int errorCode, string message)
        {
            return Request.CreateResponse(httpStatus, new ErrorResponse(errorCode, message));
        }

        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
        public virtual HttpResponseMessage CreateErrorResponse(ActionResult errorResult)
        {
            var httpErrorCode = HttpStatusCode.InternalServerError;
            if (errorResult.ErrorCode.HasValue && Enum.IsDefined(typeof(HttpStatusCode), (int)errorResult.ErrorCode.Value))
            {
                httpErrorCode = (HttpStatusCode)(int)errorResult.ErrorCode.Value;
            }

            return CreateErrorResponse(httpErrorCode, (int)(errorResult.ErrorCode ?? ApiStatusCodes.InternalServerError), errorResult.Message);
        }

        //TODO: public because Moq doesn't support protected generic methods mocking
        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
        public virtual HttpResponseMessage CreateResponseMessage<T>(T response, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return Request.CreateResponse(statusCode, response);
        }

        protected virtual HttpResponseMessage CreateResponseMessage(ActionResult result)
        {
            return result.IsSuccess ? CreateResponseMessage(result, HttpStatusCode.OK) : CreateErrorResponse(result);
        }

        //TODO: public because Moq doesn't support protected generic methods mocking
        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
        public virtual HttpResponseMessage CreateResponseMessage(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return Request.CreateResponse(statusCode);
        }

        //TODO: public because Moq doesn't support protected generic methods mocking
        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
        public virtual HttpResponseMessage CreateResponseMessage<T>(ActionResultModel<T> result)
        {
            return result.IsSuccess ? CreateResponseMessage(result.Result) : CreateErrorResponse(result);
        }

        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage CreateGeneralResultMessage(string message, bool isSuccess, ApiStatusCodes errorCode = ApiStatusCodes.OK)
        {
            return CreateResponseMessage(new ResultMessage
            {
                IsSuccess = isSuccess,
                Message = message,
                ApiStatusCode = errorCode
            }, isSuccess.Equals(false) ? HttpStatusCode.BadRequest : HttpStatusCode.OK);
        }

        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
        public string GetToken()
        {
            var token = GetTokenFromHeader();
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }
            else
            {
                return GetTokenFromSwagger();
            }
        }

        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
        private string GetTokenFromHeader()
        {
            var header = Request.Headers.FirstOrDefault(h => h.Key.Equals("Authorization"));
            if (header.Value != null)
            {
                return header.Value.FirstOrDefault();
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetTokenFromSwagger()
        {
            try
            {
                object requestUri;
                Request.Properties.TryGetValue("MS_CachedRequestQuery", out requestUri);
                var uriRequestDecoded = System.Web.HttpUtility.UrlDecode(requestUri.ToString());
                var toBeSearched = "api_key=";

                return uriRequestDecoded.Substring(uriRequestDecoded.IndexOf(toBeSearched) + toBeSearched.Length);
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Failed to retrive token from request, error = {ex}");
            }
        }
    }

    public class ResultMessage
    {
        public bool IsSuccess { set; get; }
        public string Message { set; get; }
        public ApiStatusCodes ApiStatusCode { set; get; }
    }
}