using System.Runtime.Serialization;

namespace Krosoft.Extensions.Core.Models.Exceptions;

public class KrosoftException : Exception
{
    public KrosoftException()
    {
    }

    public KrosoftException(string message)
        : base(message)
    {
    }

    public KrosoftException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public KrosoftException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}