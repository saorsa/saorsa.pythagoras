using System.Runtime.Serialization;

namespace Saorsa.Pythagoras;

public class PythagorasException : Exception
{
    public int ErrorCode { get; }

    public PythagorasException(int code, string message)
        : base(message)
    {
        ErrorCode = code;
    }

    public PythagorasException(int code, string message, Exception inner)
        : base(message, inner)
    {
        ErrorCode = code;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ErrorCode), ErrorCode, typeof(int));
    }
}
