﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PlantsIdentifierAPI.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantsIdentifierAPI.UnitTests.Helpers
{
	public class FakeSignInManager : SignInManager<ApplicationUser>
	{
		public FakeSignInManager()
				: base(new Mock<FakeUserManager>().Object,
					 new Mock<IHttpContextAccessor>().Object,
					 new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
					 new Mock<IOptions<IdentityOptions>>().Object,
					 new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
					 new Mock<IAuthenticationSchemeProvider>().Object)
		{ }
	}
}
