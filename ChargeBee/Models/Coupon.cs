using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Api;
using ChargeBee.Filters;
using ChargeBee.Filters.enums;
using ChargeBee.Internal;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class Coupon : Resource
    {
        public enum AddonConstraintEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "none")] None,
            [EnumMember(Value = "all")] All,
            [EnumMember(Value = "specific")] Specific,
            [EnumMember(Value = "not_applicable")] NotApplicable
        }

        [Obsolete]
        public enum ApplyDiscountOnEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "plans")] Plans,

            [EnumMember(Value = "plans_and_addons")]
            PlansAndAddons,

            [EnumMember(Value = "plans_with_quantity")]
            PlansWithQuantity,
            [EnumMember(Value = "not_applicable")] NotApplicable
        }

        public enum ApplyOnEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "invoice_amount")] InvoiceAmount,

            [EnumMember(Value = "specified_items_total")] [Obsolete]
            SpecifiedItemsTotal,

            [EnumMember(Value = "each_specified_item")]
            EachSpecifiedItem,

            [EnumMember(Value = "each_unit_of_specified_items")] [Obsolete]
            EachUnitOfSpecifiedItems
        }

        public enum DiscountTypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "fixed_amount")] FixedAmount,
            [EnumMember(Value = "percentage")] Percentage,

            [EnumMember(Value = "offer_quantity")] [Obsolete]
            OfferQuantity
        }

        public enum DurationTypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "one_time")] OneTime,
            [EnumMember(Value = "forever")] Forever,
            [EnumMember(Value = "limited_period")] LimitedPeriod
        }

        public enum PlanConstraintEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "none")] None,
            [EnumMember(Value = "all")] All,
            [EnumMember(Value = "specific")] Specific,
            [EnumMember(Value = "not_applicable")] NotApplicable
        }

        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "active")] Active,
            [EnumMember(Value = "expired")] Expired,
            [EnumMember(Value = "archived")] Archived,
            [EnumMember(Value = "deleted")] Deleted
        }

        public Coupon()
        {
        }

        public Coupon(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Coupon(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Coupon(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("coupons");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static CreateForItemsRequest CreateForItems()
        {
            var url = ApiUtil.BuildUrl("coupons", "create_for_items");
            return new CreateForItemsRequest(url, HttpMethod.Post);
        }

        public static UpdateForItemsRequest UpdateForItems(string id)
        {
            var url = ApiUtil.BuildUrl("coupons", CheckNull(id), "update_for_items");
            return new UpdateForItemsRequest(url, HttpMethod.Post);
        }

        public static CouponListRequest List()
        {
            var url = ApiUtil.BuildUrl("coupons");
            return new CouponListRequest(url);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("coupons", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static UpdateRequest Update(string id)
        {
            var url = ApiUtil.BuildUrl("coupons", CheckNull(id));
            return new UpdateRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Delete(string id)
        {
            var url = ApiUtil.BuildUrl("coupons", CheckNull(id), "delete");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static CopyRequest Copy()
        {
            var url = ApiUtil.BuildUrl("coupons", "copy");
            return new CopyRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Unarchive(string id)
        {
            var url = ApiUtil.BuildUrl("coupons", CheckNull(id), "unarchive");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string Name => GetValue<string>("name");

        public string InvoiceName => GetValue<string>("invoice_name", false);

        public DiscountTypeEnum DiscountType => GetEnum<DiscountTypeEnum>("discount_type");

        public double? DiscountPercentage => GetValue<double?>("discount_percentage", false);

        public int? DiscountAmount => GetValue<int?>("discount_amount", false);

        [Obsolete] public int? DiscountQuantity => GetValue<int?>("discount_quantity", false);

        public string CurrencyCode => GetValue<string>("currency_code", false);

        public DurationTypeEnum DurationType => GetEnum<DurationTypeEnum>("duration_type");

        public int? DurationMonth => GetValue<int?>("duration_month", false);

        public DateTime? ValidTill => GetDateTime("valid_till", false);

        public int? MaxRedemptions => GetValue<int?>("max_redemptions", false);

        public StatusEnum? Status => GetEnum<StatusEnum>("status", false);

        [Obsolete] public ApplyDiscountOnEnum ApplyDiscountOn => GetEnum<ApplyDiscountOnEnum>("apply_discount_on");

        public ApplyOnEnum ApplyOn => GetEnum<ApplyOnEnum>("apply_on");

        public PlanConstraintEnum PlanConstraint => GetEnum<PlanConstraintEnum>("plan_constraint");

        public AddonConstraintEnum AddonConstraint => GetEnum<AddonConstraintEnum>("addon_constraint");

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public DateTime? ArchivedAt => GetDateTime("archived_at", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public bool? IncludedInMrr => GetValue<bool?>("included_in_mrr", false);

        public List<string> PlanIds => GetList<string>("plan_ids");

        public List<string> AddonIds => GetList<string>("addon_ids");

        public List<CouponItemConstraint> ItemConstraints => GetResourceList<CouponItemConstraint>("item_constraints");

        public List<CouponItemConstraintCriteria> ItemConstraintCriteria =>
            GetResourceList<CouponItemConstraintCriteria>("item_constraint_criteria");

        public int? Redemptions => GetValue<int?>("redemptions", false);

        public string InvoiceNotes => GetValue<string>("invoice_notes", false);

        public JToken MetaData => GetJToken("meta_data", false);

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

            public CreateRequest InvoiceName(string invoiceName)
            {
                MParams.AddOpt("invoice_name", invoiceName);
                return this;
            }

            public CreateRequest DiscountType(DiscountTypeEnum discountType)
            {
                MParams.Add("discount_type", discountType);
                return this;
            }

            public CreateRequest DiscountAmount(int discountAmount)
            {
                MParams.AddOpt("discount_amount", discountAmount);
                return this;
            }

            public CreateRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public CreateRequest DiscountPercentage(double discountPercentage)
            {
                MParams.AddOpt("discount_percentage", discountPercentage);
                return this;
            }

            [Obsolete]
            public CreateRequest DiscountQuantity(int discountQuantity)
            {
                MParams.AddOpt("discount_quantity", discountQuantity);
                return this;
            }

            public CreateRequest ApplyOn(ApplyOnEnum applyOn)
            {
                MParams.Add("apply_on", applyOn);
                return this;
            }

            public CreateRequest DurationType(DurationTypeEnum durationType)
            {
                MParams.Add("duration_type", durationType);
                return this;
            }

            public CreateRequest DurationMonth(int durationMonth)
            {
                MParams.AddOpt("duration_month", durationMonth);
                return this;
            }

            public CreateRequest ValidTill(long validTill)
            {
                MParams.AddOpt("valid_till", validTill);
                return this;
            }

            public CreateRequest MaxRedemptions(int maxRedemptions)
            {
                MParams.AddOpt("max_redemptions", maxRedemptions);
                return this;
            }

            public CreateRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public CreateRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }

            public CreateRequest IncludedInMrr(bool includedInMrr)
            {
                MParams.AddOpt("included_in_mrr", includedInMrr);
                return this;
            }

            public CreateRequest PlanConstraint(PlanConstraintEnum planConstraint)
            {
                MParams.AddOpt("plan_constraint", planConstraint);
                return this;
            }

            public CreateRequest AddonConstraint(AddonConstraintEnum addonConstraint)
            {
                MParams.AddOpt("addon_constraint", addonConstraint);
                return this;
            }

            public CreateRequest PlanIds(List<string> planIds)
            {
                MParams.AddOpt("plan_ids", planIds);
                return this;
            }

            public CreateRequest AddonIds(List<string> addonIds)
            {
                MParams.AddOpt("addon_ids", addonIds);
                return this;
            }

            public CreateRequest Status(StatusEnum status)
            {
                MParams.AddOpt("status", status);
                return this;
            }
        }

        public class CreateForItemsRequest : EntityRequest<CreateForItemsRequest>
        {
            public CreateForItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateForItemsRequest Id(string id)
            {
                MParams.Add("id", id);
                return this;
            }

            public CreateForItemsRequest Name(string name)
            {
                MParams.Add("name", name);
                return this;
            }

            public CreateForItemsRequest InvoiceName(string invoiceName)
            {
                MParams.AddOpt("invoice_name", invoiceName);
                return this;
            }

            public CreateForItemsRequest DiscountType(DiscountTypeEnum discountType)
            {
                MParams.Add("discount_type", discountType);
                return this;
            }

            public CreateForItemsRequest DiscountAmount(int discountAmount)
            {
                MParams.AddOpt("discount_amount", discountAmount);
                return this;
            }

            public CreateForItemsRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public CreateForItemsRequest DiscountPercentage(double discountPercentage)
            {
                MParams.AddOpt("discount_percentage", discountPercentage);
                return this;
            }

            [Obsolete]
            public CreateForItemsRequest DiscountQuantity(int discountQuantity)
            {
                MParams.AddOpt("discount_quantity", discountQuantity);
                return this;
            }

            public CreateForItemsRequest ApplyOn(ApplyOnEnum applyOn)
            {
                MParams.Add("apply_on", applyOn);
                return this;
            }

            public CreateForItemsRequest DurationType(DurationTypeEnum durationType)
            {
                MParams.Add("duration_type", durationType);
                return this;
            }

            public CreateForItemsRequest DurationMonth(int durationMonth)
            {
                MParams.AddOpt("duration_month", durationMonth);
                return this;
            }

            public CreateForItemsRequest ValidTill(long validTill)
            {
                MParams.AddOpt("valid_till", validTill);
                return this;
            }

            public CreateForItemsRequest MaxRedemptions(int maxRedemptions)
            {
                MParams.AddOpt("max_redemptions", maxRedemptions);
                return this;
            }

            public CreateForItemsRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public CreateForItemsRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }

            public CreateForItemsRequest IncludedInMrr(bool includedInMrr)
            {
                MParams.AddOpt("included_in_mrr", includedInMrr);
                return this;
            }

            public CreateForItemsRequest Status(StatusEnum status)
            {
                MParams.AddOpt("status", status);
                return this;
            }

            public CreateForItemsRequest ItemConstraintConstraint(int index,
                CouponItemConstraint.ConstraintEnum itemConstraintConstraint)
            {
                MParams.Add("item_constraints[constraint][" + index + "]", itemConstraintConstraint);
                return this;
            }

            public CreateForItemsRequest ItemConstraintItemType(int index,
                CouponItemConstraint.ItemTypeEnum itemConstraintItemType)
            {
                MParams.Add("item_constraints[item_type][" + index + "]", itemConstraintItemType);
                return this;
            }

            public CreateForItemsRequest ItemConstraintItemPriceIds(int index, JArray itemConstraintItemPriceIds)
            {
                MParams.AddOpt("item_constraints[item_price_ids][" + index + "]", itemConstraintItemPriceIds);
                return this;
            }

            public CreateForItemsRequest ItemConstraintCriteriaItemType(int index,
                CouponItemConstraintCriteria.ItemTypeEnum itemConstraintCriteriaItemType)
            {
                MParams.AddOpt("item_constraint_criteria[item_type][" + index + "]", itemConstraintCriteriaItemType);
                return this;
            }

            public CreateForItemsRequest ItemConstraintCriteriaItemFamilyIds(int index,
                JArray itemConstraintCriteriaItemFamilyIds)
            {
                MParams.AddOpt("item_constraint_criteria[item_family_ids][" + index + "]",
                    itemConstraintCriteriaItemFamilyIds);
                return this;
            }

            public CreateForItemsRequest ItemConstraintCriteriaCurrencies(int index,
                JArray itemConstraintCriteriaCurrencies)
            {
                MParams.AddOpt("item_constraint_criteria[currencies][" + index + "]",
                    itemConstraintCriteriaCurrencies);
                return this;
            }

            public CreateForItemsRequest ItemConstraintCriteriaItemPricePeriods(int index,
                JArray itemConstraintCriteriaItemPricePeriods)
            {
                MParams.AddOpt("item_constraint_criteria[item_price_periods][" + index + "]",
                    itemConstraintCriteriaItemPricePeriods);
                return this;
            }
        }

        public class UpdateForItemsRequest : EntityRequest<UpdateForItemsRequest>
        {
            public UpdateForItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateForItemsRequest Name(string name)
            {
                MParams.AddOpt("name", name);
                return this;
            }

            public UpdateForItemsRequest InvoiceName(string invoiceName)
            {
                MParams.AddOpt("invoice_name", invoiceName);
                return this;
            }

            public UpdateForItemsRequest DiscountType(DiscountTypeEnum discountType)
            {
                MParams.AddOpt("discount_type", discountType);
                return this;
            }

            public UpdateForItemsRequest DiscountAmount(int discountAmount)
            {
                MParams.AddOpt("discount_amount", discountAmount);
                return this;
            }

            public UpdateForItemsRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public UpdateForItemsRequest DiscountPercentage(double discountPercentage)
            {
                MParams.AddOpt("discount_percentage", discountPercentage);
                return this;
            }

            public UpdateForItemsRequest ApplyOn(ApplyOnEnum applyOn)
            {
                MParams.AddOpt("apply_on", applyOn);
                return this;
            }

            public UpdateForItemsRequest DurationType(DurationTypeEnum durationType)
            {
                MParams.AddOpt("duration_type", durationType);
                return this;
            }

            public UpdateForItemsRequest DurationMonth(int durationMonth)
            {
                MParams.AddOpt("duration_month", durationMonth);
                return this;
            }

            public UpdateForItemsRequest ValidTill(long validTill)
            {
                MParams.AddOpt("valid_till", validTill);
                return this;
            }

            public UpdateForItemsRequest MaxRedemptions(int maxRedemptions)
            {
                MParams.AddOpt("max_redemptions", maxRedemptions);
                return this;
            }

            public UpdateForItemsRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public UpdateForItemsRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }

            public UpdateForItemsRequest IncludedInMrr(bool includedInMrr)
            {
                MParams.AddOpt("included_in_mrr", includedInMrr);
                return this;
            }

            public UpdateForItemsRequest ItemConstraintConstraint(int index,
                CouponItemConstraint.ConstraintEnum itemConstraintConstraint)
            {
                MParams.Add("item_constraints[constraint][" + index + "]", itemConstraintConstraint);
                return this;
            }

            public UpdateForItemsRequest ItemConstraintItemType(int index,
                CouponItemConstraint.ItemTypeEnum itemConstraintItemType)
            {
                MParams.Add("item_constraints[item_type][" + index + "]", itemConstraintItemType);
                return this;
            }

            public UpdateForItemsRequest ItemConstraintItemPriceIds(int index, JArray itemConstraintItemPriceIds)
            {
                MParams.AddOpt("item_constraints[item_price_ids][" + index + "]", itemConstraintItemPriceIds);
                return this;
            }

            public UpdateForItemsRequest ItemConstraintCriteriaItemType(int index,
                CouponItemConstraintCriteria.ItemTypeEnum itemConstraintCriteriaItemType)
            {
                MParams.AddOpt("item_constraint_criteria[item_type][" + index + "]", itemConstraintCriteriaItemType);
                return this;
            }

            public UpdateForItemsRequest ItemConstraintCriteriaItemFamilyIds(int index,
                JArray itemConstraintCriteriaItemFamilyIds)
            {
                MParams.AddOpt("item_constraint_criteria[item_family_ids][" + index + "]",
                    itemConstraintCriteriaItemFamilyIds);
                return this;
            }

            public UpdateForItemsRequest ItemConstraintCriteriaCurrencies(int index,
                JArray itemConstraintCriteriaCurrencies)
            {
                MParams.AddOpt("item_constraint_criteria[currencies][" + index + "]",
                    itemConstraintCriteriaCurrencies);
                return this;
            }

            public UpdateForItemsRequest ItemConstraintCriteriaItemPricePeriods(int index,
                JArray itemConstraintCriteriaItemPricePeriods)
            {
                MParams.AddOpt("item_constraint_criteria[item_price_periods][" + index + "]",
                    itemConstraintCriteriaItemPricePeriods);
                return this;
            }
        }

        public class CouponListRequest : ListRequestBase<CouponListRequest>
        {
            public CouponListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<CouponListRequest> Id()
            {
                return new StringFilter<CouponListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<CouponListRequest> Name()
            {
                return new StringFilter<CouponListRequest>("name", this).SupportsMultiOperators(true);
            }

            public EnumFilter<DiscountTypeEnum, CouponListRequest> DiscountType()
            {
                return new("discount_type", this);
            }

            public EnumFilter<DurationTypeEnum, CouponListRequest> DurationType()
            {
                return new("duration_type", this);
            }

            public EnumFilter<StatusEnum, CouponListRequest> Status()
            {
                return new("status", this);
            }

            public EnumFilter<ApplyOnEnum, CouponListRequest> ApplyOn()
            {
                return new("apply_on", this);
            }

            public TimestampFilter<CouponListRequest> CreatedAt()
            {
                return new("created_at", this);
            }

            public TimestampFilter<CouponListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }

            public CouponListRequest SortByCreatedAt(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "created_at");
                return this;
            }

            public StringFilter<CouponListRequest> CurrencyCode()
            {
                return new StringFilter<CouponListRequest>("currency_code", this).SupportsMultiOperators(true);
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

            public UpdateRequest InvoiceName(string invoiceName)
            {
                MParams.AddOpt("invoice_name", invoiceName);
                return this;
            }

            public UpdateRequest DiscountType(DiscountTypeEnum discountType)
            {
                MParams.AddOpt("discount_type", discountType);
                return this;
            }

            public UpdateRequest DiscountAmount(int discountAmount)
            {
                MParams.AddOpt("discount_amount", discountAmount);
                return this;
            }

            public UpdateRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public UpdateRequest DiscountPercentage(double discountPercentage)
            {
                MParams.AddOpt("discount_percentage", discountPercentage);
                return this;
            }

            public UpdateRequest ApplyOn(ApplyOnEnum applyOn)
            {
                MParams.AddOpt("apply_on", applyOn);
                return this;
            }

            public UpdateRequest DurationType(DurationTypeEnum durationType)
            {
                MParams.AddOpt("duration_type", durationType);
                return this;
            }

            public UpdateRequest DurationMonth(int durationMonth)
            {
                MParams.AddOpt("duration_month", durationMonth);
                return this;
            }

            public UpdateRequest ValidTill(long validTill)
            {
                MParams.AddOpt("valid_till", validTill);
                return this;
            }

            public UpdateRequest MaxRedemptions(int maxRedemptions)
            {
                MParams.AddOpt("max_redemptions", maxRedemptions);
                return this;
            }

            public UpdateRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public UpdateRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }

            public UpdateRequest IncludedInMrr(bool includedInMrr)
            {
                MParams.AddOpt("included_in_mrr", includedInMrr);
                return this;
            }

            public UpdateRequest PlanConstraint(PlanConstraintEnum planConstraint)
            {
                MParams.AddOpt("plan_constraint", planConstraint);
                return this;
            }

            public UpdateRequest AddonConstraint(AddonConstraintEnum addonConstraint)
            {
                MParams.AddOpt("addon_constraint", addonConstraint);
                return this;
            }

            public UpdateRequest PlanIds(List<string> planIds)
            {
                MParams.AddOpt("plan_ids", planIds);
                return this;
            }

            public UpdateRequest AddonIds(List<string> addonIds)
            {
                MParams.AddOpt("addon_ids", addonIds);
                return this;
            }
        }

        public class CopyRequest : EntityRequest<CopyRequest>
        {
            public CopyRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CopyRequest FromSite(string fromSite)
            {
                MParams.Add("from_site", fromSite);
                return this;
            }

            public CopyRequest IdAtFromSite(string idAtFromSite)
            {
                MParams.Add("id_at_from_site", idAtFromSite);
                return this;
            }

            public CopyRequest Id(string id)
            {
                MParams.AddOpt("id", id);
                return this;
            }

            public CopyRequest ForSiteMerging(bool forSiteMerging)
            {
                MParams.AddOpt("for_site_merging", forSiteMerging);
                return this;
            }
        }

        #endregion

        #region Subclasses

        public class CouponItemConstraint : Resource
        {
            public enum ConstraintEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "none")] None,
                [EnumMember(Value = "all")] All,
                [EnumMember(Value = "specific")] Specific,
                [EnumMember(Value = "criteria")] Criteria
            }

            public enum ItemTypeEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "plan")] Plan,
                [EnumMember(Value = "addon")] Addon,
                [EnumMember(Value = "charge")] Charge
            }

            public ItemTypeEnum ItemType()
            {
                return GetEnum<ItemTypeEnum>("item_type");
            }

            public ConstraintEnum Constraint()
            {
                return GetEnum<ConstraintEnum>("constraint");
            }

            public JArray ItemPriceIds()
            {
                return GetJArray("item_price_ids", false);
            }
        }

        public class CouponItemConstraintCriteria : Resource
        {
            public enum ItemTypeEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "plan")] Plan,
                [EnumMember(Value = "addon")] Addon,
                [EnumMember(Value = "charge")] Charge
            }

            public ItemTypeEnum ItemType()
            {
                return GetEnum<ItemTypeEnum>("item_type");
            }

            public JArray Currencies()
            {
                return GetJArray("currencies", false);
            }

            public JArray ItemFamilyIds()
            {
                return GetJArray("item_family_ids", false);
            }

            public JArray ItemPricePeriods()
            {
                return GetJArray("item_price_periods", false);
            }
        }

        #endregion
    }
}