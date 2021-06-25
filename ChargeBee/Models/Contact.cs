using System.IO;
using ChargeBee.Internal;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class Contact : Resource
    {
        public Contact()
        {
        }

        public Contact(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Contact(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Contact(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string FirstName => GetValue<string>("first_name", false);

        public string LastName => GetValue<string>("last_name", false);

        public string Email => GetValue<string>("email");

        public string Phone => GetValue<string>("phone", false);

        public string Label => GetValue<string>("label", false);

        public bool Enabled => GetValue<bool>("enabled");

        public bool SendAccountEmail => GetValue<bool>("send_account_email");

        public bool SendBillingEmail => GetValue<bool>("send_billing_email");

        #endregion


        #region Subclasses

        #endregion
    }
}