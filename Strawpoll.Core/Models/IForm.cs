using System.Collections.Generic;

namespace Strawpoll.Core.Models
{
    public interface IForm
    {
        int Id { get; }
        string Title { get; }
        IEnumerable<IVote> Votes { get; }      
    }
}
