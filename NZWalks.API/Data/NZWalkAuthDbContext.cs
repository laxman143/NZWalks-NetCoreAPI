using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalkAuthDbContext : IdentityDbContext
    {
        public NZWalkAuthDbContext(DbContextOptions<NZWalkAuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "f12ef71e-bd9a-44be-a5db-0078a26509ab";

            var writerRoleId = "4704c442-7193-4d60-b01b-81ced5f4fb7c";

            var roles = new List<IdentityRole> {
                 new IdentityRole
                 {
                        Id = readerRoleId,
                        ConcurrencyStamp = readerRoleId,
                        Name = "Reader",
                        NormalizedName = "Reader".ToUpper()
                 },
                  new IdentityRole
                 {
                        Id = writerRoleId,
                        ConcurrencyStamp = writerRoleId,
                        Name = "Writer",
                        NormalizedName = "Writer".ToUpper()
                 }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
