using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsIdentifierAPI.Interfaces;
using PlantsIdentifierAPI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlantsIdentifierAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlantsController : ControllerBase
	{
		readonly IPlantsServices _plantsServices;

		public PlantsController(IPlantsServices plantsServices)
		{
			_plantsServices = plantsServices;
		}

		// GET api/plants
		[HttpGet]
		[ProducesResponseType(200)]
		public ActionResult<IEnumerable<Plant>> Get()
		{
			try
			{
				var plants = _plantsServices.GetAll();
				//var plants = _plantsContext.Plant;
				if (!plants.Any())
					return new EmptyResult();
				else
					return Ok(plants);
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
				var plant = await _plantsServices.GetPlant(ID.ToString());
				//var plant = await _plantsContext.Plant.FirstOrDefaultAsync(p => p.ID.Equals(ID));
				if (plant == null)
					return NotFound(new { ID, Error = "No plant with the provided ID." });
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
		public async Task<ActionResult<bool>> Post([FromBody] Plant plant)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var existingPlant = await _plantsServices.GetPlantByCommonName(plant.CommonName);
				//var existingPlant = _plantsContext.Plant.FirstOrDefault(p => p.CommonName == plant.CommonName);
				if (existingPlant != null)
					return Conflict();
				await _plantsServices.SavePlant(plant);
				//_plantsContext.Plant.Add(plant);
				//_plantsContext.SaveChanges();
				return Ok(true);
			}
			catch (DbUpdateException dbEx)
			{
				return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, dbEx.Message);
			}
		}
	}
}
