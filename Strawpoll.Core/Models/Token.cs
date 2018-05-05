namespace Strawpoll.Core.Models
{
    public class Token : IToken
    {
        public string SecurityToken { get; set; }
        public string AuthenticityToken { get; set; }
    }
}
