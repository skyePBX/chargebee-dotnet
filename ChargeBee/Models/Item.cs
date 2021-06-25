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
    public class Item : Resource
    {
        public enum ItemApplicabilityEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "all")] All,
            [EnumMember(Value = "restricted")] Restricted
        }

        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "active")] Active,

            [EnumMember(Value = "archived")] [Obsolete]
            Archived,
            [EnumMember(Value = "deleted")] Deleted
        }

        public enum TypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "plan")] Plan,
            [EnumMember(Value = "addon")] Addon,
            [EnumMember(Value = "charge")] Charge
        }

        public enum UsageCalculationEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "sum_of_usages")] SumOfUsages,
            [EnumMember(Value = "last_usage")] LastUsage
        }

        public Item()
        {
        }

        public Item(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Item(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Item(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Subclasses

        public class ItemApplicableItem : Resource
        {
            public string Id()
            {
                return GetValue<string>("id", false);
            }
        }

        #endregion

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("items");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("items", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static UpdateRequest Update(string id)
        {
            var url = ApiUtil.BuildUrl("items", CheckNull(id));
            return new UpdateRequest(url, HttpMethod.Post);
        }

        public static ItemListRequest List()
        {
            var url = ApiUtil.BuildUrl("items");
            return new ItemListRequest(url);
        }

        public static EntityRequest<Type> Delete(string id)
        {
            var url = ApiUtil.BuildUrl("items", CheckNull(id), "delete");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string Name => GetValue<string>("name");

        public string Description => GetValue<string>("description", false);

        public StatusEnum? Status => GetEnum<StatusEnum>("status", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public string ItemFamilyId => GetValue<string>("item_family_id", false);

        public TypeEnum ItemType => GetEnum<TypeEnum>("type");

        public bool? IsShippable => GetValue<bool?>("is_shippable", false);

        public bool IsGiftable => GetValue<bool>("is_giftable");

        public string RedirectUrl => GetValue<string>("redirect_url", false);

        public bool EnabledForCheckout => GetValue<bool>("enabled_for_checkout");

        public bool EnabledInPortal => GetValue<bool>("enabled_in_portal");

        public bool? IncludedInMrr => GetValue<bool?>("included_in_mrr", false);

        public ItemApplicabilityEnum ItemApplicability => GetEnum<ItemApplicabilityEnum>("item_applicability");

        public string GiftClaimRedirectUrl => GetValue<string>("gift_claim_redirect_url", false);

        public string Unit => GetValue<string>("unit", false);

        public bool Metered => GetValue<bool>("metered");

        public UsageCalculationEnum? UsageCalculation => GetEnum<UsageCalculationEnum>("usage_calculation", false);

        public List<ItemApplicableItem> ApplicableItems => GetResourceList<ItemApplicableItem>("applicable_items");

        public JToken Metadata => GetJToken("metadata", false);

        #endregion

        #region Requests

        public class CreateRequest : EntityRequest<CreateRequest>
        {
            public CreateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateRequest Id(string id)
            {
                MParams.Add("id", id);
                return this;
            }

            public CreateRequest Name(string name)
            {
                MParams.Add("name", name);
                return this;
            }

            public CreateRequest Type(TypeEnum type)
            {
                MParams.Add("type", type);
                return this;
            }

            public CreateRequest Description(string description)
            {
                MParams.AddOpt("description", description);
                return this;
            }

            public CreateRequest ItemFamilyId(string itemFamilyId)
            {
                MParams.AddOpt("item_family_id", itemFamilyId);
                return this;
            }

            public CreateRequest IsGiftable(bool isGiftable)
            {
                MParams.AddOpt("is_giftable", isGiftable);
                return this;
            }

            public CreateRequest IsShippable(bool isShippable)
            {
                MParams.AddOpt("is_shippable", isShippable);
                return this;
            }

            public CreateRequest EnabledInPortal(bool enabledInPortal)
            {
                MParams.AddOpt("enabled_in_portal", enabledInPortal);
                return this;
            }

            public CreateRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public CreateRequest EnabledForCheckout(bool enabledForCheckout)
            {
                MParams.AddOpt("enabled_for_checkout", enabledForCheckout);
                return this;
            }

            public CreateRequest ItemApplicability(ItemApplicabilityEnum itemApplicability)
            {
                MParams.AddOpt("item_applicability", itemApplicability);
                return this;
            }

            public CreateRequest ApplicableItems(List<string> applicableItems)
            {
                MParams.AddOpt("applicable_items", applicableItems);
                return this;
            }

            public CreateRequest Unit(string unit)
            {
                MParams.AddOpt("unit", unit);
                return this;
            }

            public CreateRequest GiftClaimRedirectUrl(string giftClaimRedirectUrl)
            {
                MParams.AddOpt("gift_claim_redirect_url", giftClaimRedirectUrl);
                return this;
            }

            public CreateRequest IncludedInMrr(bool includedInMrr)
            {
                MParams.AddOpt("included_in_mrr", includedInMrr);
                return this;
            }

            public CreateRequest Metered(bool metered)
            {
                MParams.AddOpt("metered", metered);
                return this;
            }

            public CreateRequest UsageCalculation(UsageCalculationEnum usageCalculation)
            {
                MParams.AddOpt("usage_calculation", usageCalculation);
                return this;
            }

            public CreateRequest Metadata(JToken metadata)
            {
                MParams.AddOpt("metadata", metadata);
                return this;
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

            public UpdateRequest Description(string description)
            {
                MParams.AddOpt("description", description);
                return this;
            }

            public UpdateRequest IsShippable(bool isShippable)
            {
                MParams.AddOpt("is_shippable", isShippable);
                return this;
            }

            public UpdateRequest ItemFamilyId(string itemFamilyId)
            {
                MParams.AddOpt("item_family_id", itemFamilyId);
                return this;
            }

            public UpdateRequest EnabledInPortal(bool enabledInPortal)
            {
                MParams.AddOpt("enabled_in_portal", enabledInPortal);
                return this;
            }

            public UpdateRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public UpdateRequest EnabledForCheckout(bool enabledForCheckout)
            {
                MParams.AddOpt("enabled_for_checkout", enabledForCheckout);
                return this;
            }

            public UpdateRequest ItemApplicability(ItemApplicabilityEnum itemApplicability)
            {
                MParams.AddOpt("item_applicability", itemApplicability);
                return this;
            }

            [Obsolete]
            public UpdateRequest ClearApplicableItems(bool clearApplicableItems)
            {
                MParams.AddOpt("clear_applicable_items", clearApplicableItems);
                return this;
            }

            public UpdateRequest ApplicableItems(List<string> applicableItems)
            {
                MParams.AddOpt("applicable_items", applicableItems);
                return this;
            }

            public UpdateRequest Unit(string unit)
            {
                MParams.AddOpt("unit", unit);
                return this;
            }

            public UpdateRequest GiftClaimRedirectUrl(string giftClaimRedirectUrl)
            {
                MParams.AddOpt("gift_claim_redirect_url", giftClaimRedirectUrl);
                return this;
            }

            public UpdateRequest Metadata(JToken metadata)
            {
                MParams.AddOpt("metadata", metadata);
                return this;
            }

            public UpdateRequest IncludedInMrr(bool includedInMrr)
            {
                MParams.AddOpt("included_in_mrr", includedInMrr);
                return this;
            }
        }

        public class ItemListRequest : ListRequestBase<ItemListRequest>
        {
            public ItemListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<ItemListRequest> Id()
            {
                return new StringFilter<ItemListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<ItemListRequest> ItemFamilyId()
            {
                return new StringFilter<ItemListRequest>("item_family_id", this).SupportsMultiOperators(true);
            }

            public EnumFilter<TypeEnum, ItemListRequest> Type()
            {
                return new("type", this);
            }

            public StringFilter<ItemListRequest> Name()
            {
                return new("name", this);
            }

            public EnumFilter<ItemApplicabilityEnum, ItemListRequest> ItemApplicability()
            {
                return new("item_applicability", this);
            }

            public EnumFilter<StatusEnum, ItemListRequest> Status()
            {
                return new("status", this);
            }

            public BooleanFilter<ItemListRequest> IsGiftable()
            {
                return new("is_giftable", this);
            }

            public TimestampFilter<ItemListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }

            public BooleanFilter<ItemListRequest> EnabledForCheckout()
            {
                return new("enabled_for_checkout", this);
            }

            public BooleanFilter<ItemListRequest> EnabledInPortal()
            {
                return new("enabled_in_portal", this);
            }

            public BooleanFilter<ItemListRequest> Metered()
            {
                return new("metered", this);
            }

            public EnumFilter<UsageCalculationEnum, ItemListRequest> UsageCalculation()
            {
                return new("usage_calculation", this);
            }
        }

        #endregion
    }
}