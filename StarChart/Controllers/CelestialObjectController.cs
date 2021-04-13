using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject newCelestialObject)
        {
            _context.CelestialObjects.Add(newCelestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = newCelestialObject.Id }, newCelestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var existingCelestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if (existingCelestialObject == null)
            {
                return NotFound();
            }
            existingCelestialObject.Name = celestialObject.Name;
            existingCelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            existingCelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(existingCelestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var existingCelestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if (existingCelestialObject == null)
            {
                return NotFound();
            }

            existingCelestialObject.Name = name;

            _context.CelestialObjects.Update(existingCelestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingCelestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if (existingCelestialObject == null)
            {
                return NotFound();
            }

            _context.CelestialObjects.Remove(existingCelestialObject);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
