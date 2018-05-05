using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Strawpoll.Core.Connection;
using Strawpoll.Core.Exceptions;
using Strawpoll.Core.Models;

namespace Strawpoll.Core.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly IParse _parse;

        public FormRepository(IConnection connection)
        {
            _parse = new Parse(connection);
        }

        public async Task<IForm> GetFormAsync(int formId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (formId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(formId));
            }

            var request = new Request(HttpMethod.Get, $"https://www.strawpoll.me/{formId}/r");

            return await _parse.ParseAsync(request, (document, content) =>
            {
                try
                {
                    var titleHtml = document.QuerySelector("#result-list > h1");
                    var votesHtml = document.QuerySelector("#result-list > div");

                    return new Form
                    {
                        Id = formId,
                        Title = titleHtml.InnerHtml,
                        Votes = votesHtml.Children.Select(element => new Vote
                        {

                            Id = int.Parse(element.GetAttribute("data-option-id")),
                            Name = element.Children.Single(x => x.ClassName == "option-text")
                                .Children[0].InnerHtml,
                            Votes = int.Parse(element.Children.Single(x => x.ClassName == "option-text")
                                .Children[1].GetAttribute("data-count")),
                        })
                    };
                }
                catch (Exception exception)
                {
                    throw new StrawpollException("Unable to parse response", exception);
                }
            }, response =>
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new FormNotFoundException($"{formId} not found.");
                }
            }, cancellationToken);
        }

        public async Task<IToken> GetTokenAsync(IForm form, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new Request(HttpMethod.Get, $"https://www.strawpoll.me/{form.Id}");
            return await _parse.ParseAsync(request, (document, content) =>
            {
                try
                {
                    return new Token
                    {

                        SecurityToken = document.QuerySelector("#field-security-token")
                                                .GetAttribute("value"),
                        AuthenticityToken = document.QuerySelector("#field-authenticity-token")
                                                    .GetAttribute("name"),
                    };
                }
                catch (Exception exception)
                {
                    throw new StrawpollException("Unable to handle response.", exception);
                }
            }, response =>
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new FormNotFoundException($"{form.Id} not found.");
                }
            }, cancellationToken);
        }

        public async Task<bool> VoteAsync(IForm form, IVote vote, CancellationToken cancellationToken = default(CancellationToken))
        {
            var token = await GetTokenAsync(form, cancellationToken).ConfigureAwait(false);

            var request = new Request(HttpMethod.Post, $"https://www.strawpoll.me/{form.Id}", new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("security-token", token.SecurityToken),
                new KeyValuePair<string, string>(token.AuthenticityToken, string.Empty),
                new KeyValuePair<string, string>("options", vote.Id.ToString()),
            }));

            return await _parse.ParseAsync(request, (document, content) => content.Contains("failed"), response =>
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new FormNotFoundException($"{form.Id} not found.");
                }
            }, cancellationToken);
        }
    }
}
