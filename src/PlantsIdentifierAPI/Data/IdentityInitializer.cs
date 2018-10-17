using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace PlantsIdentifierAPI.Data
{
    public class IdentityInitializer
    {
        readonly ApplicationDBContext _context;
        readonly UserManager<ApplicationUser> _userManager;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly IConfiguration _configuration;

        public IdentityInitializer (ApplicationDBContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public void Initialize()
        {
            if (_context.Database.EnsureCreated())
            {
                if (!_roleManager.RoleExistsAsync(ApplicationRoles.ADMINROLE).Result)
                {
                    var resultado = _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.ADMINROLE)).Result;
                    if (!resultado.Succeeded)
                    {
                        throw new Exception($"An error ocurred while creating the role {ApplicationRoles.ADMINROLE}.");
                    }
                }

                CreateUser(
                    new ApplicationUser()
                    {
                        UserName = "Admin",
                        Email = "admin@plantsidentifier.com",
                        EmailConfirmed = true
                    }, _configuration.GetValue<string>("AdminPassword"), ApplicationRoles.ADMINROLE);
            }
        }

        void CreateUser(ApplicationUser user, string password, string initialRole = null)
        {
            if (_userManager.FindByNameAsync(user.UserName).Result == null)
            {
                var resultado = _userManager.CreateAsync(user, password).GetAwaiter().GetResult();

                if (resultado.Succeeded && !string.IsNullOrWhiteSpace(initialRole))
                {
                    _userManager.AddToRoleAsync(user, initialRole).Wait();
                }
            }
        }
    }
}