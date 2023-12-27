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
        private readonly ITransactionRepository _transactionRepository;

        public UserDataController(
            UserManager<ApplicationUser> userManager,
            ITransactionRepository transactionRepository
        )
        {
            _userManager = userManager;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            var roles = await _userManager.GetRolesAsync(curUser);
            string role = roles.Any() ? roles[0] : "";

            decimal balance = _transactionRepository.GetBalance(curUser);

            var response = new ApiResponseDto(() =>
            {
                var userResponse = new UserDataResponseDto(curUser, role, balance);
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

                decimal balance = _transactionRepository.GetBalance(curUser);

                var response = new ApiResponseDto(() =>
                {
                    if (requestedUser == null) throw new Exception("Requested user not found");

                    var userResponse = new UserDataResponseDto(requestedUser, role, balance);
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

            decimal balance = _transactionRepository.GetBalance(curUser);

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
                var userResponse = new UserDataResponseDto(curUser, role, balance);
                return userResponse;
            });
            return Ok(response);
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            var response = new ApiResponseDto(() =>
            {
                var transactions = _transactionRepository.GetAll(curUser).ToArray();
                return transactions.ToList();
            });
            return Ok(response);
        }

        [HttpPost("add-money")]
        public async Task<IActionResult> AddMoney([FromBody] AmountDto dto)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            var response = new ApiResponseDto(() =>
            {
                if (dto.Amount <= 0) throw new Exception("Amount must be positive");

                string formattedAmount = dto.Amount.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));

                var transaction = new Transaction
                {
                    User = curUser,
                    Amount = dto.Amount,
                    Description = "Added " + formattedAmount + " to the account.",
                };

                return _transactionRepository.Create(transaction);
            });
            return Ok(response);
        }

        [HttpPost("withdraw-money")]
        public async Task<IActionResult> WithdrawMoney([FromBody] AmountDto dto)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            var response = new ApiResponseDto(() =>
            {
                if (dto.Amount <= 0) throw new Exception("Amount must be positive");

                string formattedAmount = dto.Amount.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));

                var transaction = new Transaction
                {
                    User = curUser,
                    Amount = dto.Amount * -1,
                    Description = "Withdrawn " + formattedAmount + " from the account.",
                };

                return _transactionRepository.Create(transaction);
            });
            return Ok(response);
        }
    }
}
