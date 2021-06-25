using System;
using System.Collections.Generic;
using ChargeBee.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Internal
{
    public class ResultBase : JsonSupport
    {
        public ResultBase()
        {
        }

        internal ResultBase(string json)
        {
            if (!string.IsNullOrEmpty(json))
                try
                {
                    MJobj = JToken.Parse(json);
                }
                catch (JsonException e)
                {
                    throw new Exception("Not in JSON format. Probably not a ChargeBee response. \n " + json, e);
                }
        }

        internal ResultBase(JToken jobj)
        {
            MJobj = jobj;
        }

        public Subscription Subscription => GetResource<Subscription>("subscription");

        public ContractTerm ContractTerm => GetResource<ContractTerm>("contract_term");

        public AdvanceInvoiceSchedule AdvanceInvoiceSchedule =>
            GetResource<AdvanceInvoiceSchedule>("advance_invoice_schedule");

        public Customer Customer => GetResource<Customer>("customer");

        public Hierarchy Hierarchy => GetResource<Hierarchy>("hierarchy");

        public Contact Contact => GetResource<Contact>("contact");

        public Token Token => GetResource<Token>("token");

        public PaymentSource PaymentSource => GetResource<PaymentSource>("payment_source");

        public ThirdPartyPaymentMethod ThirdPartyPaymentMethod =>
            GetResource<ThirdPartyPaymentMethod>("third_party_payment_method");

        public VirtualBankAccount VirtualBankAccount => GetResource<VirtualBankAccount>("virtual_bank_account");

        public Card Card => GetResource<Card>("card");

        public PromotionalCredit PromotionalCredit => GetResource<PromotionalCredit>("promotional_credit");

        public Invoice Invoice => GetResource<Invoice>("invoice");

        public CreditNote CreditNote => GetResource<CreditNote>("credit_note");

        public UnbilledCharge UnbilledCharge => GetResource<UnbilledCharge>("unbilled_charge");

        public Order Order => GetResource<Order>("order");

        public Gift Gift => GetResource<Gift>("gift");

        public Transaction Transaction => GetResource<Transaction>("transaction");

        public HostedPage HostedPage => GetResource<HostedPage>("hosted_page");

        public Estimate Estimate => GetResource<Estimate>("estimate");

        public Quote Quote => GetResource<Quote>("quote");

        public QuotedSubscription QuotedSubscription => GetResource<QuotedSubscription>("quoted_subscription");

        public QuoteLineGroup QuoteLineGroup => GetResource<QuoteLineGroup>("quote_line_group");

        public Plan Plan => GetResource<Plan>("plan");

        public Addon Addon => GetResource<Addon>("addon");

        public Coupon Coupon => GetResource<Coupon>("coupon");

        public CouponSet CouponSet => GetResource<CouponSet>("coupon_set");

        public CouponCode CouponCode => GetResource<CouponCode>("coupon_code");

        public Address Address => GetResource<Address>("address");

        public Usage Usage => GetResource<Usage>("usage");

        public Event Event => GetResource<Event>("event");

        public Comment Comment => GetResource<Comment>("comment");

        public Download Download => GetResource<Download>("download");

        public PortalSession PortalSession => GetResource<PortalSession>("portal_session");

        public SiteMigrationDetail SiteMigrationDetail => GetResource<SiteMigrationDetail>("site_migration_detail");

        public ResourceMigration ResourceMigration => GetResource<ResourceMigration>("resource_migration");

        public TimeMachine TimeMachine => GetResource<TimeMachine>("time_machine");

        public Export Export => GetResource<Export>("export");

        public PaymentIntent PaymentIntent => GetResource<PaymentIntent>("payment_intent");

        public ItemFamily ItemFamily => GetResource<ItemFamily>("item_family");

        public Item Item => GetResource<Item>("item");

        public ItemPrice ItemPrice => GetResource<ItemPrice>("item_price");

        public AttachedItem AttachedItem => GetResource<AttachedItem>("attached_item");

        public DifferentialPrice DifferentialPrice => GetResource<DifferentialPrice>("differential_price");

        public List<UnbilledCharge> UnbilledCharges =>
            GetResourceList<UnbilledCharge>("unbilled_charges");

        public List<CreditNote> CreditNotes => GetResourceList<CreditNote>("credit_notes");

        public List<AdvanceInvoiceSchedule> AdvanceInvoiceSchedules =>
            GetResourceList<AdvanceInvoiceSchedule>("advance_invoice_schedules");

        public List<Hierarchy> Hierarchies => GetResourceList<Hierarchy>("hierarchies");

        public List<Invoice> Invoices => GetResourceList<Invoice>("invoices");

        public List<DifferentialPrice> DifferentialPrices =>
            GetResourceList<DifferentialPrice>("differential_prices");


        private List<T> GetResourceList<T>(string property) where T : Resource, new()
        {
            var list = new List<T>();
            var jArr = (JArray) MJobj.SelectToken(property);
            if (jArr != null)
                foreach (var jObj in jArr.Children())
                {
                    var t = new T();
                    t.JObj = jObj;
                    list.Add(t);
                }

            return list;
        }

        private T GetResource<T>(string property) where T : Resource, new()
        {
            if (MJobj == null)
                return default;

            var jobj = MJobj[property];
            if (jobj != null)
            {
                var t = new T();
                t.JObj = jobj;
                return t;
            }

            return default;
        }
    }
}