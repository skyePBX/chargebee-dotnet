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
    public class SiteMigrationDetail : Resource
    {
        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "moved_in")] MovedIn,
            [EnumMember(Value = "moved_out")] MovedOut,
            [EnumMember(Value = "moving_out")] MovingOut
        }

        public SiteMigrationDetail()
        {
        }

        public SiteMigrationDetail(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public SiteMigrationDetail(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public SiteMigrationDetail(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static SiteMigrationDetailListRequest List()
        {
            var url = ApiUtil.BuildUrl("site_migration_details");
            return new SiteMigrationDetailListRequest(url);
        }

        #endregion

        #region Requests

        public class SiteMigrationDetailListRequest : ListRequestBase<SiteMigrationDetailListRequest>
        {
            public SiteMigrationDetailListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<SiteMigrationDetailListRequest> EntityIdAtOtherSite()
            {
                return new("entity_id_at_other_site", this);
            }

            public StringFilter<SiteMigrationDetailListRequest> OtherSiteName()
            {
                return new("other_site_name", this);
            }

            public StringFilter<SiteMigrationDetailListRequest> EntityId()
            {
                return new("entity_id", this);
            }

            public EnumFilter<EntityTypeEnum, SiteMigrationDetailListRequest> EntityType()
            {
                return new("entity_type", this);
            }

            public EnumFilter<StatusEnum, SiteMigrationDetailListRequest> Status()
            {
                return new("status", this);
            }
        }

        #endregion

        #region Properties

        public string EntityId => GetValue<string>("entity_id");

        public string OtherSiteName => GetValue<string>("other_site_name");

        public string EntityIdAtOtherSite => GetValue<string>("entity_id_at_other_site");

        public DateTime MigratedAt => (DateTime) GetDateTime("migrated_at");

        public EntityTypeEnum EntityType => GetEnum<EntityTypeEnum>("entity_type");

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        #endregion

        #region Subclasses

        #endregion
    }
}