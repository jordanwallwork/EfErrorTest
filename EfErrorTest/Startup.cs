using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite.Geometries;
using System.Linq;

namespace EfErrorTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => AppDbContext.UseSqlServer(options, Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using (var db = scope.ServiceProvider.GetService<AppDbContext>())
            {
                db.Database.EnsureCreated();

                var testUser = db.Locations.FirstOrDefault(x => x.Name == "My Location");
                if (testUser == null)
                {
                    db.Locations.Add(new()
                    {
                        Name = "My Location",
                        Address = new()
                        {
                            Line1 = "1 Fake Street",
                            Town = "Fake Town",
                            County = "Fakeshire",
                            Postcode = "PO57 0DE",
                            Point = new Point(115.7930, 37.2431) { SRID = 4326 }
                        }
                    });
                }
                db.SaveChanges();
            }

            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
