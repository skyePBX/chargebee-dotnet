using Newtonsoft.Json.Linq;

namespace ChargeBee.Internal
{
    public class JsonSupport
    {
        protected JToken MJobj;

        internal JToken JObj
        {
            get => MJobj;
            set => MJobj = value;
        }
    }
}