using Inspection.Application.Dto;
using Inspection.Application.Response.Dto;
using System.Linq.Expressions;

namespace Inspection.DataAccessLayer.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T model);
        Task<T> AddAndGetIdAsync(T model);
        Task AddRangeAsync(IEnumerable<T> modelList);
        Task UpdateAsync(T model);
        Task UpdateRangeAsync(IEnumerable<T> model);
        Task HardDeleteAsync(object id);
        Task HardDeleteAsync(T model);
        Task HardDeleteRangeAsync(IEnumerable<T> model);
        Task DeleteAsync(object id);
        Task DeleteAsync(T model);
        Task<T?> FindAsync(object id);
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        void Update(T model);
        void Remove(T model);
        IQueryable<T> GetAsQueryable(Expression<Func<T, bool>>? predicate = null
            , Func<IQueryable<T>, IQueryable<T>>? orderBy = null
            , List<string>? include = null
            , bool disableTracking = true);

        Task<int> GetNextForSequenceAsync(string sequenceName);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IQueryable<T>>? orderBy = null, List<string>? include = null, bool disableTracking = true);
        Task<IEnumerable<T>> ToListAsync(IQueryable<T> query);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IQueryable<T>>? orderBy = null, List<string>? include = null, bool disableTracking = true);
        Task<PaginationResponse<T>> GetPaginatedAsync(IQueryable<T> query, int page = 0, int pageSize = 10);

    }
}
