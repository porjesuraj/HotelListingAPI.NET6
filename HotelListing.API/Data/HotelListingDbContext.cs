using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{
    public class HotelListingDbContext:DbContext
    {
        public HotelListingDbContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<Country> Countries { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hotel>().HasData(

                new Hotel
                {
                    Id = 1,
                    Name = "Hyatt",
                    Address ="Pune",
                    CountryId = 91,
                    Rating = 4.5
                },
                  new Hotel
                  {
                      Id = 2,
                      Name = "Mariot",
                      Address = "New York",
                      CountryId = 01,
                      Rating = 4
                  },
                   new Hotel
                   {
                       Id = 3,
                       Name = "Trident",
                       Address = "Berlin",
                       CountryId = 05,
                       Rating = 5
                   }

                );

            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id =91,
                    Name = "India",
                    ShortName = "IND"

                },
                new Country
                {
                    Id = 01,
                    Name = "United State of America",
                    ShortName = "USA"
                },
                new Country
                {
                    Id = 05,
                    Name = "Germany",
                    ShortName ="GER"
                }
                
                );
        }
    }
}
