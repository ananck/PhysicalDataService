using System;
using System.Runtime.Serialization;

namespace Core.Service.SpiderServers
{
    [Serializable]
    internal class AccountClosureException : Exception
    {
        public AccountClosureException()
        {
        }

        public AccountClosureException(string message) : base(message)
        {
        }

        public AccountClosureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AccountClosureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}