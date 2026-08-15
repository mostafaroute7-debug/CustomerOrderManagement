using CustomerOrderManagement.Domain.Common;
using System.Linq;

namespace CustomerOrderManagement.Application.Interfaces.Repositories
{
    public interface IGenaricRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity GetById(int id);

        IQueryable<TEntity> GetAll();

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
