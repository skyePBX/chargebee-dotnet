using System.Collections.Generic;
using System.Net;
using ChargeBee.Internal;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Api
{
    public class ListResult
    {
        public ListResult(HttpStatusCode statusCode, string json)
        {
            List = new List<Entry>();

            var jobj = JObject.Parse(json);

            foreach (var item in jobj["list"].Children()) List.Add(new Entry(item));

            var t = jobj["next_offset"];
            if (t != null) NextOffset = t.ToString();

            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }

        public List<Entry> List { get; }

        public string NextOffset { get; }

        public class Entry : ResultBase
        {
            public Entry()
            {
            }

            internal Entry(string json)
            {
                if (!string.IsNullOrEmpty(json))
                    MJobj = JToken.Parse(json);
            }

            internal Entry(JToken jobj)
            {
                MJobj = jobj;
            }
        }
    }
}