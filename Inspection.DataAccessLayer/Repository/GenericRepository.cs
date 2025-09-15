using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Inspection.DataAccessLayer.Context;
using Inspection.Application.Response.Dto;

namespace Inspection.DataAccessLayer.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly InspectionDbContext _db;

        public GenericRepository(InspectionDbContext db)
        {
            _db = db;
         }

        public async Task AddAsync(T model)
        {
            await _db.Set<T>().AddAsync(model);
        }
        public async Task<T> AddAndGetIdAsync(T model)
        {
            await _db.Set<T>().AddAsync(model);
            return model;
        }
        public async Task AddRangeAsync(IEnumerable<T> modellst)
        {
            await _db.Set<T>().AddRangeAsync(modellst);
        }

        public Task UpdateAsync(T model)
        {
            _db.Set<T>().Update(model);
            return Task.CompletedTask;
        }

        public Task UpdateRangeAsync(IEnumerable<T> model)
        {
            _db.Set<T>().UpdateRange(model);
            return Task.CompletedTask;
        }

        public async Task HardDeleteAsync(object id)
        {
            var entityToDelete = await FindAsync(id);
            if (entityToDelete != null)
            {
                await DeleteAsync(entityToDelete);
            }
        }

        public Task HardDeleteAsync(T model)
        {
            if (_db.Entry(model).State == EntityState.Detached)
            {
                _db.Set<T>().Attach(model);
            }

            _db.Set<T>().Remove(model);
            return Task.CompletedTask;

        }
        public Task HardDeleteRangeAsync(IEnumerable<T> model)
        {
            _db.Set<T>().RemoveRange(model);
            return Task.CompletedTask;

        }
        public async Task DeleteAsync(object id)
        {
            var entityToDelete = await FindAsync(id);
            if (entityToDelete != null)
            {
                var property = entityToDelete.GetType().GetProperty("IsDelete");

                var propertyValue = (bool?)property?.GetValue(entityToDelete);

                if (!propertyValue.HasValue)
                {
                    property?.SetValue(entityToDelete, true);
                }
            }
        }

        public async Task<T?> FindAsync(object? id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public IQueryable<T> GetAsQueryable(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IQueryable<T>>? orderBy = null, List<string>? include = null, bool disableTracking = true)
        {
            var query = _db.Set<T>().AsQueryable();

            if (include != null) query = include.Aggregate(query, (current, item) => current.Include(item));

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            if (disableTracking)
                query.AsNoTracking();

            return query;
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IQueryable<T>>? orderBy = null, List<string>? include = null, bool disableTracking = true)
        {
            var query = _db.Set<T>().AsQueryable();

            if (include != null) query = include.Aggregate(query, (current, item) => current.Include(item));

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            if (disableTracking)
                query.AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task<PaginationResponse<T>> GetPaginatedAsync(IQueryable<T> query, int page = 0, int pageSize = 10)
        {
            var skip = page * pageSize;
            var take = pageSize;

            var values = await query
                .Skip(skip).Take(take)
                .ToListAsync();

            var totalCount = await query.CountAsync();

            return new PaginationResponse<T>
            {
                Values = values,
                Count = values.Count,
                Page = page,
                TotalCount = totalCount
            };
        }

        public async Task<IEnumerable<T>> ToListAsync(IQueryable<T> query)
        {
            return await query.ToListAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IQueryable<T>>? orderBy = null, List<string>? include = null, bool disableTracking = true)
        {
            var query = _db.Set<T>().AsQueryable();

            if (include != null)
                query = include.Aggregate(query, (current, item) => current.Include(item));

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            if (disableTracking)
                query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public Task<int> GetNextForSequenceAsync(string sequenceName)
        {
            //int res = await _db.Database.ExecuteSqlRawAsync("select demo.\"RequestNumber\".nextval from dual;");

            using var command = _db.Database.GetDbConnection().CreateCommand();
            command.CommandText = $"SELECT \"REQUESTNUMBER\".NEXTVAL FROM DUAL";
            _db.Database.OpenConnection();
            using var reader = command.ExecuteReader();
            reader.Read();
            return Task.FromResult(reader.GetInt32(0));
        }

        // Missing methods implementation
        public Task DeleteAsync(T model)
        {
            if (_db.Entry(model).State == EntityState.Detached)
            {
                _db.Set<T>().Attach(model);
            }

            _db.Set<T>().Remove(model);
            return Task.CompletedTask;
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _db.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _db.Set<T>().AnyAsync(predicate);
        }

        public void Update(T model)
        {
            _db.Set<T>().Update(model);
        }

        public void Remove(T model)
        {
            _db.Set<T>().Remove(model);
        }

    }
}
