using System;

namespace PlantsIdentifierAPI.Helpers
{
	public static class Constants
	{
		public const string WRONGEMAILORPASSWORD = "Wrong Email/Password. Please check your credentials.";
		public const string BADREQUEST = "An error occurred when validating your request.";
		public const string INVALIDREFRESHTOKEN = "Invalid refresh token";
		public const string USERALREADYEXISTS = "A user with this email already exists.";
		public const string PASSWORDMISMATCH = "The passwords don't match";
		public const string DATEFORMAT = "yyyy-MM-dd HH:mm:ss";
	}
}