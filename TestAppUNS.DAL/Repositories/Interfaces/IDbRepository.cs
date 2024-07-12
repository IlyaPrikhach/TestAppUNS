using System.Linq.Expressions;
using TestAppUNS.DAL.Entities;

namespace TestAppUNS.DAL.Repositories.Interfaces
{
    public interface IDbRepository
    {
        IQueryable<T> GetAll<T>() where T : class, IEntity;

        IQueryable<T> Get<T>(Expression<Func<T, bool>> expression) where T : class, IEntity;

        Task<int> Add<T>(T entity) where T : class, IEntity;

        Task Update<T>(T entity) where T : class, IEntity;

        Task Delete<T>(T entity) where T : class, IEntity;

        Task<int> SaveChangesAsync();
    }
}
