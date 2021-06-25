﻿using System.Runtime.Serialization;

namespace ChargeBee.Filters.enums
{
    public enum SortOrderEnum
    {
        [EnumMember(Value = "Unknown Enum")]
        UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */

        [EnumMember(Value = "asc")] Asc,

        [EnumMember(Value = "desc")] Desc
    }
}