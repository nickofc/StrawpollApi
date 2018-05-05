using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Strawpoll.Core;
using Strawpoll.Core.Connection;
using Strawpoll.Core.Exceptions;
using Strawpoll.Core.Models;
using Xunit;

namespace Strawpoll.Tests
{
    public class FormRepositoryTests
    {
        [Fact]
        public async Task Should_return_form()
        {
            var strawpoll = new StrawpollApi();
            var formId = 15612959;
            var form = await strawpoll.FormRepository.GetFormAsync(formId, CancellationToken.None);

            Assert.NotNull(form);
            Assert.NotNull(form.Title);
            Assert.NotEmpty(form.Votes);

            var token = await strawpoll.FormRepository.GetTokenAsync(form, CancellationToken.None);
            Assert.False(string.IsNullOrWhiteSpace(token.AuthenticityToken));
            Assert.False(string.IsNullOrWhiteSpace(token.SecurityToken));
        }

        [Fact]
        public async Task Should_throw_exception()
        {
            var strawpoll = new StrawpollApi();
            await Assert.ThrowsAsync<FormNotFoundException>(async () => await strawpoll.FormRepository.GetFormAsync(int.MaxValue, CancellationToken.None));
        }

        [Fact]
        public async Task Should_throw_core_exception()
        {
            var connectionMock = new Mock<IConnection>();
            connectionMock
                .Setup(connection => connection.SendAsync(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response{StatusCode = HttpStatusCode.OK, Content = string.Empty});

            var strawpoll = new StrawpollApi(connectionMock.Object);
            await Assert.ThrowsAsync<StrawpollException>(async () =>
                await strawpoll.FormRepository.GetFormAsync(int.MaxValue, CancellationToken.None));

            await Assert.ThrowsAsync<StrawpollException>(async () =>
                await strawpoll.FormRepository.VoteAsync(new Form(), new Vote(), CancellationToken.None));

            await Assert.ThrowsAsync<StrawpollException>(async () =>
                await strawpoll.FormRepository.GetTokenAsync(new Form(), CancellationToken.None));
        }

        [Fact]
        public async Task Should_throw_outofrange_exception()
        {
            var strawpoll = new StrawpollApi();

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await strawpoll.FormRepository.GetFormAsync(0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
                await strawpoll.FormRepository.GetFormAsync(-1, CancellationToken.None));
        }
    }
}
