using System.Runtime.Serialization;

namespace ChargeBee.Models.Enums
{
    public enum ChargeOnEnum
    {
        [EnumMember(Value = "Unknown Enum")]
        UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */

        [EnumMember(Value = "immediately")] Immediately,

        [EnumMember(Value = "on_event")] OnEvent
    }
}