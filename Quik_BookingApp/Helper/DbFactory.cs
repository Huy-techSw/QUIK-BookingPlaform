using Microsoft.EntityFrameworkCore;
using Quik_BookingApp.DAO;

namespace Quik_BookingApp.Helper
{
    public class DbFactory : IDisposable
    {
        private bool _disposed;
        private Func<QuikDbContext> _instanceFunc;
        private DbContext _dbContext;
        public DbContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());

        public DbFactory(Func<QuikDbContext> dbContextFactory)
        {
            _instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            if (!_disposed && _dbContext != null)
            {
                _disposed = true;
                _dbContext.Dispose();
            }
        }
    }
}
