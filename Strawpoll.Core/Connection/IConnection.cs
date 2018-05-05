using System.Threading;
using System.Threading.Tasks;

namespace Strawpoll.Core.Connection
{
    public interface IConnection
    {
        Task<IResponse> SendAsync(IRequest request, CancellationToken cancellationToken = default (CancellationToken));
    }
}