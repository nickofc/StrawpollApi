using System.Net;

namespace Strawpoll.Core.Connection
{
    public interface IResponse
    {
        HttpStatusCode StatusCode { get; }
        string Content { get; }
    }
}
