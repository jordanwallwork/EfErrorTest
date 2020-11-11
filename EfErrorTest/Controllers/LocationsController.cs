using EfErrorTest.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EfErrorTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public LocationsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IEnumerable<Location> Get()
        {
            return _db.Locations.ToList();
        }
    }
}
