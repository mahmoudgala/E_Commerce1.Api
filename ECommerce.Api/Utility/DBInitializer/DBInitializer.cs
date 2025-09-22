using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace ECommerce.Api.Utility.DBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly ApplicationDbContext  _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DBInitializer> _logger;

        public DBInitializer(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<DBInitializer> logger)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        public void Initialize()
        {
           try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
                if (_roleManager.Roles.IsNullOrEmpty())
                {
                    _roleManager.CreateAsync(new(SD.SuperAdminRoles)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(SD.AdminRoles)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(SD.CompanyRoles)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(SD.CustomerRoles)).GetAwaiter().GetResult();
                }
                var result = _userManager.CreateAsync(new()
                {
                    Email = "Galal@gmail.com",
                    EmailConfirmed = true,
                    UserName = "Hoda",
                    Name = "Mahmoud Galal",

                }, "Galal123").GetAwaiter().GetResult();
                var user = _userManager.FindByEmailAsync("Galal@gmail.com").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(user, SD.SuperAdminRoles).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                _logger.LogError("Check Connection. Use DB  on Local Server (.)");
            }
        }
    }
}
