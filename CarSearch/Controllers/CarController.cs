using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSearch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : Controller
    {
        private readonly CarDbContext _context;
        public CarController(CarDbContext context)
        {
            _context = context;
            if (_context.Vehicles.Count() == 0) //will only add if vehicle table is empty?
            {
                _context.Vehicles.Add(new Vehicles { Make = "Jeep", Model = "Grand Cherokee Trailhawk", Year = 2019, Color="Black" });
            } //here so API has something to call on, if empty, API would be getting an empty table
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicles>>> GetCars()
        //returning a List Of Vehicles
        {
            var carList = await _context.Vehicles.ToListAsync();
            return carList;
            //similar to listing out vehicles in MVC
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Vehicles>> GetVehicleById(int Id)
        //actionresult allows us to return json/string type of vehicle
        {
            var found = await _context.Vehicles.FindAsync(Id);
            if (found == null)
            {
                return NotFound(); //error 404 basically
            }
            return found; //returning full object
        }

        [HttpPost] // POST--Adding an vehicle
        public async Task<ActionResult<Vehicles>> PostVehicle(Vehicles car)
        {
            _context.Vehicles.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVehicleById), new { id = car.Id }, car);

            //will return http 201 status code if successful
        }

        [HttpPut("{Id}")] // /api/company/1--(id)-- PUT: UPDATING Vehicle
        public async Task<ActionResult<Vehicles>> PutCar(int Id, Vehicles vehicle)
        {
            if (Id != vehicle.Id)
            {
                return BadRequest(); 
            }

            _context.Entry(vehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Vehicles>> DeleteVehicle(int Id)
        {
            var car = await _context.Vehicles.FindAsync(Id);
            if (car == null)
            {
                return NotFound();
            }
            _context.Vehicles.Remove(car);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}