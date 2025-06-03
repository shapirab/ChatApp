using AutoMapper;
using ChatApp.data.DataModels.DTOs.Users;
using ChatApp.data.DataModels.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(SignInManager<UserEntity> signInManager, IMapper mapper) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            UserEntity user = new UserEntity
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Address = registerDto.Address
            };

            IdentityResult? result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem(ModelState);
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            UserEntity? userEntity = await signInManager.UserManager.FindByEmailAsync(loginDto.Email);
            if(userEntity == null)
            {
                return Unauthorized();
            }

            var result = await signInManager.CheckPasswordSignInAsync(userEntity, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            await signInManager.SignInAsync(userEntity, new AuthenticationProperties
            {
                IsPersistent = true,
            });

            var authCookie = Response.Headers.FirstOrDefault(h => h.Key.Contains("Set-Cookie"));
            return Ok();
        }
    }
}
