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
        private readonly IRentOrderRepository _rentOrderRepository;
        private readonly IRentItemRepository _rentItemRepository;

        public UserDataController(
            UserManager<ApplicationUser> userManager,
            ITransactionRepository transactionRepository,
            IRentOrderRepository rentOrderRepository,
            IRentItemRepository rentItemRepository
        )
        {
            _userManager = userManager;
            _transactionRepository = transactionRepository;
            _rentOrderRepository = rentOrderRepository;
            _rentItemRepository = rentItemRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            // get current user role
            var roles = await _userManager.GetRolesAsync(curUser);
            string role = roles.Any() ? roles[0] : "";

            // get user balance by summarizing all of his transactions
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
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                // get user from username from query
                var requestedUser = await _userManager.FindByEmailAsync(userName);

                // get requested user role
                var roles = await _userManager.GetRolesAsync(requestedUser);
                string role = roles.Any() ? roles[0] : "";

                // get requested user balance
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
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            // get current user role
            var roles = await _userManager.GetRolesAsync(curUser);
            string role = roles.Any() ? roles[0] : "";

            // get user balance
            decimal balance = _transactionRepository.GetBalance(curUser);

            // update fields, replacing them with data from dto
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
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            var response = new ApiResponseDto(() =>
            {
                // get current user transactions
                return _transactionRepository.GetList(curUser);
            });
            return Ok(response);
        }

        [HttpGet("rent-orders")]
        public async Task<IActionResult> GetRentOrders()
        {
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            var response = new ApiResponseDto(() =>
            {
                // get current user rental orders
                return _rentOrderRepository.GetList(curUser);
            });
            return Ok(response);
        }

        [HttpPost("rent-orders/{id}/finish")]
        public async Task<IActionResult> FinishRentOrder(int id)
        {
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            // continue if it is buyer, otherwise forbid
            if (await _userManager.IsInRoleAsync(curUser, "Buyer"))
            {
                var response = new ApiResponseDto(() =>
                {
                    // returns rentorder only if it belongs to current user
                    var rentOrder = _rentOrderRepository.Get(id, curUser);
                    if (rentOrder == null) throw new Exception("Rent order not found");

                    if (rentOrder.Status != "Pending") throw new Exception("Rent order is already finished");

                    // get rent item from rent order data
                    var rentItem = _rentItemRepository.Get(rentOrder.RentItemId, curUser, "Buyer");
                    if (rentItem == null) throw new Exception("Rent item not found. Please contact administrator");

                    // get total ride time in minutes
                    TimeSpan difference = DateTime.Now - rentOrder.StartTime;
                    int minutesDifference = (int)Math.Ceiling(difference.TotalMinutes);

                    // calculate price, multiplying total minutes with price per minute
                    decimal totalPrice = rentItem.Price * minutesDifference;

                    string formattedAmount = totalPrice.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));
                    string vehicleTitle = rentItem.Mark.Name != "Other" ? rentItem.Mark.Name + " " + rentItem.Model : rentItem.Model;

                    // create new transaction from buyer
                    var buyerTransaction = new Transaction
                    {
                        User = curUser,
                        Amount = totalPrice * -1,
                        Description = "Rented " + vehicleTitle + " on " + minutesDifference + " minute (-s) for " + formattedAmount,
                    };

                    // create new transaction to seller
                    var sellerTransaction = new Transaction
                    {
                        User = rentItem.User,
                        Amount = totalPrice,
                        Description = "Customer rented " + vehicleTitle + " on " + minutesDifference + " minute (-s) for " + formattedAmount,
                    };

                    _transactionRepository.Create(buyerTransaction);
                    _transactionRepository.Create(sellerTransaction);

                    // change rent item status from 'Busy' to 'Submitted', so that other buyers can rent it
                    rentItem.Status = "Submitted";
                    _rentItemRepository.Update(rentItem);

                    // change rent order status to 'Done' and add end time
                    rentOrder.Status = "Done";
                    rentOrder.EndTime = DateTime.Now;

                    return _rentOrderRepository.Update(rentOrder);
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpPost("add-money")]
        public async Task<IActionResult> AddMoney([FromBody] AmountDto dto)
        {
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            var response = new ApiResponseDto(() =>
            {
                if (dto.Amount <= 0) throw new Exception("Amount must be positive");

                string formattedAmount = dto.Amount.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));

                // add new transction with selected amount to this user
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
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            var response = new ApiResponseDto(() =>
            {
                if (dto.Amount <= 0) throw new Exception("Amount must be positive");

                decimal balance = _transactionRepository.GetBalance(curUser);

                // requested withdraw amount cannot be higher than actual user balance
                if (balance < dto.Amount) throw new Exception("Insufficient funds");

                string formattedAmount = dto.Amount.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));

                // add new transction with selected amount to this user
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
