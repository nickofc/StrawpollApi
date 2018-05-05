using System;
using System.Collections.Generic;
using System.Text;

namespace Strawpoll.Core.Exceptions
{
    public class StrawpollException : Exception
    {
        public StrawpollException(string message) : base(message)
        {
        }

        public StrawpollException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
