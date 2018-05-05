using System.Collections.Generic;
using System.Net.Http;

namespace Strawpoll.Core.Connection
{
    public interface IRequest
    {
        HttpContent Content { get; }
        HttpMethod Method { get; }
        string RequestUri { get; }
        IReadOnlyDictionary<string, string> Headers { get; }
    }
}
