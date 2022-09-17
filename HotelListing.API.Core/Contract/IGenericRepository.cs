using HotelListing.API.Core.Models.Query;

namespace HotelListing.API.Core.Contract
{
    public interface IGenericRepository<T> where T:class
    {
        Task<T> GetAsync(int? id);

        Task<List<T>> GetAllAsync();

        Task<PageResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);

        Task<T> AddAsync(T entity);

        Task DeleteAsync(int id);

        Task UpdateAsync(T entity);

        Task<bool> RowExists(int id);

        bool TableExist();
    }
}
