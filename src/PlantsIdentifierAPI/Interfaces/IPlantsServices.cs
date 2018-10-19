using PlantsIdentifierAPI.DTOS;
using PlantsIdentifierAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantsIdentifierAPI.Interfaces
{
	public interface IPlantsServices
	{
		Task<PlantDTO> GetPlant(string ID);
		IEnumerable<PlantDTO> GetAll();
		Task<PlantDTO> GetPlantByCommonName(string commonName);
		void SavePlant(PlantDTO plant);
	}
}
