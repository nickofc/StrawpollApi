using System.Threading;
using System.Threading.Tasks;
using Strawpoll.Core.Models;

namespace Strawpoll.Core.Repositories
{
    public interface IFormRepository
    {
        Task<IForm> GetFormAsync(int formId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> VoteAsync(IForm form, IVote vote, CancellationToken cancellationToken = default(CancellationToken));
        Task<IToken> GetTokenAsync(IForm form, CancellationToken cancellationToken = default(CancellationToken));
    }
}