using CarShop.Data;
using CarShop.Dtos;
using CarShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarShop.Controllers
{
    [Authorize]
    [Route("api/user-data")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserDataController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            var roles = await _userManager.GetRolesAsync(curUser);
            string role = roles.Any() ? roles[0] : "";

            var response = new ApiResponseDto(() =>
            {
                var userResponse = new UserDataResponseDto(curUser, role);
                return userResponse;
            });
            return Ok(response);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetForAdmin([FromQuery] string userName)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var requestedUser = await _userManager.FindByEmailAsync(userName);

                var roles = await _userManager.GetRolesAsync(requestedUser);
                string role = roles.Any() ? roles[0] : "";

                var response = new ApiResponseDto(() =>
                {
                    if (requestedUser == null) throw new Exception("Requested user not found");

                    var userResponse = new UserDataResponseDto(requestedUser, role);
                    return userResponse;
                });

                return Ok(response);
            }

            return Forbid();
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto dto)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            var roles = await _userManager.GetRolesAsync(curUser);
            string role = roles.Any() ? roles[0] : "";

            if (!string.IsNullOrWhiteSpace(dto.FirstName))
            {
                curUser.FirstName = dto.FirstName;
            }
            if (!string.IsNullOrWhiteSpace(dto.LastName))
            {
                curUser.LastName = dto.LastName;
            }
            if (!string.IsNullOrWhiteSpace(dto.ImgSrc))
            {
                curUser.ImgSrc = dto.ImgSrc;
            }

            var result = await _userManager.UpdateAsync(curUser);

            if (!result.Succeeded) throw new Exception("User update error");

            var response = new ApiResponseDto(() =>
            {
                var userResponse = new UserDataResponseDto(curUser, role);
                return userResponse;
            });
            return Ok(response);
        }
    }
}
