using System;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;

namespace Strawpoll.Core.Connection
{
    public interface IParse
    {
        Task<T> ParseAsync<T>(IRequest request, Func<IHtmlDocument, string, T> func, Action<IResponse> handleResponse, CancellationToken cancellationToken);
    }
}
