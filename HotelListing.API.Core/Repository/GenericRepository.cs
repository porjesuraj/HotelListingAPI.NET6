using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Contract;
using HotelListing.API.Data;
using HotelListing.API.Core.Models.Query;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Core.Exceptions;

namespace HotelListing.API.Core.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelListingDbContext _hotelListingDbContext;
        private readonly IMapper _mapper;
        public GenericRepository(HotelListingDbContext hotelListingDbContext, IMapper mapper)
        {
            _hotelListingDbContext = hotelListingDbContext;
            _mapper = mapper;
        }
        public async Task<T> AddAsync(T entity)
        {
          await _hotelListingDbContext.AddAsync(entity);
          await _hotelListingDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if(entity is null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }

            _hotelListingDbContext.Set<T>().Remove(entity);
            await _hotelListingDbContext.SaveChangesAsync();

        }

        public async Task<bool> RowExists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _hotelListingDbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int? id)
        {
            if (id is null)
                    throw new NotFoundException(typeof(T).Name, id);                

            return await _hotelListingDbContext.Set<T>().FindAsync(id);
        }

        public  bool TableExist()
        {
            var result =  _hotelListingDbContext.Set<T>() != null;
            return result;
        }

        public async Task UpdateAsync(T entity)
        {           
                _hotelListingDbContext.Update(entity);
                await _hotelListingDbContext.SaveChangesAsync();

            
   
        }

        public async Task<PageResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
        {
            var totalSize = await _hotelListingDbContext.Set<T>().CountAsync();
            var items = await _hotelListingDbContext.Set<T>().
                Skip(queryParameters.StartIndex)
                .Take(queryParameters.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PageResult<TResult>
            {
                Items = items,
                TotalCount = totalSize,
                PageNumber = queryParameters.PageNumber,
                RecordNumber = items.Count
            };
        }

        public async Task<TResult> GetAsync<TResult>(int? id)
        {
            var entity = await _hotelListingDbContext.Set<T>().FindAsync(id);

            if (entity is null)
               throw new NotFoundException(typeof(T).Name, id.HasValue? id:"No Key Provided");


           return _mapper.Map<TResult>(entity);
                
        }

        public async Task<List<TResult>> GetAllAsync<TResult>()
        {
            return await _hotelListingDbContext.Set<T>()
                        .ProjectTo<TResult>(_mapper.ConfigurationProvider).
                        ToListAsync();
        }

        public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
        {
            var entity = _mapper.Map<T>(source);

            await _hotelListingDbContext.AddAsync(entity);
            await _hotelListingDbContext.SaveChangesAsync();
            return _mapper.Map<TResult>(entity);
        }

        public async Task UpdateAsync<TSource>(int id, TSource source)
        {
            var entity = await GetAsync(id);

            if (entity is null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }

            _mapper.Map(source, entity);
            _hotelListingDbContext.Update(entity);

            await _hotelListingDbContext.SaveChangesAsync();
        }
    }
}
