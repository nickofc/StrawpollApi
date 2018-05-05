using System;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;

namespace Strawpoll.Core.Connection
{
    public class Parse : IParse
    {
        private readonly IConnection _connection;

        public Parse(IConnection connection)
        {
            _connection = connection;
        }

        public async Task<T> ParseAsync<T>(IRequest request, Func<IHtmlDocument, string, T> func,
            CancellationToken cancellationToken)
        {
            return await ParseAsync(request, func, null, cancellationToken);
        }

        public async Task<T> ParseAsync<T>(IRequest request, Func<IHtmlDocument, string, T> func, Action<IResponse> handleResponse, CancellationToken cancellationToken)
        {
            var response = await _connection.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (handleResponse != null)
            {
                handleResponse(response);
            }
            
            var html = new HtmlParser();
            using (var document = await html.ParseAsync(response.Content, cancellationToken).ConfigureAwait(false))
            {
                return func(document, response.Content);
            }
        }
    }
}