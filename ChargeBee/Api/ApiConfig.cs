using System;
using System.Text;

namespace ChargeBee.Api
{
    public sealed class ApiConfig
    {
        public static string DomainSuffix = "chargebee.com";
        public static string Proto = "https";
        public static string Version = "2.8.9-skypbx";
        public static readonly string ApiVersion = "v2";

        private static volatile ApiConfig _mInstance;

        private ApiConfig(string siteName, string apiKey)
        {
            Charset = Encoding.UTF8.WebName;
            ConnectTimeout = 15000;
            TimeTravelMillis = 3000;
            ExportSleepMillis = 10000;
            SiteName = siteName;
            ApiKey = apiKey;
        }

        public static int TimeTravelMillis { get; set; }
        public static int ExportSleepMillis { get; set; }

        public string ApiKey { get; set; }
        public string SiteName { get; set; }
        public string Charset { get; set; }
        public static int ConnectTimeout { get; set; }

        public string ApiBaseUrl =>
            $"{Proto}://{SiteName}.{DomainSuffix}/api/{ApiVersion}";

        public string AuthValue =>
            $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ApiKey}:"))}";

        public static ApiConfig Instance
        {
            get
            {
                if (_mInstance == null)
                    throw new Exception("Not yet configured!");

                return _mInstance;
            }
        }

        public static void Configure(string siteName, string apiKey)
        {
            if (string.IsNullOrEmpty(siteName))
                throw new ArgumentException("Site name can't be empty!");

            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("Api key can't be empty!");

            _mInstance = new ApiConfig(siteName, apiKey);
        }
    }
}