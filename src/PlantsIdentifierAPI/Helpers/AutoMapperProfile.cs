using AutoMapper;
using PlantsIdentifierAPI.Dtos;
using PlantsIdentifierAPI.Models;

namespace PlantsIdentifierAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Configure DTO mapping
            CreateMap<Plant, PlantDto>();
            CreateMap<PlantDto, Plant>();
        }
    }
}
