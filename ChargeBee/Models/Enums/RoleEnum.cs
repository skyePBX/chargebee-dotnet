using System.Runtime.Serialization;

namespace ChargeBee.Models.Enums
{
    public enum RoleEnum
    {
        [EnumMember(Value = "Unknown Enum")]
        UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */

        [EnumMember(Value = "primary")] Primary,

        [EnumMember(Value = "backup")] Backup,

        [EnumMember(Value = "none")] None
    }
}