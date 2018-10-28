using PlantsIdentifierAPI.DTOS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlantsIdentifierAPI.Interfaces
{
	public interface IPlantsServices
	{
		Task<PlantDTO> GetPlant(Guid ID);
		IEnumerable<PlantDTO> GetAll();
		Task<PlantDTO> GetPlantByCommonName(string commonName);
		void SavePlant(PlantDTO plant);
	}
}
