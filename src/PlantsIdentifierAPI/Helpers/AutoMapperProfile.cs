using AutoMapper;
using PlantsIdentifierAPI.DTOS;
using PlantsIdentifierAPI.Models;

namespace PlantsIdentifierAPI.Helpers
{
	public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Configure DTO mapping
            CreateMap<Plant, PlantDTO>();
            CreateMap<PlantDTO, Plant>();
        }
    }
}
