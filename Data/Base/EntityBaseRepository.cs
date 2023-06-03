using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoraetionTask.Data.Base
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private readonly AppDbContext _db;
        public EntityBaseRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<T>> GetAllAsync() => await _db.Set<T>().ToListAsync();


        public async Task<T> GetByIDAsync(int id) => await _db.Set<T>().FirstOrDefaultAsync(a => a.ID == id);

        public async Task<T> AddAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            _db.SaveChanges();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.Set<T>().FirstOrDefaultAsync(n => n.ID == id);
            EntityEntry entityEntry = _db.Entry<T>(entity);
            entityEntry.State = EntityState.Deleted;

            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, T entity)
        {
            // Get the existing entity with the same 'Id' value, if any
            var existingEntity = await _db.Set<T>().FindAsync(id);

            if (existingEntity != null)
            {
                // Detach the existing entity from the context to avoid conflicts
                _db.Entry(existingEntity).State = EntityState.Detached;
            }

            // Attach the new entity to the context
            _db.Entry(entity).State = EntityState.Modified;

            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _db.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _db.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.FirstOrDefaultAsync(n => n.ID == id);
        }
    }
}
