using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsIdentifierAPI.Interfaces;
using PlantsIdentifierAPI.DTOS;
using PlantsIdentifierAPI.Models;

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
		public ActionResult<IEnumerable<PlantDTO>> Get()
		{
			try
			{
				var plants = _plantsServices.GetAll();
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
		public async Task<ActionResult<PlantDTO>> Get(string ID)
		{
			try
			{
				var plant = await _plantsServices.GetPlant(ID);
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
		public async Task<ActionResult<bool>> Post([FromBody] PlantDTO plant)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var existingPlant = await _plantsServices.GetPlantByCommonName(plant.CommonName);
				if (existingPlant != null)
					return Conflict();
				_plantsServices.SavePlant(plant);
				return Ok(true);
			}
			catch (DbUpdateException dbEx)
			{
				return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, dbEx.Message);
			}
		}
	}
}
