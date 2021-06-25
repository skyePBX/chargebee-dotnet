using System;
using System.Collections.Generic;
using System.Net;

namespace ChargeBee.Api
{
    public class ApiException : Exception
    {
        private readonly string _errorParam;

        private readonly string _errorType;

        public ApiException(HttpStatusCode httpStatusCode, IReadOnlyDictionary<string, string> errorResp)
            : base(errorResp["message"])
        {
            HttpStatusCode = httpStatusCode;
            errorResp.TryGetValue("type", out _errorType);
            ApiErrorCode = errorResp["api_error_code"];

            errorResp.TryGetValue("param", out _errorParam);

            //Deprecated fields.
            ApiCode = errorResp["error_code"];
            ApiMessage = errorResp["error_msg"];
        }

        public HttpStatusCode HttpStatusCode { get; set; }

        public string Type => _errorType;

        public string ApiErrorCode { get; set; }

        public string Param => _errorParam;

        [Obsolete("Use HttpStatusCode")] public HttpStatusCode HttpCode => HttpStatusCode;

        [Obsolete("Use Code")] public string ApiCode { get; set; }

        [Obsolete("Use Param")] public string Parameter => Param;

        [Obsolete("Use Message")] public string ApiMessage { get; set; }
    }
}