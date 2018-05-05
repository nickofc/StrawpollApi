using Strawpoll.Core.Connection;
using Strawpoll.Core.Repositories;

namespace Strawpoll.Core
{
    public class StrawpollApi
    {
        private readonly IConnection _connection;
        private IFormRepository _formRepository;

        public IFormRepository FormRepository =>
            _formRepository ?? (_formRepository = new FormRepository(_connection));

        public StrawpollApi(IConnection connection)
        {
            _connection = connection ?? throw new System.ArgumentNullException(nameof(connection));
        }

        public StrawpollApi()
        {
            _connection = new StrawpollClient();
        }
    }
}
