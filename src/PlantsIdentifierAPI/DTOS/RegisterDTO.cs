using System.ComponentModel.DataAnnotations;

namespace PlantsIdentifierAPI.DTOS
{
	public class RegisterDTO
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public string ConfirmPassword { get; set; }
	}
}
