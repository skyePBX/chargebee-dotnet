using System;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Api;
using ChargeBee.Filters;
using ChargeBee.Internal;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class CouponCode : Resource
    {
        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "not_redeemed")] NotRedeemed,
            [EnumMember(Value = "redeemed")] Redeemed,
            [EnumMember(Value = "archived")] Archived
        }

        public CouponCode()
        {
        }

        public CouponCode(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public CouponCode(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public CouponCode(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        [Obsolete]
        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("coupon_codes");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("coupon_codes", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static CouponCodeListRequest List()
        {
            var url = ApiUtil.BuildUrl("coupon_codes");
            return new CouponCodeListRequest(url);
        }

        public static EntityRequest<Type> Archive(string id)
        {
            var url = ApiUtil.BuildUrl("coupon_codes", CheckNull(id), "archive");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Code => GetValue<string>("code");

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public string CouponId => GetValue<string>("coupon_id");

        public string CouponSetId => GetValue<string>("coupon_set_id");

        public string CouponSetName => GetValue<string>("coupon_set_name");

        #endregion

        #region Requests

        public class CreateRequest : EntityRequest<CreateRequest>
        {
            public CreateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateRequest CouponId(string couponId)
            {
                MParams.Add("coupon_id", couponId);
                return this;
            }

            public CreateRequest CouponSetName(string couponSetName)
            {
                MParams.Add("coupon_set_name", couponSetName);
                return this;
            }

            public CreateRequest Code(string code)
            {
                MParams.Add("code", code);
                return this;
            }
        }

        public class CouponCodeListRequest : ListRequestBase<CouponCodeListRequest>
        {
            public CouponCodeListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<CouponCodeListRequest> Code()
            {
                return new StringFilter<CouponCodeListRequest>("code", this).SupportsMultiOperators(true);
            }

            public StringFilter<CouponCodeListRequest> CouponId()
            {
                return new StringFilter<CouponCodeListRequest>("coupon_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<CouponCodeListRequest> CouponSetName()
            {
                return new("coupon_set_name", this);
            }

            public EnumFilter<StatusEnum, CouponCodeListRequest> Status()
            {
                return new("status", this);
            }
        }

        #endregion

        #region Subclasses

        #endregion
    }
}