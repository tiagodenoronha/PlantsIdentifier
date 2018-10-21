using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PlantsIdentifierAPI.Data;
using PlantsIdentifierAPI.Helpers;
using PlantsIdentifierAPI.Models;
using PlantsIdentifierAPI.Interfaces;
using PlantsIdentifierAPI.Services;

namespace PlantsIdentifierAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			///////////////////////// using a local .db file /////////////////////////
			//services.AddDbContext<ApplicationDBContext>(options => options.UseSqlite(Configuration.GetConnectionString("PlantContext")));

			/////////////////////////using a docker container with an SQL Server /////////////////////////
			services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("PlantsSQLServer")));

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDBContext>()
				.AddDefaultTokenProviders();

			var signingConfigurations = new SigningConfigurations(Configuration);
			services.AddSingleton(signingConfigurations);

			var tokenConfigurations = new TokenConfigurations();
			new ConfigureFromConfigurationOptions<TokenConfigurations>(
				Configuration.GetSection("TokenConfigurations"))
					.Configure(tokenConfigurations);
			services.AddSingleton(tokenConfigurations);

			services.AddAuthentication(authOptions =>
			{
				authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(bearerOptions =>
			{
				var paramsValidation = bearerOptions.TokenValidationParameters;
				paramsValidation.IssuerSigningKey = signingConfigurations.Key;
				paramsValidation.ValidAudience = tokenConfigurations.Audience;
				paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

				//Checks for a valid signature of a token
				paramsValidation.ValidateIssuerSigningKey = true;

				//Checks if the token has expired
				paramsValidation.ValidateLifetime = true;

				paramsValidation.ClockSkew = TimeSpan.Zero;

				//Handling refresh tokens
				bearerOptions.Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = context =>
					{
						if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
						{
							context.Response.Headers.Add("Token-Expired", "true");
						}
						return Task.CompletedTask;
					}
				};
			});

			//Activates the authorizing middleware
			services.AddAuthorization(auth =>
			{
				auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
					.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
					.RequireAuthenticatedUser().Build());
			});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.AddAutoMapper();

			services.AddScoped<ILoginService, LoginService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDBContext context,
		ILoggerFactory loggerFactory, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				//loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Trace);
			}
			else
			{
				app.UseHsts();
				//loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Information);
			}

			//Ensure the DB is filled
			new IdentityInitializer(context, userManager, roleManager, Configuration).Initialize();

			app.UseHttpsRedirection();
			app.UseMvcWithDefaultRoute();
		}
	}
}
