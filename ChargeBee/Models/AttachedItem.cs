using System;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Api;
using ChargeBee.Filters;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class AttachedItem : Resource
    {
        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "active")] Active,
            [EnumMember(Value = "archived")] Archived,
            [EnumMember(Value = "deleted")] Deleted
        }

        public enum TypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "recommended")] Recommended,
            [EnumMember(Value = "mandatory")] Mandatory,
            [EnumMember(Value = "optional")] Optional
        }

        public AttachedItem()
        {
        }

        public AttachedItem(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public AttachedItem(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public AttachedItem(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateRequest Create(string id)
        {
            var url = ApiUtil.BuildUrl("items", CheckNull(id), "attached_items");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static UpdateRequest Update(string id)
        {
            var url = ApiUtil.BuildUrl("attached_items", CheckNull(id));
            return new UpdateRequest(url, HttpMethod.Post);
        }

        public static RetrieveRequest Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("attached_items", CheckNull(id));
            return new RetrieveRequest(url, HttpMethod.Get);
        }

        public static DeleteRequest Delete(string id)
        {
            var url = ApiUtil.BuildUrl("attached_items", CheckNull(id), "delete");
            return new DeleteRequest(url, HttpMethod.Post);
        }

        public static AttachedItemListRequest List(string id)
        {
            var url = ApiUtil.BuildUrl("items", CheckNull(id), "attached_items");
            return new AttachedItemListRequest(url);
        }

        [Obsolete]
        public static AttachedItemListInternalRequest ListInternal()
        {
            var url = ApiUtil.BuildUrl("attached_items", "list_internal");
            return new AttachedItemListInternalRequest(url);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string ParentItemId => GetValue<string>("parent_item_id");

        public string ItemId => GetValue<string>("item_id");

        public TypeEnum AttachedItemType => GetEnum<TypeEnum>("type");

        public StatusEnum? Status => GetEnum<StatusEnum>("status", false);

        public int? Quantity => GetValue<int?>("quantity", false);

        public int? BillingCycles => GetValue<int?>("billing_cycles", false);

        public ChargeOnEventEnum ChargeOnEvent => GetEnum<ChargeOnEventEnum>("charge_on_event");

        public bool ChargeOnce => GetValue<bool>("charge_once");

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        #endregion

        #region Requests

        public class CreateRequest : EntityRequest<CreateRequest>
        {
            public CreateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateRequest ItemId(string itemId)
            {
                MParams.Add("item_id", itemId);
                return this;
            }

            public CreateRequest Type(TypeEnum type)
            {
                MParams.AddOpt("type", type);
                return this;
            }

            public CreateRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CreateRequest Quantity(int quantity)
            {
                MParams.AddOpt("quantity", quantity);
                return this;
            }

            public CreateRequest ChargeOnEvent(ChargeOnEventEnum chargeOnEvent)
            {
                MParams.AddOpt("charge_on_event", chargeOnEvent);
                return this;
            }

            public CreateRequest ChargeOnce(bool chargeOnce)
            {
                MParams.AddOpt("charge_once", chargeOnce);
                return this;
            }
        }

        public class UpdateRequest : EntityRequest<UpdateRequest>
        {
            public UpdateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateRequest ParentItemId(string parentItemId)
            {
                MParams.Add("parent_item_id", parentItemId);
                return this;
            }

            public UpdateRequest Type(TypeEnum type)
            {
                MParams.AddOpt("type", type);
                return this;
            }

            public UpdateRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public UpdateRequest Quantity(int quantity)
            {
                MParams.AddOpt("quantity", quantity);
                return this;
            }

            public UpdateRequest ChargeOnEvent(ChargeOnEventEnum chargeOnEvent)
            {
                MParams.AddOpt("charge_on_event", chargeOnEvent);
                return this;
            }

            public UpdateRequest ChargeOnce(bool chargeOnce)
            {
                MParams.AddOpt("charge_once", chargeOnce);
                return this;
            }
        }

        public class RetrieveRequest : EntityRequest<RetrieveRequest>
        {
            public RetrieveRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RetrieveRequest ParentItemId(string parentItemId)
            {
                MParams.Add("parent_item_id", parentItemId);
                return this;
            }
        }

        public class DeleteRequest : EntityRequest<DeleteRequest>
        {
            public DeleteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public DeleteRequest ParentItemId(string parentItemId)
            {
                MParams.Add("parent_item_id", parentItemId);
                return this;
            }
        }

        public class AttachedItemListRequest : ListRequestBase<AttachedItemListRequest>
        {
            public AttachedItemListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<AttachedItemListRequest> Id()
            {
                return new StringFilter<AttachedItemListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<AttachedItemListRequest> ItemId()
            {
                return new StringFilter<AttachedItemListRequest>("item_id", this).SupportsMultiOperators(true);
            }

            public EnumFilter<TypeEnum, AttachedItemListRequest> Type()
            {
                return new("type", this);
            }

            public EnumFilter<ItemTypeEnum, AttachedItemListRequest> ItemType()
            {
                return new("item_type", this);
            }

            public EnumFilter<ChargeOnEventEnum, AttachedItemListRequest> ChargeOnEvent()
            {
                return new("charge_on_event", this);
            }
        }

        public class AttachedItemListInternalRequest : ListRequestBase<AttachedItemListInternalRequest>
        {
            public AttachedItemListInternalRequest(string url)
                : base(url)
            {
            }

            public StringFilter<AttachedItemListInternalRequest> Id()
            {
                return new StringFilter<AttachedItemListInternalRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<AttachedItemListInternalRequest> ItemId()
            {
                return new StringFilter<AttachedItemListInternalRequest>("item_id", this).SupportsMultiOperators(true);
            }

            public EnumFilter<TypeEnum, AttachedItemListInternalRequest> Type()
            {
                return new("type", this);
            }

            public EnumFilter<ItemTypeEnum, AttachedItemListInternalRequest> ItemType()
            {
                return new("item_type", this);
            }

            public EnumFilter<ChargeOnEventEnum, AttachedItemListInternalRequest> ChargeOnEvent()
            {
                return new("charge_on_event", this);
            }

            public StringFilter<AttachedItemListInternalRequest> ParentItemId()
            {
                return new StringFilter<AttachedItemListInternalRequest>("parent_item_id", this)
                    .SupportsMultiOperators(true);
            }
        }

        #endregion

        #region Subclasses

        #endregion
    }
}