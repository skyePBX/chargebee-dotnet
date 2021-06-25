using System;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Internal;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class ContractTerm : Resource
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

        public ContractTerm()
        {
        }

        public ContractTerm(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public ContractTerm(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public ContractTerm(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public DateTime ContractStart => (DateTime) GetDateTime("contract_start");

        public DateTime ContractEnd => (DateTime) GetDateTime("contract_end");

        public int BillingCycle => GetValue<int>("billing_cycle");

        public ActionAtTermEndEnum ActionAtTermEnd => GetEnum<ActionAtTermEndEnum>("action_at_term_end");

        public long TotalContractValue => GetValue<long>("total_contract_value");

        public int? CancellationCutoffPeriod => GetValue<int?>("cancellation_cutoff_period", false);

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public string SubscriptionId => GetValue<string>("subscription_id");

        public int? RemainingBillingCycles => GetValue<int?>("remaining_billing_cycles", false);

        #endregion

        #region Subclasses

        #endregion
    }
}