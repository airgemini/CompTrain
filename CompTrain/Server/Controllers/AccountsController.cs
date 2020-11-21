using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CompTrain.Server.Interfaces;
using CompTrain.Shared.Models.Account;
using CompTrain.Shared.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CompTrain.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<Athlete> _userManager;
        private readonly ILogger<AccountsController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<Athlete> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountsController(ILogger<AccountsController> logger, UserManager<Athlete> userManager, IEmailSender emailSender, SignInManager<Athlete> signInManager, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest registerRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new Athlete
                    {
                        UserName = registerRequest.Email,
                        Email = registerRequest.Email,
                        LastName = registerRequest.LastName,
                        FirstName = registerRequest.FirstName,
                        Birthday = registerRequest.Birthday.Value,
                        Sex = Convert.ToChar(registerRequest.Sex)
                    };

                    var result = await _userManager.CreateAsync(user, registerRequest.Password);
                    if (!result.Succeeded)
                    {
                        RegisterResponse registerResponse = new RegisterResponse
                        {
                            IsSuccess = false,
                            EmailExist = result.Errors.FirstOrDefault(x => x.Code.Equals("DuplicateUserName")) != null,
                            Errors = result.Errors.Select(x => x.Description),
                        };
                        return Ok(registerResponse);
                    }

                    _logger.LogInformation("User created a new account with password.");

                    
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var urlConfirmation = $"{Request.Scheme}://{Request.Host}/account/emailconfirmation/?userid={HttpUtility.UrlEncode(user.Id)}&code={HttpUtility.UrlEncode(code)}";
                    await _emailSender.SendMail(user.Email, "Email confirmation", $"Please confirm your account by <a href='{urlConfirmation}'>clicking here</a>");
                    
                    return Ok(new RegisterResponse { IsSuccess = true });
                }
                else
                {
                    return Ok(new RegisterResponse { IsSuccess = false, Errors = new List<string> { "modello non valido" } });
                }
            } catch (Exception ex)
            {
                _logger.LogError($"User register error: {ex.Message}");
                return Ok(new RegisterResponse { IsSuccess = false, Errors = new List<string> { ex.Message } });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> EmailConfirmation([FromBody]EmailConfirmationRequest confirmationRequest)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(confirmationRequest.UserId);
                if (user == null)
                    throw new Exception("User not found");

                var result = await _userManager.ConfirmEmailAsync(user, confirmationRequest.Code);
                return Ok(result.Succeeded);
            } catch(Exception ex)
            {
                _logger.LogError($"Email confirmation error: {ex.Message} - UserID: {confirmationRequest.UserId} - Code: {confirmationRequest.Code}");
                return Ok(false);
            }
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordRequest forgotPasswordRequest)
        {
            ForgotPasswordResponse response = new ForgotPasswordResponse();
            try
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordRequest.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    response.Errors = new List<string> { "User not found or not confirmed email" };
                }
                else
                {
                    
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var urlConfirmation = $"{Request.Scheme}://{Request.Host}/account/changepassword/?code={HttpUtility.UrlEncode(code)}";
                    await _emailSender.SendMail(user.Email, "Reset password", $"Please reset your password by <a href='{urlConfirmation}'>clicking here</a>");
                    response.IsSuccess = true;
                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Forgot password error: {ex.Message} - Email: {forgotPasswordRequest.Email}");
                response.Errors = new List<string> { ex.Message };
                return Ok(response);
            }
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordRequest changePasswordRequest)
        {
            ChangePasswordResponse response = new ChangePasswordResponse();
            try
            {
                var user = await _userManager.FindByEmailAsync(changePasswordRequest.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    response.Errors = new List<string> { "User not found or not confirmed email" };
                }
                else
                {
                    var result = await _userManager.ResetPasswordAsync(user, changePasswordRequest.Code, changePasswordRequest.Password);
                    if (result.Succeeded)
                    {
                        response.IsSuccess = true;
                    } else
                    {
                        response.Errors = result.Errors.Select(x => x.Description);
                    }
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Forgot password error: {ex.Message} - Email: {changePasswordRequest.Email}");
                response.Errors = new List<string> { ex.Message };
                return Ok(response);
            }
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            LoginResponse response = new LoginResponse();
            try {
                var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, false);

                if (!result.Succeeded)
                {
                    response.Errors = new List<string> { "Username and password are invalid." };
                    return Ok(response);
                }

                
                var user = await _signInManager.UserManager.FindByEmailAsync(loginRequest.Email);
                var roles = await _signInManager.UserManager.GetRolesAsync(user);

                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Name, loginRequest.Email));

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["Jwt:ExpiryInDays"]));

                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: expiry,
                    signingCredentials: creds
                );
                response.Token = new JwtSecurityTokenHandler().WriteToken(token);
                response.IsSuccess = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login error: {ex.Message} - Email: {loginRequest.Email}");
                response.Errors = new List<string> { ex.Message };
                return Ok(response);
            }
        }

    }
}
