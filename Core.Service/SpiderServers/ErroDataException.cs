using System;

namespace Core.Service.SpiderServers
{
    internal class ErroDataException : Exception
    {
        public ErroDataException(string message) : base(message)
        {
        }
    }
}