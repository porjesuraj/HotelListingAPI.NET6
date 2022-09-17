﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Contract;
using HotelListing.API.Data;
using HotelListing.API.Models.Query;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
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
                return null;

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
    }
}
