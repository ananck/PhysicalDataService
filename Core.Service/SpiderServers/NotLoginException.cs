using System;

namespace Core.Service.SpiderServers
{
    internal class NotLoginException : Exception
    {
        public NotLoginException(string message) : base(message)
        {
        }
    }
}