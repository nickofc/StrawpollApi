namespace Strawpoll.Core.Models
{
    public interface IVote
    {
        int Id { get; }
        int Votes { get; }
        string Name { get; }
    }
}
