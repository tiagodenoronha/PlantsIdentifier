using System;

namespace PlantsIdentifierAPI.Dtos
{
    public class PlantDto
    {
        public Guid ID { get; set; }
        public string PhotoURL { get; set; }
        public string CommonName { get; set; }

        public override string ToString()
        {
            return $"PlantDto(ID={ID},CommonName={CommonName},PhotoURL={PhotoURL})";
        }
    }
}
