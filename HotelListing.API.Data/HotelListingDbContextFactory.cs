using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HotelListing.API.Data
{
   /* public class HotelListingDbContextFactory : IDesignTimeDbContextFactory<HotelListingDbContext>
    {
        public HotelListingDbContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json",optional:false,reloadOnChange:true)
                 .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HotelListingDbContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("HotelListingDbConnectionString"));
            return new HotelListingDbContext(optionsBuilder.Options);
        }
    }*/
}
