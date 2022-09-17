using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations
{
    public class HotelsConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(

                new Hotel
                {
                    Id = 1,
                    Name = "Hyatt",
                    Address = "Pune",
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
        }
    }
}
