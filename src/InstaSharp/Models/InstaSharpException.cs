using System;

namespace InstaSharp.Models
{
    public class InstaSharpException : Exception
    {
        public InstaSharpException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
