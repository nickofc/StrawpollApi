using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Strawpoll.Core.Connection
{
    public class Request : IRequest
    {
        public HttpMethod Method { get; set; }
        public string RequestUri { get; set; }
        public HttpContent Content { get; set; }
        public IReadOnlyDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public Request(HttpMethod method, string requestUri)
        {
            if (string.IsNullOrWhiteSpace(requestUri))     
                throw new ArgumentException("Cannot be empty.", nameof(requestUri));

            Method = method ?? throw new ArgumentNullException(nameof(method));
            RequestUri = requestUri;
        }

        public Request(HttpMethod method, string requestUri, HttpContent content)
        {
            if (string.IsNullOrWhiteSpace(requestUri))     
                throw new ArgumentException("Cannot be empty.", nameof(requestUri));

            Method = method ?? throw new ArgumentNullException(nameof(method));
            Content = content ?? throw new ArgumentNullException(nameof(content));
            RequestUri = requestUri;
        }
    }
}
