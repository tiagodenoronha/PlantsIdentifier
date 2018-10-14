using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantsIdentifierAPI.Models
{
    public class Plant
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid ID { get; set; }

        public string PhotoURL { get; set; }

        //[Required(ErrorMessage = "Please input the Common Name for the Plant")]
        //[StringLength(50, ErrorMessage = "The First Name must be less than {1} characters.")]
        [Display(Name = "Common Name")]
        public string CommonName { get; set; }

    }
}
