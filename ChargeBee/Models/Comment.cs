using System;
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
    public class Comment : Resource
    {
        public enum TypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "user")] User,
            [EnumMember(Value = "system")] System
        }

        public Comment()
        {
        }

        public Comment(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Comment(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Comment(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("comments");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("comments", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static CommentListRequest List()
        {
            var url = ApiUtil.BuildUrl("comments");
            return new CommentListRequest(url);
        }

        public static EntityRequest<Type> Delete(string id)
        {
            var url = ApiUtil.BuildUrl("comments", CheckNull(id), "delete");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public EntityTypeEnum EntityType => GetEnum<EntityTypeEnum>("entity_type");

        public string AddedBy => GetValue<string>("added_by", false);

        public string Notes => GetValue<string>("notes");

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public TypeEnum CommentType => GetEnum<TypeEnum>("type");

        public string EntityId => GetValue<string>("entity_id");

        #endregion

        #region Requests

        public class CreateRequest : EntityRequest<CreateRequest>
        {
            public CreateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateRequest EntityType(EntityTypeEnum entityType)
            {
                MParams.Add("entity_type", entityType);
                return this;
            }

            public CreateRequest EntityId(string entityId)
            {
                MParams.Add("entity_id", entityId);
                return this;
            }

            public CreateRequest Notes(string notes)
            {
                MParams.Add("notes", notes);
                return this;
            }

            public CreateRequest AddedBy(string addedBy)
            {
                MParams.AddOpt("added_by", addedBy);
                return this;
            }
        }

        public class CommentListRequest : ListRequestBase<CommentListRequest>
        {
            public CommentListRequest(string url)
                : base(url)
            {
            }

            public CommentListRequest EntityType(EntityTypeEnum entityType)
            {
                MParams.AddOpt("entity_type", entityType);
                return this;
            }

            public CommentListRequest EntityId(string entityId)
            {
                MParams.AddOpt("entity_id", entityId);
                return this;
            }

            public TimestampFilter<CommentListRequest> CreatedAt()
            {
                return new("created_at", this);
            }

            public CommentListRequest SortByCreatedAt(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "created_at");
                return this;
            }
        }

        #endregion

        #region Subclasses

        #endregion
    }
}