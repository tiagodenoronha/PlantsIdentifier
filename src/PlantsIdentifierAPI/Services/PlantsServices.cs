using Microsoft.EntityFrameworkCore;
using PlantsIdentifierAPI.Interfaces;
using PlantsIdentifierAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantsIdentifierAPI.Services
{
	public class PlantsServices : IPlantsServices
	{
		readonly PlantsContext _plantsContext;

		public PlantsServices(PlantsContext plantsContext)
		{
			_plantsContext = plantsContext;
		}

		public IEnumerable<Plant> GetAll() => _plantsContext.Plant.ToArray();
		public async Task<Plant> GetPlant(string ID) => await _plantsContext.Plant.FirstOrDefaultAsync(p => p.ID.Equals(ID));
		public async Task<Plant> GetPlantByCommonName(string commonName) => await _plantsContext.Plant.FirstOrDefaultAsync(p => p.CommonName == commonName);
		public void SavePlant(Plant plant)
		{
			_plantsContext.Plant.Add(plant);
			_plantsContext.SaveChanges();
		}
	}
}