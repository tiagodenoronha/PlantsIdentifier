using System;

namespace PlantsIdentifierAPI.DTOS
{
	public class PlantDTO
    {
        public Guid ID { get; set; }
        public string PhotoURL { get; set; }
        public string CommonName { get; set; }
    }
}
