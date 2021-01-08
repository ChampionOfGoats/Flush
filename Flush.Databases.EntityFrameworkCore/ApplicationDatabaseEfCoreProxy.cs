using Microsoft.Extensions.Logging;
namespace Flush.Databases.EntityFrameworkCore
{
    public class ApplicationDatabaseEfCoreProxy : IApplicationDatabaseProxy
    {
        private readonly ILogger<ApplicationDatabaseEfCoreProxy> logger;
        private readonly ApplicationContext context;

        public ApplicationDatabaseEfCoreProxy(
            ILogger<ApplicationDatabaseEfCoreProxy> logger,
            ApplicationContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void EndSession()
        {
            throw new System.NotImplementedException();
        }

        public IRoom GetOrCreateRoom()
        {
            throw new System.NotImplementedException();
        }

        public ISession GetOrStartSession()
        {
            throw new System.NotImplementedException();
        }

        public void SetRoomName()
        {
            throw new System.NotImplementedException();
        }

        public void SetRoomOwner()
        {
            throw new System.NotImplementedException();
        }

        public void Trace()
        {
            throw new System.NotImplementedException();
        }
    }
}
