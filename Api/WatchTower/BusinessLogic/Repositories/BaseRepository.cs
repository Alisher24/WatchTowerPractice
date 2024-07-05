using WatchTower.Server.BusinessLogic.Interfaces;

namespace WatchTower.Server.BusinessLogic.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly WatchTowerDbContext _dbContext;

        public BaseRepository(WatchTowerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public Task<TEntity> CreateAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbContext.Add(entity);
            _dbContext.SaveChanges();

            return Task.FromResult(entity);

        }

        public Task<TEntity> RemoveAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbContext.Remove(entity);
            _dbContext.SaveChanges();

            return Task.FromResult(entity);
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbContext.Update(entity);
            _dbContext.SaveChanges();

            return Task.FromResult(entity);
        }
    }
}
