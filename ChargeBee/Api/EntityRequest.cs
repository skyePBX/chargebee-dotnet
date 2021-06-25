using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargeBee.Api
{
    public class EntityRequest<T>
    {
        protected Dictionary<string, string> Headers = new();
        protected HttpMethod MMethod;
        protected Params MParams = new();
        protected string MUrl;

        public EntityRequest(string url, HttpMethod method)
        {
            MUrl = url;
            MMethod = method;
        }

        public Params Params()
        {
            return MParams;
        }

        public T Param(string paramName, object value)
        {
            MParams.AddOpt(paramName, value);
            return (T) Convert.ChangeType(this, typeof(T));
        }

        public T Header(string headerName, string headerValue)
        {
            Headers.Add(headerName, headerValue);
            return (T) Convert.ChangeType(this, typeof(T));
        }

        public EntityResult Request()
        {
            return Request(ApiConfig.Instance);
        }

        public Task<EntityResult> RequestAsync()
        {
            return RequestAsync(ApiConfig.Instance);
        }

        public EntityResult Request(ApiConfig env)
        {
            switch (MMethod)
            {
                case HttpMethod.Get:
                    return ApiUtil.Get(MUrl, MParams, Headers, env);
                case HttpMethod.Post:
                    return ApiUtil.Post(MUrl, MParams, Headers, env);
                default:
                    throw new NotImplementedException(
                        $"HTTP method {Enum.GetName(typeof(HttpMethod), MMethod)} is not implemented");
            }
        }

        public Task<EntityResult> RequestAsync(ApiConfig env)
        {
            switch (MMethod)
            {
                case HttpMethod.Get:
                    return ApiUtil.GetAsync(MUrl, MParams, Headers, env);
                case HttpMethod.Post:
                    return ApiUtil.PostAsync(MUrl, MParams, Headers, env);
                default:
                    throw new NotImplementedException(
                        $"HTTP method {Enum.GetName(typeof(HttpMethod), MMethod)} is not implemented");
            }
        }
    }
}