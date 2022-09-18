using HotelListing.API.Core.Models.Query;

namespace HotelListing.API.Core.Contract
{
    public interface IGenericRepository<T> where T:class
    {
        Task<T> GetAsync(int? id);

        Task<TResult> GetAsync<TResult>(int? id);

        Task<List<T>> GetAllAsync();

        Task<List<TResult>> GetAllAsync<TResult>();

        Task<PageResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);

        Task<T> AddAsync(T entity);

        Task<TResult> AddAsync<TSource, TResult>(TSource source);


        Task DeleteAsync(int id);

        Task UpdateAsync(T entity);

        Task UpdateAsync<TSource>(int id, TSource source);

        Task<bool> RowExists(int id);

        bool TableExist();
    }
}
