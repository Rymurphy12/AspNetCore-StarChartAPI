using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var model = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if ( model == null)
            {
                return NotFound();
            }

            model.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();

            return Ok(model);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var results = _context.CelestialObjects.Where(x => x.Name == name).ToList();

            if (results == null || results.Count == 0)
            {
                return NotFound();
            }

            foreach (var result in results)
            {
                result.Satellites = _context.CelestialObjects.Where(x => x.Id == result.Id).ToList();
            }
            
            return Ok(results);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var results = _context.CelestialObjects.ToList();

            foreach (var result in results)
            {
                result.Satellites = _context.CelestialObjects.Where(x => x.Id == result.Id).ToList();
            }

            return Ok(results);
        }
    }
}
