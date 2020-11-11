using EfErrorTest.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EfErrorTest
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public static void UseSqlServer(DbContextOptionsBuilder builder, string connectionString)
        {
            builder.UseSqlServer(connectionString, x => x.UseNetTopologySuite().EnableRetryOnFailure()).EnableSensitiveDataLogging();
        }
    }

    public class DbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            AppDbContext.UseSqlServer(builder, "Data Source=(local); Initial Catalog=EfErrorTest;User=EfErrorTest; Password=EfErrorTest;");

            return new AppDbContext(builder.Options);
        }
    }
}
