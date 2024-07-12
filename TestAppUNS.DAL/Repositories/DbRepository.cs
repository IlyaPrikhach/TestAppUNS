using System.Linq.Expressions;
using TestAppUNS.DAL.Entities;
using TestAppUNS.DAL.Repositories.Interfaces;

namespace TestAppUNS.DAL.Repositories
{
    public class DbRepository : IDbRepository
    {
        private readonly DataContext _context;

        public DbRepository(DataContext context) { 
            _context = context; 
        }

        public async Task<int> Add<T>(T newEntity) where T : class, IEntity
        {
            var entity = await _context.Set<T>().AddAsync(newEntity);
            await SaveChangesAsync();
            return entity.Entity.Id;
        }

        public async Task Delete<T>(T entity) where T : class, IEntity
        {
            _context.Set<T>().Remove(entity);
            await SaveChangesAsync();
        }

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> expression) where T : class, IEntity
        {
            return _context.Set<T>().Where(expression).AsQueryable();
        }

        public IQueryable<T> GetAll<T>() where T : class, IEntity
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task Update<T>(T entity) where T : class, IEntity
        {
            _context.Set<T>().Update(entity);
            await SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
