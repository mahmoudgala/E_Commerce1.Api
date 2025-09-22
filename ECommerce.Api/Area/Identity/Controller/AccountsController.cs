using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Area.Identity.Controller
{
    [Route("api/[area]/[controller]")]
    [Area(SD.IdentityArea)]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepository<UserOTP> _userOTP;

        public AccountsController(UserManager<ApplicationUser> userManager, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager, IRepository<UserOTP> userOTP, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _userOTP = userOTP;
        }
        [HttpPost]


        //Register Post

        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {

            ApplicationUser applicationUser = new()
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Address = registerDto.Address,
                UserName = registerDto.UserName,
                FirstName = registerDto.FirstName,
                SecondName = registerDto.SecondName,
                State = registerDto.State,
                Street = registerDto.Street,
                City = registerDto.City,
                ZipCode = registerDto.ZipCode
            };
            //ApplicationUser applicationUser = registerVM.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(applicationUser, registerDto.Password);
            if (!result.Succeeded)
            {
                //foreach (var item in result.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, item.Description);
                //}
                //return Ok(registerDto);

                return BadRequest(result.Errors);

            }


            //Add User To Customer Role
            await _userManager.AddToRoleAsync(applicationUser, SD.CustomerRoles);

            //send comfirmation msg

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            var link = Url.Action("ConfirmEmail", "Account", new { area = "Identity", token = token, userId = applicationUser.Id }, Request.Scheme);

            await _emailSender.SendEmailAsync(applicationUser.Email, "Confirm Email", $"<h1>Confirm Your Email By Clicking <a href = '{link}'>Here</a></h1>");
            //Create Msg
            return Ok(new
            {
                msg = "Create User Successfully, Confirm Your Email!"
            });
        }
    }
}
