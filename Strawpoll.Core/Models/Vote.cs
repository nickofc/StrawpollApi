namespace Strawpoll.Core.Models
{
    public class Vote : IVote
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Votes { get; set; }
    }
}
