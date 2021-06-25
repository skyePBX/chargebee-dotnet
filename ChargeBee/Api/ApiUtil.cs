using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ChargeBee.Exceptions;
using Newtonsoft.Json;

namespace ChargeBee.Api
{
    public static class ApiUtil
    {
        private static readonly DateTime MUnixTime = new(1970, 1, 1);

        private static readonly HttpClient HttpClient = new()
            {Timeout = TimeSpan.FromMilliseconds(ApiConfig.ConnectTimeout)};

        public static string BuildUrl(params string[] paths)
        {
            var sb = new StringBuilder(ApiConfig.Instance.ApiBaseUrl);

            foreach (var path in paths) sb.Append('/').Append(Uri.EscapeUriString(path));
            return sb.ToString();
        }

        private static HttpRequestMessage BuildRequest(string url, HttpMethod method, Params parameters, ApiConfig env)
        {
            HttpRequestMessage request;
            var meth = new System.Net.Http.HttpMethod(method.ToString());
            if (method.Equals(HttpMethod.Post))
            {
                var paramBytes = Encoding.GetEncoding(env.Charset).GetBytes(parameters.GetQuery(false));
                var postData = Encoding.GetEncoding(env.Charset).GetString(paramBytes, 0, paramBytes.Length);
                request = new HttpRequestMessage(meth, new Uri(url))
                {
                    Content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded")
                };
            }
            else
            {
                request = new HttpRequestMessage(meth, new Uri(url));
            }

            return request;
        }

        private static HttpRequestMessage GetRequestMessage(string url, HttpMethod method, Params parameters,
            Dictionary<string, string> headers, ApiConfig env)
        {
            var request = BuildRequest(url, method, parameters, env);
            AddHeaders(request, env);
            AddCustomHeaders(request, headers);
            return request;
        }

        private static void AddHeaders(HttpRequestMessage request, ApiConfig env)
        {
            request.Headers.Add("Accept-Charset", env.Charset);
            request.Headers.Add("Authorization", env.AuthValue);
            request.Headers.Add("Accept", "application/json");
            request.Headers.UserAgent.ParseAdd("ChargeBee-DotNet-Client v" + ApiConfig.Version);
#if NET45
            request.Headers.Add("Lang-Version",Environment.Version.ToString());
            request.Headers.Add("OS-Version",Environment.OSVersion.ToString());

#else
            request.Headers.Add("Lang-Version", RuntimeInformation.FrameworkDescription);
            request.Headers.Add("OS-Version", RuntimeInformation.OSDescription);

#endif
        }

        private static void AddCustomHeaders(HttpRequestMessage request, Dictionary<string, string> headers)
        {
            foreach (var entry in headers) AddHeader(request, entry.Key, entry.Value);
        }

        private static void AddHeader(HttpRequestMessage request, string headerName, string value)
        {
            request.Headers.Add(headerName, value);
        }

        private static void HandleException(HttpResponseMessage response)
        {
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Dictionary<string, string> errorJson;
            try
            {
                errorJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            }
            catch (JsonException e)
            {
                throw new ArgumentException("Not in JSON format. Probably not a ChargeBee response. \n " + content, e);
            }

            if (errorJson == null) return;

            errorJson.TryGetValue("type", out var type);
            throw type switch
            {
                "payment" => new PaymentException(response.StatusCode, errorJson),
                "operation_failed" => new OperationFailedException(response.StatusCode, errorJson),
                "invalid_request" => new InvalidRequestException(response.StatusCode, errorJson),
                _ => new ApiException(response.StatusCode, errorJson)
            };
        }

        private static EntityResult GetEntityResult(string url, Params parameters, Dictionary<string, string> headers,
            ApiConfig env, HttpMethod meth)
        {
            return GetEntityResultAsync(url, parameters, headers, env, meth).GetAwaiter()
                .GetResult();
        }

        private static async Task<EntityResult> GetEntityResultAsync(string url, Params parameters,
            Dictionary<string, string> headers, ApiConfig env, HttpMethod meth)
        {
            var request = GetRequestMessage(url, meth, parameters, headers, env);
            var response = await HttpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = new EntityResult(response.StatusCode, json);
                return result;
            }

            HandleException(response);
            return null;
        }

        public static EntityResult Post(string url, Params parameters, Dictionary<string, string> headers,
            ApiConfig env)
        {
            return GetEntityResult(url, parameters, headers, env, HttpMethod.Post);
        }

        public static Task<EntityResult> PostAsync(string url, Params parameters, Dictionary<string, string> headers,
            ApiConfig env)
        {
            return GetEntityResultAsync(url, parameters, headers, env, HttpMethod.Post);
        }

        public static EntityResult Get(string url, Params parameters, Dictionary<string, string> headers, ApiConfig env)
        {
            url = $"{url}?{parameters.GetQuery(false)}";
            return GetEntityResult(url, parameters, headers, env, HttpMethod.Get);
        }

        public static Task<EntityResult> GetAsync(string url, Params parameters, Dictionary<string, string> headers,
            ApiConfig env)
        {
            url = $"{url}?{parameters.GetQuery(false)}";
            return GetEntityResultAsync(url, parameters, headers, env, HttpMethod.Get);
        }

        public static ListResult GetList(string url, Params parameters, Dictionary<string, string> headers,
            ApiConfig env)
        {
            return GetListAsync(url, parameters, headers, env).GetAwaiter().GetResult();
        }

        public static async Task<ListResult> GetListAsync(string url, Params parameters,
            Dictionary<string, string> headers, ApiConfig env)
        {
            url = $"{url}?{parameters.GetQuery(true)}";
            var request = GetRequestMessage(url, HttpMethod.Get, parameters, headers, env);
            var response = await HttpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = new ListResult(response.StatusCode, json);
                return result;
            }

            HandleException(response);
            return null;
        }

        public static DateTime ConvertFromTimestamp(long timestamp)
        {
            return MUnixTime.AddSeconds(timestamp).ToLocalTime();
        }

        public static long? ConvertToTimestamp(DateTime? t)
        {
            if (t == null) return null;

            var dtutc = ((DateTime) t).ToUniversalTime();

            if (dtutc < MUnixTime) throw new ArgumentException("Time can't be before 1970, January 1!");

            return (long) (dtutc - MUnixTime).TotalSeconds;
        }
    }

    /// <summary>
    ///     HTTP method
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        ///     DELETE
        /// </summary>
        Delete,

        /// <summary>
        ///     GET
        /// </summary>
        Get,

        /// <summary>
        ///     POST
        /// </summary>
        Post,

        /// <summary>
        ///     PUT
        /// </summary>
        Put
    }
}