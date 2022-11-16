using System.Runtime.Serialization;

namespace Og.Commerce.Core.Result;

public enum ResultStatus
{
    [EnumMember(Value = "Ok")]
    Ok,
    [EnumMember(Value = "Error")]
    Error,
    [EnumMember(Value = "Forbidden")]
    Forbidden,
    [EnumMember(Value = "Unauthorized")]
    Unauthorized,
    [EnumMember(Value = "Invalid")]
    Invalid,
    [EnumMember(Value = "NotFound")]
    NotFound
}
