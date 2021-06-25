using System.Collections.Generic;
using System.IO;
using ChargeBee.Internal;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class Hierarchy : Resource
    {
        public Hierarchy()
        {
        }

        public Hierarchy(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Hierarchy(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Hierarchy(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        #endregion

        #region Properties

        public string CustomerId => GetValue<string>("customer_id");

        public string ParentId => GetValue<string>("parent_id", false);

        public string PaymentOwnerId => GetValue<string>("payment_owner_id");

        public string InvoiceOwnerId => GetValue<string>("invoice_owner_id");

        public List<string> ChildrenIds => GetList<string>("children_ids");

        #endregion


        #region Subclasses

        #endregion
    }
}