using System;
using System.Collections.Generic;
using System.IO;
using ChargeBee.Api;
using ChargeBee.Filters;
using ChargeBee.Internal;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class CouponSet : Resource
    {
        public CouponSet()
        {
        }

        public CouponSet(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public CouponSet(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public CouponSet(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("coupon_sets");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static AddCouponCodesRequest AddCouponCodes(string id)
        {
            var url = ApiUtil.BuildUrl("coupon_sets", CheckNull(id), "add_coupon_codes");
            return new AddCouponCodesRequest(url, HttpMethod.Post);
        }

        public static CouponSetListRequest List()
        {
            var url = ApiUtil.BuildUrl("coupon_sets");
            return new CouponSetListRequest(url);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("coupon_sets", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static UpdateRequest Update(string id)
        {
            var url = ApiUtil.BuildUrl("coupon_sets", CheckNull(id), "update");
            return new UpdateRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Delete(string id)
        {
            var url = ApiUtil.BuildUrl("coupon_sets", CheckNull(id), "delete");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> DeleteUnusedCouponCodes(string id)
        {
            var url = ApiUtil.BuildUrl("coupon_sets", CheckNull(id), "delete_unused_coupon_codes");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string CouponId => GetValue<string>("coupon_id");

        public string Name => GetValue<string>("name");

        public int? TotalCount => GetValue<int?>("total_count", false);

        public int? RedeemedCount => GetValue<int?>("redeemed_count", false);

        public int? ArchivedCount => GetValue<int?>("archived_count", false);

        public JToken MetaData => GetJToken("meta_data", false);

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

            public CreateRequest Name(string name)
            {
                MParams.Add("name", name);
                return this;
            }

            public CreateRequest Id(string id)
            {
                MParams.Add("id", id);
                return this;
            }

            public CreateRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }
        }

        public class AddCouponCodesRequest : EntityRequest<AddCouponCodesRequest>
        {
            public AddCouponCodesRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public AddCouponCodesRequest Code(List<string> code)
            {
                MParams.AddOpt("code", code);
                return this;
            }
        }

        public class CouponSetListRequest : ListRequestBase<CouponSetListRequest>
        {
            public CouponSetListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<CouponSetListRequest> Id()
            {
                return new StringFilter<CouponSetListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<CouponSetListRequest> Name()
            {
                return new StringFilter<CouponSetListRequest>("name", this).SupportsMultiOperators(true);
            }

            public StringFilter<CouponSetListRequest> CouponId()
            {
                return new StringFilter<CouponSetListRequest>("coupon_id", this).SupportsMultiOperators(true);
            }

            public NumberFilter<int, CouponSetListRequest> TotalCount()
            {
                return new("total_count", this);
            }

            public NumberFilter<int, CouponSetListRequest> RedeemedCount()
            {
                return new("redeemed_count", this);
            }

            public NumberFilter<int, CouponSetListRequest> ArchivedCount()
            {
                return new("archived_count", this);
            }
        }

        public class UpdateRequest : EntityRequest<UpdateRequest>
        {
            public UpdateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateRequest Name(string name)
            {
                MParams.AddOpt("name", name);
                return this;
            }

            public UpdateRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }
        }

        #endregion


        #region Subclasses

        #endregion
    }
}