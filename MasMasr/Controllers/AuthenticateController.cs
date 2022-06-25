using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MasMasr.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using MasMasr.Helper;

namespace MasMasr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                if (!user.UserApproved)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Not Approved From Admin!" });
                }

                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!model.TermsAndConditions.HasValue||!model.TermsAndConditions.Value)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Terms And Conditions Must Be Checked!" });
            }

            if (model.Password!=model.ConfirmPassword)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Password And Confirm Password Not Matched!" });
            }

            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = Guid.NewGuid().ToString(),
                Address=model.Address,
                CompanyName=model.CompanyName,
                CompanyPhone=model.CompanyPhone,
                File1= FileUpload.SaveFiles(model.File1,""),
                File2= FileUpload.SaveFiles(model.File1, ""),
                File3= FileUpload.SaveFiles(model.File1, ""),
                FistName=model.FistName,
                JobTitile=model.JobTitile,
                LastName=model.LastName,
                TermsAndConditions=model.TermsAndConditions.Value,
                UserApproved=false,
                IsUser=true
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await roleManager.RoleExistsAsync(UserRoles.User))
            {
                await userManager.AddToRoleAsync(user, UserRoles.User);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Password And Confirm Password Not Matched!" });
            }

            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                UserName= Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserApproved = true,
                IsUser=false
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        [Route("approve-user/{Id}")]
        public async Task<IActionResult> ApproveUser([FromRoute] string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Not Found!" });

            user.UserApproved = true;
            userManager.UpdateAsync(user).GetAwaiter().GetResult();

            return Ok(new Response { Status = "Success", Message = "User Approved Successfully!" });
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        [Route("decline-user/{Id}")]
        public async Task<IActionResult> DeclineUser([FromRoute] string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Not Found!" });

            user.UserApproved = false;
            userManager.UpdateAsync(user).GetAwaiter().GetResult();

            return Ok(new Response { Status = "Success", Message = "User Declined Successfully!" });
        }

        [HttpPost]
        [Route("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] LoginModel model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Not Found!" });

            var token=await userManager.GeneratePasswordResetTokenAsync(userExists);

            await userManager.ResetPasswordAsync(userExists, token, model.Password);

            return Ok(new Response { Status = "Success", Message = "Password Added Successfully!" });
        }

        [Authorize(Roles = UserRoles.Admin+","+UserRoles.User)]
        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] RegisterModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Password And Confirm Password Not Matched!" });
            }

            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Not Found!" });

            var token = await userManager.GeneratePasswordResetTokenAsync(userExists);

            await userManager.ResetPasswordAsync(userExists, token, model.Password);

            return Ok(new Response { Status = "Success", Message = "Password Changed Successfully!" });
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
        [HttpGet]
        [Route("get-user/{Id}")]
        public async Task<IActionResult> GetUser([FromRoute] string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Not Found!" });

            UserResultDto userResultDto = new UserResultDto
            {
                Id = user.Id,
                Address =user.Address,
                CompanyName=user.CompanyName,
                CompanyPhone=user.CompanyPhone,
                Email=user.Email,
                File1=user.File1,
                File2=user.File2,
                File3=user.File3,
                FistName=user.FistName,
                JobTitile=user.JobTitile,
                LastName=user.LastName,
                TermsAndConditions=user.TermsAndConditions,
                UserApproved=user.UserApproved
            };

            return Ok(new Response { Status = "Success", Data= userResultDto });
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        [Route("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users =  userManager.Users.Where(x=>x.IsUser).ToList();
            if (users == null || users.Count()<1)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "No Users For Now!" });

            List<UserResultDto> userResultDtos = new List<UserResultDto>();
            foreach (var user in users)
            {
                UserResultDto userResultDto = new UserResultDto
                {
                    Id=user.Id,
                    Address = user.Address,
                    CompanyName = user.CompanyName,
                    CompanyPhone = user.CompanyPhone,
                    Email = user.Email,
                    File1 = user.File1,
                    File2 = user.File2,
                    File3 = user.File3,
                    FistName = user.FistName,
                    JobTitile = user.JobTitile,
                    LastName = user.LastName,
                    TermsAndConditions = user.TermsAndConditions,
                    UserApproved = user.UserApproved
                };
                userResultDtos.Add(userResultDto);
            }

            return Ok(new Response { Status = "Success", Data = userResultDtos });
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete]
        [Route("delete-user/{Id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Not Found!" });

            await userManager.DeleteAsync(user);

            return Ok(new Response { Status = "Success", Message="User Deleted Successfully" });
        }
    }
}
