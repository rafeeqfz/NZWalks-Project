using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalksAPI.Data
{
    public class NZWalkAuthDbContext : IdentityDbContext
    {
        public NZWalkAuthDbContext(DbContextOptions<NZWalkAuthDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRolesd = "a5db240c-3825-425f-8f0a-d6871aea45a0";
            var writerRoleId = "3b249408-933d-4493-9bcf-2283baaa7674";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
            {
                Id=readerRolesd,
                ConcurrencyStamp=readerRolesd,
                Name="Reader",
                NormalizedName="Reader".ToUpper()
            },
                    new IdentityRole
            {
                Id=writerRoleId,
                ConcurrencyStamp=writerRoleId,
                Name="Writer",
                NormalizedName="Writer".ToUpper()
            },
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }

}

