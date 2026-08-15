using CustomerOrderManagement.Application.Interfaces.Repositories;
using CustomerOrderManagement.Infrastructure.Data.Contexts;
using System.Data.Entity;
using System.Linq;
using CustomerOrderManagement.Domain.Common;

namespace CustomerOrderManagement.Infrastructure.Data.Repositories
{
    public class GenaricRepository<TEntity> : IGenaricRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public GenaricRepository(ApplicationDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public TEntity GetById(int id)
        {
            return DbSet.Find(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet.AsQueryable();
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }
    }
}
