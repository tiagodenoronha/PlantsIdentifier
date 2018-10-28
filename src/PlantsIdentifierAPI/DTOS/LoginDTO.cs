using System.ComponentModel.DataAnnotations;

namespace PlantsIdentifierAPI.DTOS
{
	public class LoginDTO
	{
		[Required]
		public string UserEmail { get; set; }

		[Required]
		public string Password { get; set; }
	}
}