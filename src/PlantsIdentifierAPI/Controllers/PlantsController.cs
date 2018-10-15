using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsIdentifierAPI.Dtos;
using PlantsIdentifierAPI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlantsIdentifierAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantsController : ControllerBase
    {
        private readonly PlantsContext _plantsContext;
        private readonly IMapper _mapper;

        public PlantsController(PlantsContext plantsContext, IMapper mapper)
        {
            _plantsContext = plantsContext;
            _mapper = mapper;
        }

        // GET api/plants
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<PlantDto>> Get()
        {
            try
            {
                var plants = _plantsContext.Plant.ToList();
                if (plants.Count == 0)
                    return new EmptyResult();
                else
                {
                    var plantDtos = _mapper.Map<IList<PlantDto>>(plants);
                    return Ok(plantDtos);
                }
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
        public async Task<ActionResult<Plant>> Get(Guid ID)
        {
            try
            {
                var plant = await _plantsContext.Plant.FirstOrDefaultAsync(p => p.ID.Equals(ID));
                if (plant == null)
                    return NotFound(new { ID = ID, Error = "No plant with the provided ID." });
                var plantDto = _mapper.Map<PlantDto>(plant);
                return Ok(plantDto);
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
        public ActionResult<bool> Post([FromBody] PlantDto plantDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var plant = _mapper.Map<Plant>(plantDto);
                var existingPlant = _plantsContext.Plant.FirstOrDefault(p => p.CommonName == plant.CommonName);
                if (existingPlant != null)
                    return Conflict();
                _plantsContext.Plant.Add(plant);
                _plantsContext.SaveChanges();
                return Ok(true);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, dbEx.Message);
            }
        }
    }
}
