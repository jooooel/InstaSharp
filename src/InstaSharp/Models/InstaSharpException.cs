using System;

namespace InstaSharp.Models
{
    public class InstaSharpException : Exception
    {
        public InstaSharpExceptionType InstaSharpExceptionType { get; set; }

        public InstaSharpException(string message, InstaSharpExceptionType instaSharpExceptionType = InstaSharpExceptionType.Unknown) : base(message)
        {
            InstaSharpExceptionType = instaSharpExceptionType;
        }

        public InstaSharpException(string message, Exception innerException, InstaSharpExceptionType instaSharpExceptionType = InstaSharpExceptionType.Unknown) : base(message, innerException)
        {
            InstaSharpExceptionType = instaSharpExceptionType;
        }
    }

    public enum InstaSharpExceptionType
    {
        Unknown,
        InstagramApiUnavailable
    }
}
