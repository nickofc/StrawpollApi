using System.Net;

namespace Strawpoll.Core.Connection
{
    public class Response : IResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
    }
}
