using System;

namespace Strawpoll.Core.Exceptions
{
    public class FormNotFoundException : Exception
    {
        public FormNotFoundException(string message) : base(message)
        {
        }
    }
}
