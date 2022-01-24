using Chapkhone.DataAccess.Context;
using Chapkhone.DataAccess.Services.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Services.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(ChapkhoneContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<TEntity> FindAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, params string[] includeProperties)
        {
            var entities = await GetAllAsync(filter: filter, includeProperties: includeProperties);

            return entities.FirstOrDefault();
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] includeProperties)
        {
            IQueryable<TEntity> entities = _dbSet;

            if (filter != null)
                entities = entities.Where(filter);

            if (includeProperties.Length > 0)
            {
                foreach (var includeProperty in includeProperties)
                    entities = entities.Include(includeProperty);
            }

            if (orderBy != null)
                entities = orderBy(entities);

            return await entities.ToListAsync();
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async void Delete(object id)
        {
            var entity = await FindAsync(id);
            Delete(entity);
        }
    }
}