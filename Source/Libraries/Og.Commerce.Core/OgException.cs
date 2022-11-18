using System.Runtime.Serialization;

namespace Og.Commerce.Core;

/// <summary>
/// Base exception type for those are thrown by system for og specific exceptions.
/// </summary>
public class OgException : Exception
{
    public OgException()
    {

    }

    public OgException(string message)
        : base(message)
    {

    }

    public OgException(string message, Exception innerException)
        : base(message, innerException)
    {

    }

    public OgException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }
}
