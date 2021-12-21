using CoreSmart.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace InfrastructureSmartDB.Configurations
{
    public class PropertiesConfiguration : IEntityTypeConfiguration<CoreSmart.Entities.Properties>
    {
        public void Configure(EntityTypeBuilder<Properties> builder)
        {
            builder.Property(e => e.PropertyID).HasColumnName("PropertyID");

            builder.Property(e => e.Name)
                .IsRequired();

            builder.Property(e => e.FormerName).HasColumnType("ntext");
            builder.Property(e => e.StreetAddress).HasColumnType("ntext");
            builder.Property(e => e.City).HasColumnType("ntext");
            builder.Property(e => e.Market).HasColumnType("ntext");
            builder.Property(e => e.State).HasColumnType("ntext");
            builder.Property(e => e.Lat).HasColumnName("Lat"); 
            builder.Property(e => e.Lng).HasColumnName("Lng");


        }
    }

}
