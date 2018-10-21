using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlantsIdentifierAPI.DTOS;
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
        readonly IMapper _mapper;

        public PlantsServices(PlantsContext plantsContext, IMapper mapper)
        {
            _plantsContext = plantsContext;
            _mapper = mapper;
        }

        public IEnumerable<PlantDTO> GetAll()
        {
            var plants = _plantsContext.Plant.ToArray();
            for (var i = 0; i < plants.Count(); i++)
                yield return _mapper.Map<PlantDTO>(plants[i]);
        }
        public async Task<PlantDTO> GetPlant(Guid ID)
        {
            var plant = await _plantsContext.Plant.FirstOrDefaultAsync(p => p.ID.Equals(ID));
            return _mapper.Map<PlantDTO>(plant);
        }
        public async Task<PlantDTO> GetPlantByCommonName(string commonName) => _mapper.Map<PlantDTO>(await _plantsContext.Plant.FirstOrDefaultAsync(p => p.CommonName == commonName));
        public void SavePlant(PlantDTO plant)
        {
            _plantsContext.Plant.Add(_mapper.Map<Plant>(plant));
            _plantsContext.SaveChanges();
        }
    }
}