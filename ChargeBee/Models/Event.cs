using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Api;
using ChargeBee.Filters;
using ChargeBee.Filters.enums;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class Event : Resource
    {
        [Obsolete]
        public enum WebhookStatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "not_configured")] NotConfigured,
            [EnumMember(Value = "scheduled")] Scheduled,
            [EnumMember(Value = "succeeded")] Succeeded,
            [EnumMember(Value = "re_scheduled")] ReScheduled,
            [EnumMember(Value = "failed")] Failed,
            [EnumMember(Value = "skipped")] Skipped,
            [EnumMember(Value = "not_applicable")] NotApplicable
        }

        public Event()
        {
        }

        public Event(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Event(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Event(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Requests

        public class EventListRequest : ListRequestBase<EventListRequest>
        {
            public EventListRequest(string url)
                : base(url)
            {
            }

            [Obsolete]
            public EventListRequest StartTime(long startTime)
            {
                MParams.AddOpt("start_time", startTime);
                return this;
            }

            [Obsolete]
            public EventListRequest EndTime(long endTime)
            {
                MParams.AddOpt("end_time", endTime);
                return this;
            }

            public StringFilter<EventListRequest> Id()
            {
                return new StringFilter<EventListRequest>("id", this).SupportsMultiOperators(true);
            }

            public EnumFilter<WebhookStatusEnum, EventListRequest> WebhookStatus()
            {
                return new("webhook_status", this);
            }

            [Obsolete]
            public EventListRequest WebhookStatus(WebhookStatusEnum webhookStatus)
            {
                MParams.AddOpt("webhook_status", webhookStatus);
                return this;
            }

            public EnumFilter<EventTypeEnum, EventListRequest> EventType()
            {
                return new("event_type", this);
            }

            [Obsolete]
            public EventListRequest EventType(EventTypeEnum eventType)
            {
                MParams.AddOpt("event_type", eventType);
                return this;
            }

            public EnumFilter<SourceEnum, EventListRequest> Source()
            {
                return new("source", this);
            }

            public TimestampFilter<EventListRequest> OccurredAt()
            {
                return new("occurred_at", this);
            }

            public EventListRequest SortByOccurredAt(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "occurred_at");
                return this;
            }
        }

        #endregion

        #region Methods

        public static EventListRequest List()
        {
            var url = ApiUtil.BuildUrl("events");
            return new EventListRequest(url);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("events", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public DateTime OccurredAt => (DateTime) GetDateTime("occurred_at");

        public SourceEnum Source => GetEnum<SourceEnum>("source");

        public string User => GetValue<string>("user", false);

        [Obsolete] public WebhookStatusEnum WebhookStatus => GetEnum<WebhookStatusEnum>("webhook_status");

        [Obsolete] public string WebhookFailureReason => GetValue<string>("webhook_failure_reason", false);

        public List<EventWebhook> Webhooks => GetResourceList<EventWebhook>("webhooks");

        public EventTypeEnum? EventType => GetEnum<EventTypeEnum>("event_type", false);

        public ApiVersionEnum? ApiVersion => GetEnum<ApiVersionEnum>("api_version", false);

        public EventContent Content => new(GetValue<JToken>("content"));

        #endregion

        #region Subclasses

        public class EventWebhook : Resource
        {
            public enum WebhookStatusEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "not_configured")] NotConfigured,
                [EnumMember(Value = "scheduled")] Scheduled,
                [EnumMember(Value = "succeeded")] Succeeded,
                [EnumMember(Value = "re_scheduled")] ReScheduled,
                [EnumMember(Value = "failed")] Failed,
                [EnumMember(Value = "skipped")] Skipped,
                [EnumMember(Value = "not_applicable")] NotApplicable
            }

            public string Id()
            {
                return GetValue<string>("id");
            }

            public WebhookStatusEnum WebhookStatus()
            {
                return GetEnum<WebhookStatusEnum>("webhook_status");
            }
        }

        public class EventContent : ResultBase
        {
            public EventContent()
            {
            }

            internal EventContent(JToken jobj)
            {
                MJobj = jobj;
            }
        }

        #endregion
    }
}