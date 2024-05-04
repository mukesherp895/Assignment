using Assignment.DataAccess.DomainConfig;
using Assignment.Model.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Assignment.DataAccess.DBContext
{
    public class ApplicationDBContext : IdentityDbContext<Users>
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new CompanyInfoConfig());
            
        }
        public DbSet<CompanyInfos> CompanyInfos { get; set; }
    }
}
