using Microsoft.EntityFrameworkCore;
using TestAppUNS.DAL.Entities;

namespace TestAppUNS.DAL
{
    public class DataContext: DbContext
    {
        public DbSet<OrderEntity> Orders { get; set; }

        public DbSet<ReportEntity> Reports { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public DbSet<T> DbSet<T>() where T : class, IEntity
        {
            return Set<T>();
        }

        public IQueryable<T> Query<T>() where T : class, IEntity
        {
            return Set<T>();
        }

    }
}
