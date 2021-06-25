using System.Runtime.Serialization;

namespace ChargeBee.Models.Enums
{
    public enum PriceTypeEnum
    {
        [EnumMember(Value = "Unknown Enum")]
        UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */

        [EnumMember(Value = "tax_exclusive")] TaxExclusive,

        [EnumMember(Value = "tax_inclusive")] TaxInclusive
    }
}