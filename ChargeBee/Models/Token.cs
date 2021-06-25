using System;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class Token : Resource
    {
        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "new")] New,
            [EnumMember(Value = "expired")] Expired,
            [EnumMember(Value = "consumed")] Consumed
        }

        public enum VaultEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "spreedly")] Spreedly,
            [EnumMember(Value = "gateway")] Gateway
        }

        public Token()
        {
        }

        public Token(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Token(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Token(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public GatewayEnum Gateway => GetEnum<GatewayEnum>("gateway");

        public string GatewayAccountId => GetValue<string>("gateway_account_id");

        public PaymentMethodTypeEnum PaymentMethodType => GetEnum<PaymentMethodTypeEnum>("payment_method_type");

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public string IdAtVault => GetValue<string>("id_at_vault");

        public VaultEnum Vault => GetEnum<VaultEnum>("vault");

        public string IpAddress => GetValue<string>("ip_address", false);

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public DateTime? ExpiredAt => GetDateTime("expired_at", false);

        #endregion

        #region Subclasses

        #endregion
    }
}