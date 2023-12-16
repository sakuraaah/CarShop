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
    [Route("api/rent-submissions")]
    [ApiController]
    public class RentSubmissionController : ControllerBase
    {
        private readonly IRentSubmissionRepository _rentSubmissionRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMarkRepository _markRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public RentSubmissionController(
            IRentSubmissionRepository rentSubmissionRepository,
            IStatusRepository statusRepository,
            ICategoryRepository categoryRepository,
            IMarkRepository markRepository, 
            UserManager<ApplicationUser> userManager
            )
        {
            _rentSubmissionRepository = rentSubmissionRepository;
            _statusRepository = statusRepository;
            _categoryRepository = categoryRepository;
            _markRepository = markRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var response = new ApiResponseDto(() =>
                {
                    var rentSubmissions = _rentSubmissionRepository.GetAdminList().ToArray();
                    return rentSubmissions.ToList();
                });
                return Ok(response);
            }

            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                var response = new ApiResponseDto(() =>
                {
                    var rentSubmissions = _rentSubmissionRepository.GetSellerList(curUser).ToArray();
                    return rentSubmissions.ToList();
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRentSubmissionDto dto)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Seller")) 
            {
                var response = new ApiResponseDto(() =>
                {
                    var category = _categoryRepository.Get(dto.CategoryId);
                    if (category == null) throw new Exception("Category not found");

                    var mark = _markRepository.Get(dto.MarkId);
                    if (mark == null) throw new Exception("Mark not found");

                    string[] statusNames = { "Submitted", "Cancelled" };
                    List<Status> availableStatusTransitions = _statusRepository.GetByName(statusNames).ToList();

                    var rentSubmission = new RentSubmission
                    {
                        AplNr = dto.AplNr,
                        RegNr = dto.RegNr,
                        User = curUser,
                        CategoryId = dto.CategoryId,
                        MarkId = dto.MarkId,
                        Model = dto.Model,
                        Mileage = dto.Mileage,
                        Year = dto.Year,
                        Status = "Draft",
                        AvailableStatusTransitions = availableStatusTransitions,
                    };

                    return _rentSubmissionRepository.Create(rentSubmission);
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                var rentSubmission = _rentSubmissionRepository.GetForSeller(id, curUser);
                if (rentSubmission == null) return NotFound();

                var response = new ApiResponseDto(() =>
                {
                    return rentSubmission;
                });
                return Ok(response);
            }

            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var rentSubmission = _rentSubmissionRepository.GetForAdmin(id);
                if (rentSubmission == null) return NotFound();

                var response = new ApiResponseDto(() =>
                {
                    return rentSubmission;
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRentSubmissionDto dto)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                var response = new ApiResponseDto(() =>
                {
                    var rentSubmission = _rentSubmissionRepository.GetForSeller(id, curUser);
                    if (rentSubmission == null) throw new Exception("Rent Submission not found");

                    if (!string.IsNullOrEmpty(dto.AplNr))
                    {
                        rentSubmission.AplNr = dto.AplNr;
                    }
                    if (!string.IsNullOrEmpty(dto.RegNr))
                    {
                        rentSubmission.RegNr = dto.RegNr;
                    }
                    if (dto.CategoryId.HasValue)
                    {
                        var category = _categoryRepository.Get(dto.CategoryId.Value);
                        if (category == null) throw new Exception("Category not found");

                        rentSubmission.CategoryId = dto.CategoryId.Value;
                    }
                    if (dto.MarkId.HasValue)
                    {
                        var mark = _markRepository.Get(dto.MarkId.Value);
                        if (mark == null) throw new Exception("Mark not found");

                        rentSubmission.MarkId = dto.MarkId.Value;
                    }
                    if (!string.IsNullOrEmpty(dto.Model))
                    {
                        rentSubmission.Model = dto.Model;
                    }
                    if (dto.Mileage.HasValue)
                    {
                        rentSubmission.Mileage = dto.Mileage.Value;
                    }
                    if (dto.Year.HasValue)
                    {
                        rentSubmission.Year = dto.Year.Value;
                    }

                    return _rentSubmissionRepository.Update(rentSubmission);
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var response = new ApiResponseDto(() =>
                {
                    var rentSubmission = _rentSubmissionRepository.GetForAdmin(id);
                    if (rentSubmission == null) throw new Exception("Rent Submission not found");

                    _rentSubmissionRepository.Delete(rentSubmission);
                });
                return Ok(response);
            }

            return Forbid();
        }
    }
}
