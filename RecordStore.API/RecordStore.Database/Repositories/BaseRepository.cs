using Microsoft.EntityFrameworkCore;
using RecordStore.Database.Context;
using RecordStore.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Database.Repositories
{
    public class BaseRepository<T>(RecordStoreDatabaseContext databaseContext) where T : BaseEntity
    {
        private DbSet<T> DbSet { get; } = databaseContext.Set<T>();

        public Task<List<T>> GetAllAsync(bool includeDeletedEntities = false)
        {
            return GetRecords(includeDeletedEntities).ToListAsync();
        }

        public void Insert(params T[] records)
        {
            DbSet.AddRange(records);
        }

        public void Update(params T[] records)
        {
            foreach (var baseEntity in records)
            {
                baseEntity.ModifiedAt = DateTime.UtcNow;
            }

            DbSet.UpdateRange(records);
        }

        public void SoftDelete(params T[] records)
        {
            foreach (var baseEntity in records)
            {
                baseEntity.DeletedAt = DateTime.UtcNow;
            }

            Update(records);
        }

        public Task SaveChangesAsync()
        {
            return databaseContext.SaveChangesAsync();
        }

        protected IQueryable<T> GetRecords(bool includeDeletedEntities = false)
        {
            var result = DbSet.AsQueryable();

            if (includeDeletedEntities is false)
            {
                result = result.Where(r => r.DeletedAt == null);
            }

            return result;
        }
    }
}
