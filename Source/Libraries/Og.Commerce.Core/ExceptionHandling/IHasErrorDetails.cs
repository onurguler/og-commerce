namespace Og.Commerce.Core.ExceptionHandling;

public interface IHasErrorDetails
{
    string? Details { get; }
}
