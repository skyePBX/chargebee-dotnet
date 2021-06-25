using System.IO;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class ThirdPartyPaymentMethod : Resource
    {
        public ThirdPartyPaymentMethod()
        {
        }

        public ThirdPartyPaymentMethod(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public ThirdPartyPaymentMethod(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public ThirdPartyPaymentMethod(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        #endregion

        #region Properties

        public TypeEnum ThirdPartyPaymentMethodType => GetEnum<TypeEnum>("type");

        public GatewayEnum Gateway => GetEnum<GatewayEnum>("gateway");

        public string GatewayAccountId => GetValue<string>("gateway_account_id", false);

        public string ReferenceId => GetValue<string>("reference_id");

        #endregion


        #region Subclasses

        #endregion
    }
}