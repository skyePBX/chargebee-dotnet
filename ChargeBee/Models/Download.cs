using System;
using System.IO;
using ChargeBee.Internal;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class Download : Resource
    {
        public Download()
        {
        }

        public Download(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Download(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Download(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        #endregion

        #region Properties

        public string DownloadUrl => GetValue<string>("download_url");

        public DateTime ValidTill => (DateTime) GetDateTime("valid_till");

        #endregion


        #region Subclasses

        #endregion
    }
}