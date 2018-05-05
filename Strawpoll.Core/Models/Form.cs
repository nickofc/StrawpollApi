using System.Collections.Generic;

namespace Strawpoll.Core.Models
{
    public class Form : IForm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<IVote> Votes { get; set; } = new List<IVote>();
    }
}
