using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ChargeBee.Api;
using ChargeBee.Exceptions;
using ChargeBee.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class TimeMachine : Resource
    {
        public enum TimeTravelStatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "not_enabled")] NotEnabled,
            [EnumMember(Value = "in_progress")] InProgress,
            [EnumMember(Value = "succeeded")] Succeeded,
            [EnumMember(Value = "failed")] Failed
        }

        public TimeMachine()
        {
        }

        public TimeMachine(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public TimeMachine(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public TimeMachine(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("time_machines", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static StartAfreshRequest StartAfresh(string id)
        {
            var url = ApiUtil.BuildUrl("time_machines", CheckNull(id), "start_afresh");
            return new StartAfreshRequest(url, HttpMethod.Post);
        }

        public static TravelForwardRequest TravelForward(string id)
        {
            var url = ApiUtil.BuildUrl("time_machines", CheckNull(id), "travel_forward");
            return new TravelForwardRequest(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Name => GetValue<string>("name");

        public TimeTravelStatusEnum TimeTravelStatus => GetEnum<TimeTravelStatusEnum>("time_travel_status");

        public DateTime GenesisTime => (DateTime) GetDateTime("genesis_time");

        public DateTime DestinationTime => (DateTime) GetDateTime("destination_time");

        public string FailureCode => GetValue<string>("failure_code", false);

        public string FailureReason => GetValue<string>("failure_reason", false);

        public string ErrorJson => GetValue<string>("error_json", false);

        public TimeMachine WaitForTimeTravelCompletion()
        {
            return WaitForTimeTravelCompletion(null);
        }

        public TimeMachine WaitForTimeTravelCompletion(ApiConfig config)
        {
            var count = 0;
            while (TimeTravelStatus == TimeTravelStatusEnum.InProgress)
            {
                if (count++ > 30) throw new Exception("Time travel is taking too much time");
                var t = Task.Factory.StartNew(() => { Task.Delay(ApiConfig.TimeTravelMillis).Wait(); });
                t.Wait();
                var req = Retrieve(Name);
                JObj = (config == null ? req.Request() : req.Request(config)).TimeMachine.JObj;
            }

            if (TimeTravelStatus == TimeTravelStatusEnum.Failed)
            {
                var errorJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(ErrorJson
                );
                var httpStatusCode = (HttpStatusCode) Convert.ToInt32(errorJson["http_code"]);
                throw new OperationFailedException(httpStatusCode, errorJson);
            }

            if (TimeTravelStatus == TimeTravelStatusEnum.NotEnabled || TimeTravelStatus == TimeTravelStatusEnum.UnKnown)
                throw new Exception("Time travel is in wrong state : " + TimeTravelStatus);
            return this;
        }

        #endregion

        #region Requests

        public class StartAfreshRequest : EntityRequest<StartAfreshRequest>
        {
            public StartAfreshRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public StartAfreshRequest GenesisTime(long genesisTime)
            {
                MParams.AddOpt("genesis_time", genesisTime);
                return this;
            }
        }

        public class TravelForwardRequest : EntityRequest<TravelForwardRequest>
        {
            public TravelForwardRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public TravelForwardRequest DestinationTime(long destinationTime)
            {
                MParams.AddOpt("destination_time", destinationTime);
                return this;
            }
        }

        #endregion

        #region Subclasses

        #endregion
    }
}