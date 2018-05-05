using System;

namespace Strawpoll.Core.Exceptions
{
    public class VoteNotFoundException : Exception
    {
        public VoteNotFoundException(string message) : base(message)
        {
        }
    }
}
