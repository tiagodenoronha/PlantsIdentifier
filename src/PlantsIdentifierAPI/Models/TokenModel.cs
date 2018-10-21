namespace PlantsIdentifierAPI.Models
{
	public class TokenModel
	{
		public bool Authenticated { get; internal set; }
		public string CreatedDate { get; internal set; }
		public object ExpirationDate { get; internal set; }
		public string AccessToken { get; internal set; }
		public string RefreshToken { get; internal set; }
	}
}