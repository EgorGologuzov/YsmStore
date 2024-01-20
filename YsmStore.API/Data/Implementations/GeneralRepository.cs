using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using YsmStore.API.Data.Interfaces;
using YsmStore.Services.Utils;
using YsmStore.API.Data.Exceptions;

namespace YsmStore.API.Data.Implementations
{
    public class GeneralRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly PostgresContext Context;
        protected readonly ILogger Logger;

        public GeneralRepository(PostgresContext context, ILogger loger)
        {
            Context = context;
            Logger = loger;
        }

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            DbSet<TEntity> dbSet = Context.Set<TEntity>();
            TEntity result = (await dbSet.AddAsync(entity)).Entity;
            await Context.SaveChangesAsync();

            Logger.LogInformation("Created {type} creation data {data}", typeof(TEntity).Name, result.ToJson());

            return result;
        }

        public virtual async Task<TEntity> Update(object id, TEntity entity)
        {
            DbSet<TEntity> dbSet = Context.Set<TEntity>();
            TEntity oldEntity = await dbSet.FindAsync(id);

            if (oldEntity is null)
            {
                throw new EntityNotFoundException();
            }

            Context.Entry(oldEntity).CurrentValues.SetValues(entity);
            EntityEntry<TEntity> entry = dbSet.Update(oldEntity);
            TEntity result = entry.Entity;
            var original = (TEntity)entry.OriginalValues.ToObject();

            await Context.SaveChangesAsync();

            Logger.LogInformation("Updated {type} from {oldData} to {newData}", typeof(TEntity).Name, original.ToJson(), result.ToJson());

            return result;
        }

        public virtual async Task<TEntity> Delete(object id)
        {
            DbSet<TEntity> dbSet = Context.Set<TEntity>();
            TEntity entity = await dbSet.FindAsync(id);

            if (entity is null)
            {
                throw new EntityNotFoundException();
            }

            TEntity result = dbSet.Remove(entity).Entity;
            await Context.SaveChangesAsync();

            Logger.LogInformation("Deleted {type} with data {data}", typeof(TEntity).Name, result.ToJson());

            return result;
        }

        public virtual async Task<TEntity> GetById(object id)
        {
            DbSet<TEntity> dbSet = Context.Set<TEntity>();
            TEntity result = await dbSet.FindAsync(id);

            return result;
        }

        public virtual async Task<bool> IsExists(object id)
        {
            DbSet<TEntity> dbSet = Context.Set<TEntity>();
            return await dbSet.FindAsync(id) is not null;
        }
    }
}
