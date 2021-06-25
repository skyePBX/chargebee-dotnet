using System;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class SubscriptionEstimate : Resource
    {
        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "future")] Future,
            [EnumMember(Value = "in_trial")] InTrial,
            [EnumMember(Value = "active")] Active,
            [EnumMember(Value = "non_renewing")] NonRenewing,
            [EnumMember(Value = "paused")] Paused,
            [EnumMember(Value = "cancelled")] Cancelled
        }

        public SubscriptionEstimate()
        {
        }

        public SubscriptionEstimate(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public SubscriptionEstimate(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public SubscriptionEstimate(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        #endregion

        #region Properties

        public string Id => GetValue<string>("id", false);

        public string CurrencyCode => GetValue<string>("currency_code");

        public StatusEnum? Status => GetEnum<StatusEnum>("status", false);

        public DateTime? NextBillingAt => GetDateTime("next_billing_at", false);

        public DateTime? PauseDate => GetDateTime("pause_date", false);

        public DateTime? ResumeDate => GetDateTime("resume_date", false);

        public SubscriptionEstimateShippingAddress ShippingAddress =>
            GetSubResource<SubscriptionEstimateShippingAddress>("shipping_address");

        public SubscriptionEstimateContractTerm ContractTerm =>
            GetSubResource<SubscriptionEstimateContractTerm>("contract_term");

        #endregion

        #region Subclasses

        public class SubscriptionEstimateShippingAddress : Resource
        {
            public string FirstName()
            {
                return GetValue<string>("first_name", false);
            }

            public string LastName()
            {
                return GetValue<string>("last_name", false);
            }

            public string Email()
            {
                return GetValue<string>("email", false);
            }

            public string Company()
            {
                return GetValue<string>("company", false);
            }

            public string Phone()
            {
                return GetValue<string>("phone", false);
            }

            public string Line1()
            {
                return GetValue<string>("line1", false);
            }

            public string Line2()
            {
                return GetValue<string>("line2", false);
            }

            public string Line3()
            {
                return GetValue<string>("line3", false);
            }

            public string City()
            {
                return GetValue<string>("city", false);
            }

            public string StateCode()
            {
                return GetValue<string>("state_code", false);
            }

            public string State()
            {
                return GetValue<string>("state", false);
            }

            public string Country()
            {
                return GetValue<string>("country", false);
            }

            public string Zip()
            {
                return GetValue<string>("zip", false);
            }

            public ValidationStatusEnum? ValidationStatus()
            {
                return GetEnum<ValidationStatusEnum>("validation_status", false);
            }
        }

        public class SubscriptionEstimateContractTerm : Resource
        {
            public enum ActionAtTermEndEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "renew")] Renew,
                [EnumMember(Value = "evergreen")] Evergreen,
                [EnumMember(Value = "cancel")] Cancel,
                [EnumMember(Value = "renew_once")] RenewOnce
            }

            public enum StatusEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "active")] Active,
                [EnumMember(Value = "completed")] Completed,
                [EnumMember(Value = "cancelled")] Cancelled,
                [EnumMember(Value = "terminated")] Terminated
            }

            public string Id()
            {
                return GetValue<string>("id");
            }

            public StatusEnum Status()
            {
                return GetEnum<StatusEnum>("status");
            }

            public DateTime ContractStart()
            {
                return (DateTime) GetDateTime("contract_start");
            }

            public DateTime ContractEnd()
            {
                return (DateTime) GetDateTime("contract_end");
            }

            public int BillingCycle()
            {
                return GetValue<int>("billing_cycle");
            }

            public ActionAtTermEndEnum ActionAtTermEnd()
            {
                return GetEnum<ActionAtTermEndEnum>("action_at_term_end");
            }

            public long TotalContractValue()
            {
                return GetValue<long>("total_contract_value");
            }

            public int? CancellationCutoffPeriod()
            {
                return GetValue<int?>("cancellation_cutoff_period", false);
            }

            public DateTime CreatedAt()
            {
                return (DateTime) GetDateTime("created_at");
            }

            public string SubscriptionId()
            {
                return GetValue<string>("subscription_id");
            }

            public int? RemainingBillingCycles()
            {
                return GetValue<int?>("remaining_billing_cycles", false);
            }
        }

        #endregion
    }
}