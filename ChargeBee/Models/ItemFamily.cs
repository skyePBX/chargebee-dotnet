using System;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Api;
using ChargeBee.Filters;
using ChargeBee.Internal;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class ItemFamily : Resource
    {
        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "active")] Active,
            [EnumMember(Value = "deleted")] Deleted
        }

        public ItemFamily()
        {
        }

        public ItemFamily(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public ItemFamily(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public ItemFamily(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("item_families");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("item_families", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static ItemFamilyListRequest List()
        {
            var url = ApiUtil.BuildUrl("item_families");
            return new ItemFamilyListRequest(url);
        }

        public static UpdateRequest Update(string id)
        {
            var url = ApiUtil.BuildUrl("item_families", CheckNull(id));
            return new UpdateRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Delete(string id)
        {
            var url = ApiUtil.BuildUrl("item_families", CheckNull(id), "delete");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string Name => GetValue<string>("name");

        public string Description => GetValue<string>("description", false);

        public StatusEnum? Status => GetEnum<StatusEnum>("status", false);

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

            public CreateRequest Description(string description)
            {
                MParams.AddOpt("description", description);
                return this;
            }
        }

        public class ItemFamilyListRequest : ListRequestBase<ItemFamilyListRequest>
        {
            public ItemFamilyListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<ItemFamilyListRequest> Id()
            {
                return new StringFilter<ItemFamilyListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<ItemFamilyListRequest> Name()
            {
                return new("name", this);
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

            public UpdateRequest Description(string description)
            {
                MParams.AddOpt("description", description);
                return this;
            }
        }

        #endregion

        #region Subclasses

        #endregion
    }
}