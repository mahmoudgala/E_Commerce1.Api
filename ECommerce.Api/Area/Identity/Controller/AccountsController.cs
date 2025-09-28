using ECommerce.Api.DTOs.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        [Route("Register")]


        //Register Post

        public async Task<IActionResult> Register(RegisterRequest registerDto)
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
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.EmailOrUserName) ?? await _userManager.FindByNameAsync(loginDTO.EmailOrUserName);
            if (user is null)
            {
                return BadRequest(new NotificationResponse
                {
                    MSG = "Invalid User Name or password",
                    TraceId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow

                });
            }
            //Check pass
            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, loginDTO.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    return BadRequest(new NotificationResponse
                    {
                        MSG = "Too many attempts",
                        TraceId = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.UtcNow
                    });
                return NotFound(new NotificationResponse
                {
                    MSG = "Invalid user name or password",
                    TraceId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow
                });
            }
            if (!user.EmailConfirmed)
                return BadRequest(new NotificationResponse
                {
                    MSG = "Confirm your Email and try again",
                    TraceId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow
                });
            if (!user.LockoutEnabled)
                return BadRequest(new NotificationResponse
                {
                    MSG = "You are locked, try again later",
                    TraceId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow
                });
            return Ok(new NotificationResponse
            {
                MSG = "Login Successfully",
                TraceId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow
            });
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
                 return NotFound(new NotificationResponse
                    {
                        MSG = "Invalid user name or password",
                        TraceId = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.UtcNow
                    });
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return BadRequest(new NotificationResponse
                {
                    MSG = "Link Expired, Resend Email confirm",
                    TraceId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow
                });
            else
                return Ok(new NotificationResponse
                {
                    MSG = "Email confirmed successfully",
                    TraceId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow
                });
        }
        [HttpGet("ResendEmailConfirmation")]

        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationRequest resendEmailConfirmationDTO) 
        {
            var user = await _userManager.FindByEmailAsync(resendEmailConfirmationDTO.EmailOrUserName) ?? await _userManager.FindByEmailAsync(resendEmailConfirmationDTO.EmailOrUserName);
            if (user is null)
                return NotFound(new NotificationResponse
                {
                    MSG = "Invalid User name or password",
                    TraceId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow
                });
            if(user.EmailConfirmed)

                return BadRequest(new NotificationResponse
                {
                    MSG = "This Email is already Confirmed",
                    TraceId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow
                });
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("ConfirmEmail", "Accounts", new { area = "Identity", token = token, userId = user.Id }, Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "Confirm Email", $"<h1>Confirm Your Email By Clicking <a href = '{link}'>Here</a></h1>");
            return Ok(new NotificationResponse
            {
                MSG = "Email Confirmation send Successful",
                TraceId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow
            });
        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest forgetPasswordDTO)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordDTO.EmailOrUserName) ?? await _userManager.FindByNameAsync(forgetPasswordDTO.EmailOrUserName);
            if(user is null)
                return NotFound(new NotificationResponse
                {
                    MSG = "Invalid User name or password",
                    TraceId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow
                });
            var OTPcode = new Random().Next(1000,9999);
            var link = Url.Action("ForgetPassword", "Account", new { area = "Identity", userId = user.Id }, Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "Reset Password", $"<h1>Reset Your Password By {OTPcode}. Don't share it </h1>");
            await _userOTP.CreateAsync(new()
            {
                ApplicationUserId = user.Id,
                OTPCode = OTPcode.ToString(),
                ExpiredTime = DateTime.UtcNow.AddDays(1)
            });
            await _userOTP.CommitAsync();
            //Create Msg
            return Ok(new 
            {
                MSG = "OTPCode Send to your Email Successful",
                userId = user.Id
            });
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordDTO)
        {
            var user = await _userManager.FindByIdAsync(resetPasswordDTO.ApplicationUserId);

            if (user is null)
                return NotFound(new NotificationResponse
                {
                    MSG = "Invalid User name or password",
                    TraceId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow
                });
            var userotp = (await _userOTP.GetAsync(e=>e.ApplicationUserId == user.Id)).OrderBy(e=>e.Id).LastOrDefault();
            if (userotp == null)
                return NotFound();
            if (userotp.OTPCode != resetPasswordDTO.OTPCode)
                return BadRequest(new NotificationResponse
                {
                    MSG = "Invalid Code",
                    TraceId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow
                });
            if (DateTime.UtcNow > userotp.ExpiredTime)
                return BadRequest(new NotificationResponse
                {
                    MSG = "Expired Time",
                    TraceId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow
                });
            return Ok(new 
            {
                MSG = "Success Code",
                userId = user.Id
            });

        }

        [HttpPost("NewPassword")]
        public async Task<IActionResult> NewPassword(NewPasswordRequest newPasswordDTO)
        {
            var user = await _userManager.FindByIdAsync(newPasswordDTO.ApplicationUserId);

            if (user is null)
                return NotFound(new NotificationResponse
                {
                    MSG = "Invalid User name or password",
                    TraceId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow
                });
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user,token, newPasswordDTO.Password);
            return Ok(new NotificationResponse
            {
                MSG = "Change Password Successfully",
                TraceId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow
            });

        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new NotificationResponse
            {
                MSG = "Logout Success",
                TraceId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow
            });
        }
    }
}
