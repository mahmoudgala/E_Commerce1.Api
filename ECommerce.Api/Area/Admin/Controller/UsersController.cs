using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Area.Admin.Controller
{
    [Area(SD.AdminArea)]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users;
            return Ok(users.Adapt<List<UsersResponse>>());
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> LockUnLock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();
            user.LockoutEnabled = !user.LockoutEnabled;
            if (!user.LockoutEnabled)
            {
                user.LockoutEnd = DateTime.UtcNow.AddDays(2);
            }
            else
            {
                user.LockoutEnd = null;
            }
            await _userManager.UpdateAsync(user);
            return NoContent();
        }
    }
}
