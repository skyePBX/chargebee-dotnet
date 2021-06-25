using System;
using ChargeBee.Api;
using ChargeBee.Filters;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;

namespace ChargeBee.Models
{
    public class Usage : Resource
    {
        #region Methods

        public static CreateRequest Create(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "usages");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static RetrieveRequest Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "usage");
            return new RetrieveRequest(url, HttpMethod.Get);
        }

        public static DeleteRequest Delete(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "delete_usage");
            return new DeleteRequest(url, HttpMethod.Post);
        }

        public static UsageListRequest List()
        {
            var url = ApiUtil.BuildUrl("usages");
            return new UsageListRequest(url);
        }

        public static PdfRequest Pdf()
        {
            var url = ApiUtil.BuildUrl("usages", "pdf");
            return new PdfRequest(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id", false);

        public DateTime UsageDate => (DateTime) GetDateTime("usage_date");

        public string SubscriptionId => GetValue<string>("subscription_id");

        public string ItemPriceId => GetValue<string>("item_price_id");

        public string InvoiceId => GetValue<string>("invoice_id", false);

        public string LineItemId => GetValue<string>("line_item_id", false);

        public string Quantity => GetValue<string>("quantity");

        public SourceEnum? Source => GetEnum<SourceEnum>("source", false);

        public string Note => GetValue<string>("note", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

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
                MParams.AddOpt("id", id);
                return this;
            }

            public CreateRequest ItemPriceId(string itemPriceId)
            {
                MParams.Add("item_price_id", itemPriceId);
                return this;
            }

            public CreateRequest Quantity(string quantity)
            {
                MParams.Add("quantity", quantity);
                return this;
            }

            public CreateRequest UsageDate(long usageDate)
            {
                MParams.Add("usage_date", usageDate);
                return this;
            }

            [Obsolete]
            public CreateRequest DedupeOption(DedupeOptionEnum dedupeOption)
            {
                MParams.AddOpt("dedupe_option", dedupeOption);
                return this;
            }

            public CreateRequest Note(string note)
            {
                MParams.AddOpt("note", note);
                return this;
            }
        }

        public class RetrieveRequest : EntityRequest<RetrieveRequest>
        {
            public RetrieveRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RetrieveRequest Id(string id)
            {
                MParams.Add("id", id);
                return this;
            }
        }

        public class DeleteRequest : EntityRequest<DeleteRequest>
        {
            public DeleteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public DeleteRequest Id(string id)
            {
                MParams.Add("id", id);
                return this;
            }
        }

        public class UsageListRequest : ListRequestBase<UsageListRequest>
        {
            public UsageListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<UsageListRequest> Id()
            {
                return new("id", this);
            }

            public StringFilter<UsageListRequest> SubscriptionId()
            {
                return new("subscription_id", this);
            }

            public TimestampFilter<UsageListRequest> UsageDate()
            {
                return new("usage_date", this);
            }

            public StringFilter<UsageListRequest> ItemPriceId()
            {
                return new("item_price_id", this);
            }

            public StringFilter<UsageListRequest> InvoiceId()
            {
                return new StringFilter<UsageListRequest>("invoice_id", this).SupportsPresenceOperator(true);
            }

            public EnumFilter<SourceEnum, UsageListRequest> Source()
            {
                return new("source", this);
            }
        }

        public class PdfRequest : EntityRequest<PdfRequest>
        {
            public PdfRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public PdfRequest DispositionType(DispositionTypeEnum dispositionType)
            {
                MParams.AddOpt("disposition_type", dispositionType);
                return this;
            }

            public PdfRequest InvoiceId(string invoiceId)
            {
                MParams.Add("invoice[id]", invoiceId);
                return this;
            }
        }

        #endregion


        #region Subclasses

        #endregion
    }
}