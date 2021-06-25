using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Api;
using ChargeBee.Filters;
using ChargeBee.Internal;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class DifferentialPrice : Resource
    {
        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "active")] Active,
            [EnumMember(Value = "deleted")] Deleted
        }

        public DifferentialPrice()
        {
        }

        public DifferentialPrice(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public DifferentialPrice(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public DifferentialPrice(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateRequest Create(string id)
        {
            var url = ApiUtil.BuildUrl("item_prices", CheckNull(id), "differential_prices");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static RetrieveRequest Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("differential_prices", CheckNull(id));
            return new RetrieveRequest(url, HttpMethod.Get);
        }

        public static UpdateRequest Update(string id)
        {
            var url = ApiUtil.BuildUrl("differential_prices", CheckNull(id));
            return new UpdateRequest(url, HttpMethod.Post);
        }

        public static DeleteRequest Delete(string id)
        {
            var url = ApiUtil.BuildUrl("differential_prices", CheckNull(id), "delete");
            return new DeleteRequest(url, HttpMethod.Post);
        }

        public static DifferentialPriceListRequest List()
        {
            var url = ApiUtil.BuildUrl("differential_prices");
            return new DifferentialPriceListRequest(url);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string ItemPriceId => GetValue<string>("item_price_id");

        public string ParentItemId => GetValue<string>("parent_item_id");

        public int? Price => GetValue<int?>("price", false);

        public StatusEnum? Status => GetEnum<StatusEnum>("status", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public DateTime ModifiedAt => (DateTime) GetDateTime("modified_at");

        public List<DifferentialPriceTier> Tiers => GetResourceList<DifferentialPriceTier>("tiers");

        public string CurrencyCode => GetValue<string>("currency_code");

        public List<DifferentialPriceParentPeriod> ParentPeriods =>
            GetResourceList<DifferentialPriceParentPeriod>("parent_periods");

        #endregion

        #region Requests

        public class CreateRequest : EntityRequest<CreateRequest>
        {
            public CreateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateRequest ParentItemId(string parentItemId)
            {
                MParams.Add("parent_item_id", parentItemId);
                return this;
            }

            public CreateRequest Price(int price)
            {
                MParams.AddOpt("price", price);
                return this;
            }

            public CreateRequest ParentPeriodPeriodUnit(int index,
                DifferentialPriceParentPeriod.PeriodUnitEnum parentPeriodPeriodUnit)
            {
                MParams.Add("parent_periods[period_unit][" + index + "]", parentPeriodPeriodUnit);
                return this;
            }

            public CreateRequest ParentPeriodPeriod(int index, JArray parentPeriodPeriod)
            {
                MParams.AddOpt("parent_periods[period][" + index + "]", parentPeriodPeriod);
                return this;
            }

            public CreateRequest TierStartingUnit(int index, int tierStartingUnit)
            {
                MParams.AddOpt("tiers[starting_unit][" + index + "]", tierStartingUnit);
                return this;
            }

            public CreateRequest TierEndingUnit(int index, int tierEndingUnit)
            {
                MParams.AddOpt("tiers[ending_unit][" + index + "]", tierEndingUnit);
                return this;
            }

            public CreateRequest TierPrice(int index, int tierPrice)
            {
                MParams.AddOpt("tiers[price][" + index + "]", tierPrice);
                return this;
            }
        }

        public class RetrieveRequest : EntityRequest<RetrieveRequest>
        {
            public RetrieveRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RetrieveRequest ItemPriceId(string itemPriceId)
            {
                MParams.Add("item_price_id", itemPriceId);
                return this;
            }
        }

        public class UpdateRequest : EntityRequest<UpdateRequest>
        {
            public UpdateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateRequest ItemPriceId(string itemPriceId)
            {
                MParams.Add("item_price_id", itemPriceId);
                return this;
            }

            public UpdateRequest Price(int price)
            {
                MParams.AddOpt("price", price);
                return this;
            }

            public UpdateRequest ParentPeriodPeriodUnit(int index,
                DifferentialPriceParentPeriod.PeriodUnitEnum parentPeriodPeriodUnit)
            {
                MParams.Add("parent_periods[period_unit][" + index + "]", parentPeriodPeriodUnit);
                return this;
            }

            public UpdateRequest ParentPeriodPeriod(int index, JArray parentPeriodPeriod)
            {
                MParams.AddOpt("parent_periods[period][" + index + "]", parentPeriodPeriod);
                return this;
            }

            public UpdateRequest TierStartingUnit(int index, int tierStartingUnit)
            {
                MParams.AddOpt("tiers[starting_unit][" + index + "]", tierStartingUnit);
                return this;
            }

            public UpdateRequest TierEndingUnit(int index, int tierEndingUnit)
            {
                MParams.AddOpt("tiers[ending_unit][" + index + "]", tierEndingUnit);
                return this;
            }

            public UpdateRequest TierPrice(int index, int tierPrice)
            {
                MParams.AddOpt("tiers[price][" + index + "]", tierPrice);
                return this;
            }
        }

        public class DeleteRequest : EntityRequest<DeleteRequest>
        {
            public DeleteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public DeleteRequest ItemPriceId(string itemPriceId)
            {
                MParams.Add("item_price_id", itemPriceId);
                return this;
            }
        }

        public class DifferentialPriceListRequest : ListRequestBase<DifferentialPriceListRequest>
        {
            public DifferentialPriceListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<DifferentialPriceListRequest> ItemPriceId()
            {
                return new StringFilter<DifferentialPriceListRequest>("item_price_id", this)
                    .SupportsMultiOperators(true);
            }

            public StringFilter<DifferentialPriceListRequest> ItemId()
            {
                return new StringFilter<DifferentialPriceListRequest>("item_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<DifferentialPriceListRequest> Id()
            {
                return new StringFilter<DifferentialPriceListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<DifferentialPriceListRequest> ParentItemId()
            {
                return new StringFilter<DifferentialPriceListRequest>("parent_item_id", this)
                    .SupportsMultiOperators(true);
            }
        }

        #endregion

        #region Subclasses

        public class DifferentialPriceTier : Resource
        {
            public int StartingUnit()
            {
                return GetValue<int>("starting_unit");
            }

            public int? EndingUnit()
            {
                return GetValue<int?>("ending_unit", false);
            }

            public int Price()
            {
                return GetValue<int>("price");
            }

            public string StartingUnitInDecimal()
            {
                return GetValue<string>("starting_unit_in_decimal", false);
            }

            public string EndingUnitInDecimal()
            {
                return GetValue<string>("ending_unit_in_decimal", false);
            }

            public string PriceInDecimal()
            {
                return GetValue<string>("price_in_decimal", false);
            }
        }

        public class DifferentialPriceParentPeriod : Resource
        {
            public enum PeriodUnitEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "day")] Day,
                [EnumMember(Value = "week")] Week,
                [EnumMember(Value = "month")] Month,
                [EnumMember(Value = "year")] Year
            }

            public PeriodUnitEnum PeriodUnit()
            {
                return GetEnum<PeriodUnitEnum>("period_unit");
            }

            public JArray Period()
            {
                return GetJArray("period", false);
            }
        }

        #endregion
    }
}