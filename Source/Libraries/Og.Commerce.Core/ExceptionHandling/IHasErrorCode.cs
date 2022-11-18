namespace Og.Commerce.Core.ExceptionHandling;

public interface IHasErrorCode
{
    string? Code { get; }
}
