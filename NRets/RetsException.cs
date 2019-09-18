using System;
using System.Collections.Generic;
using System.Text;

namespace NRets
{
    public class RetsException : Exception
    {
        public string ExtendedMessage { get; set; }

        public RetsException(string message)
            : base(message)
        {
        }

        public RetsException(string message, string extendedMessage)
            : base(message)
        {
            ExtendedMessage = extendedMessage;
        }

        public RetsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
