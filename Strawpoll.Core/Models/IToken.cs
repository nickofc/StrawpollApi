namespace Strawpoll.Core.Models
{
    public interface IToken
    {
        string SecurityToken { get; }
        string AuthenticityToken { get; }
    }
}