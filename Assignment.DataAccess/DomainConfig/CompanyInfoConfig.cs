using Assignment.Model.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Assignment.DataAccess.DomainConfig
{
    public class CompanyInfoConfig : IEntityTypeConfiguration<CompanyInfos>
    {
        public void Configure(EntityTypeBuilder<CompanyInfos> builder)
        {
            builder.Property(p => p.CompanyName).IsRequired().HasMaxLength(255).HasColumnType("varchar");
            builder.Property(p => p.Schema).IsRequired().HasMaxLength(5).HasColumnType("varchar");
            builder.HasIndex(p => p.Schema).IsUnique();
        }
    }
}
