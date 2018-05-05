using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Strawpoll.Core.Connection
{
    public class StrawpollClient : IConnection
    {
        private readonly HttpClient _httpClient;

        public StrawpollClient()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            }, true)
            {
                DefaultRequestHeaders =
                {
                    {"accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8"},
                    {"accept-encoding", " gzip, deflate"},
                    {"accept-language", "pl-PL,pl;q=0.9,en-US;q=0.8,en;q=0.7"},
                    {
                        "user-agent",
                        "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36"
                    },
                },
            };
        }

        public StrawpollClient(IWebProxy proxy)
        {
            if (proxy == null)
                throw new ArgumentNullException(nameof(proxy));

            _httpClient = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseProxy = true,
                Proxy = proxy,
            }, true)
            {
                DefaultRequestHeaders =
                {
                    {"accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8"},
                    {"accept-encoding", " gzip, deflate"},
                    {"accept-language", "pl-PL,pl;q=0.9,en-US;q=0.8,en;q=0.7"},
                    {
                        "user-agent",
                        "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36"
                    },
                }
            };
        }

        protected virtual HttpRequestMessage BuildHttpRequestMessage(IRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var requestMessage = new HttpRequestMessage();
            try
            {
                requestMessage.Method = request.Method;
                requestMessage.Content = request.Content;
                requestMessage.RequestUri = new Uri(request.RequestUri);

                if (request.Headers != null)
                {
                    if (request.Headers.Any())
                    {
                        foreach (var header in request.Headers)
                        {
                            requestMessage.Headers.Add(header.Key, header.Value);
                        }
                    }
                }
            }
            catch (Exception)
            {
                requestMessage.Dispose();
                throw;
            }

            return requestMessage;
        }

        public async Task<IResponse> SendAsync(IRequest request, CancellationToken cancellationToken)
        {
            using (var httpRequestMessage = BuildHttpRequestMessage(request))
            {
                using (var responseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken)
                                                              .ConfigureAwait(false))
                {
                    return new Response
                    {
                        StatusCode = responseMessage.StatusCode,
                        Content = await responseMessage.Content.ReadAsStringAsync()
                                                               .ConfigureAwait(false)
                    };
                }
            }
        }
    }
}
