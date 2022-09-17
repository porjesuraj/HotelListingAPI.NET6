using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations
{
    public class CountriesConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                new Country
                {
                    Id = 91,
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
                    ShortName = "GER"
                });


        }
    }
}
