using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsIdentifierAPI.Data;
using PlantsIdentifierAPI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlantsIdentifierAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantsController : ControllerBase
    {
        readonly ApplicationDBContext _applicationDBContext;

        public PlantsController(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        // GET api/plants
        [HttpGet]
        [ProducesResponseType(200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<IEnumerable<string>> Get()
        {
            try
            {
                var plants = _applicationDBContext.Plant.ToList();
                if (plants.Count == 0)
                    return new EmptyResult();
                else
                    return Ok(plants);
            }
            catch (ArgumentNullException argEx)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, argEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        // GET api/plants/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        //Returns this because the Plant may not exist
        [ProducesResponseType(404)]
        public async Task<ActionResult<Plant>> Get(int ID)
        {
            try
            {
                var plant = await _applicationDBContext.Plant.FirstOrDefaultAsync(p => p.ID.Equals(ID));
                if (plant == null)
                    return NotFound(new { ID = ID, Error = "No plant with the provided ID." });
                return Ok(plant);
            }
            catch (Exception ex)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST api/plants
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public ActionResult<bool> Post([FromBody] Plant plant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var existingPlant = _applicationDBContext.Plant.FirstOrDefault(p => p.CommonName == plant.CommonName);
                if (existingPlant != null)
                    return Conflict();
                _applicationDBContext.Plant.Add(plant);
                _applicationDBContext.SaveChanges();
                return Ok(true);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, dbEx.Message);
            }
        }
    }
}
