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

		public IEnumerable<Plant> GetAll() => throw new NotImplementedException();
		public Task<Plant> GetPlant(string ID) => throw new NotImplementedException();
		public Task<Plant> GetPlantByCommonName(string commonName) => throw new NotImplementedException();
		public Task SavePlant(Plant plant) => throw new NotImplementedException();
	}
}
