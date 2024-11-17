using IdentityDataProtection.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityDataProtection.Context
{
    public class DataProtectionJwtDbContext : DbContext
    {
        public DataProtectionJwtDbContext(DbContextOptions<DataProtectionJwtDbContext> options) : base(options)
        {

        }

        public DbSet<UserEntity> Users => Set<UserEntity>();
    }
}
