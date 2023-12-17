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
        public async Task<IActionResult> GetAll([FromQuery] RentSubmissionQueryDto query)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var response = new ApiResponseDto(() =>
                {
                    var rentSubmissions = _rentSubmissionRepository.GetList(query, true);
                    return rentSubmissions;
                });
                return Ok(response);
            }

            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                var response = new ApiResponseDto(() =>
                {
                    query.Username = curUser.UserName;
                    var rentSubmissions = _rentSubmissionRepository.GetList(query);
                    return rentSubmissions;
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
                    if(_rentSubmissionRepository.Exists(dto.AplNr, dto.RegNr, null)) throw new Exception("Vehicle is already registered");

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
                var rentSubmission = _rentSubmissionRepository.GetItem(id, curUser);
                if (rentSubmission == null) return NotFound();

                var response = new ApiResponseDto(() =>
                {
                    return rentSubmission;
                });
                return Ok(response);
            }

            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var rentSubmission = _rentSubmissionRepository.GetItem(id, null);
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
                    var rentSubmission = _rentSubmissionRepository.Get(id, curUser);
                    if (rentSubmission == null) throw new Exception("Rent Submission not found");

                    if (rentSubmission.Status != "Draft") throw new Exception("Only drafts can be edited");

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

                    if (_rentSubmissionRepository.Exists(rentSubmission.AplNr, rentSubmission.RegNr, rentSubmission.Id)) throw new Exception("Vehicle is already registered");

                    return _rentSubmissionRepository.Update(rentSubmission);
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateDto dto)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                var response = new ApiResponseDto(() =>
                {
                    var rentSubmission = _rentSubmissionRepository.Get(id, curUser);
                    if (rentSubmission == null) throw new Exception("Rent Submission not found");

                    var newStatus = rentSubmission.AvailableStatusTransitions.Find(x => x.Name.Equals(dto.Status));
                    if (newStatus == null) throw new Exception("Incorrect status transition");

                    string[] statusNames;

                    switch (newStatus.Name)
                    {
                        case "Submitted":
                            statusNames = new string[] { "Cancelled" };
                            rentSubmission.AdminStatus = "Processing";
                            break;
                        default:
                            statusNames = new string[] { };
                            break;
                    }

                    List<Status> availableStatusTransitions = _statusRepository.GetByName(statusNames).ToList();

                    rentSubmission.Status = newStatus.Name;
                    rentSubmission.AvailableStatusTransitions = availableStatusTransitions;

                    return _rentSubmissionRepository.Update(rentSubmission);
                });
                return Ok(response);
            }

            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var response = new ApiResponseDto(() =>
                {
                    var rentSubmission = _rentSubmissionRepository.Get(id, null);
                    if (rentSubmission == null) throw new Exception("Rent Submission not found");

                    var newStatus = _statusRepository.GetByName(new string[] { dto.Status }).FirstOrDefault();
                    if (newStatus == null) throw new Exception("Status not found");

                    rentSubmission.AdminStatus = newStatus.Name;
                    rentSubmission.AdminComment = dto.Comment;

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
                    var rentSubmission = _rentSubmissionRepository.Get(id, null);
                    if (rentSubmission == null) throw new Exception("Rent Submission not found");

                    _rentSubmissionRepository.Delete(rentSubmission);
                });
                return Ok(response);
            }

            return Forbid();
        }
    }
}
