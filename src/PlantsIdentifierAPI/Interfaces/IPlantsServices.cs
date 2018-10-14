using PlantsIdentifierAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantsIdentifierAPI.Interfaces
{
	public interface IPlantsServices
	{
		Task<Plant> GetPlant(string ID);
		IEnumerable<Plant> GetAll();
		Task<Plant> GetPlantByCommonName(string commonName);
		void SavePlant(Plant plant);
	}
}
