using System;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Api;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class ResourceMigration : Resource
    {
        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "scheduled")] Scheduled,
            [EnumMember(Value = "failed")] Failed,
            [EnumMember(Value = "succeeded")] Succeeded
        }

        public ResourceMigration()
        {
        }

        public ResourceMigration(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public ResourceMigration(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public ResourceMigration(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static RetrieveLatestRequest RetrieveLatest()
        {
            var url = ApiUtil.BuildUrl("resource_migrations", "retrieve_latest");
            return new RetrieveLatestRequest(url, HttpMethod.Get);
        }

        #endregion

        #region Requests

        public class RetrieveLatestRequest : EntityRequest<RetrieveLatestRequest>
        {
            public RetrieveLatestRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RetrieveLatestRequest FromSite(string fromSite)
            {
                MParams.Add("from_site", fromSite);
                return this;
            }

            public RetrieveLatestRequest EntityType(EntityTypeEnum entityType)
            {
                MParams.Add("entity_type", entityType);
                return this;
            }

            public RetrieveLatestRequest EntityId(string entityId)
            {
                MParams.Add("entity_id", entityId);
                return this;
            }
        }

        #endregion

        #region Properties

        public string FromSite => GetValue<string>("from_site");

        public EntityTypeEnum EntityType => GetEnum<EntityTypeEnum>("entity_type");

        public string EntityId => GetValue<string>("entity_id");

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public string Errors => GetValue<string>("errors", false);

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public DateTime UpdatedAt => (DateTime) GetDateTime("updated_at");

        #endregion

        #region Subclasses

        #endregion
    }
}