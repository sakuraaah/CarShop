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
    [Route("api/rent-items")]
    [ApiController]
    public class RentItemController : ControllerBase
    {
        private readonly IRentItemRepository _rentItemRepository;
        private readonly IRentSubmissionRepository _rentSubmissionRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IBodyTypeRepository _bodyTypeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ICarClassRepository _carClassRepository;
        private readonly IRentCategoryRepository _rentCategoryRepository;
        private readonly IFeatureRepository _featureRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public RentItemController(
            IRentItemRepository rentItemRepository,
            IRentSubmissionRepository rentSubmissionRepository,
            IStatusRepository statusRepository,
            IBodyTypeRepository bodyTypeRepository,
            IColorRepository colorRepository,
            ICarClassRepository carClassRepository,
            IRentCategoryRepository rentCategoryRepository,
            IFeatureRepository featureRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            _rentItemRepository = rentItemRepository;
            _rentSubmissionRepository = rentSubmissionRepository;
            _statusRepository = statusRepository;
            _bodyTypeRepository = bodyTypeRepository;
            _colorRepository = colorRepository;
            _carClassRepository = carClassRepository;
            _rentCategoryRepository = rentCategoryRepository;
            _featureRepository = featureRepository;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPublicList([FromQuery] RentItemQueryDto query)
        {
            var response = new ApiResponseDto(() =>
            {
                var rentItems = _rentItemRepository.GetList(query);
                return rentItems;
            });
            return Ok(response);
        }

        [HttpGet("seller")]
        public async Task<IActionResult> GetSellerList([FromQuery] RentItemQueryDto query)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                var response = new ApiResponseDto(() =>
                {
                    query.Username = curUser.UserName;
                    var rentItems = _rentItemRepository.GetList(query, "Seller");
                    return rentItems;
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminList([FromQuery] RentItemQueryDto query)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var response = new ApiResponseDto(() =>
                {
                    var rentItems = _rentItemRepository.GetList(query, "Admin");
                    return rentItems;
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRentItemDto dto)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Seller")) 
            {
                var response = new ApiResponseDto(() =>
                {
                    var rentSubmission = _rentSubmissionRepository.Get(dto.RentSubmissionId, curUser);
                    if (rentSubmission == null) throw new Exception("Rent submission not found");

                    if (rentSubmission.Status != "Submitted") throw new Exception("You cannot create item if rent submission is not submitted");

                    if (rentSubmission.AdminStatus != "Confirmed") throw new Exception("Your rent sumbission must be confirmed by admin before creating rent item");

                    if (_rentItemRepository.Exists(dto.RentSubmissionId, null)) throw new Exception("Vehicle is already registered");

                    var bodyType = _bodyTypeRepository.Get(dto.BodyTypeId);
                    if (bodyType == null) throw new Exception("Body type not found");

                    var color = _colorRepository.Get(dto.ColorId);
                    if (color == null) throw new Exception("Color not found");

                    var rentCategory = _rentCategoryRepository.Get(dto.RentCategoryId);
                    if (rentCategory == null) throw new Exception("Rent category not found");

                    var carClass = _carClassRepository.Get(dto.CarClassId);
                    if (carClass == null) throw new Exception("Car Class not found");

                    string[] statusNames = { "Submitted", "Cancelled" };
                    List<Status> availableStatusTransitions = _statusRepository.GetByName(statusNames).ToList();

                    string[] featureNames = dto.Features.ConvertAll(obj => obj.Name).ToArray();
                    List<Feature> features = _featureRepository.GetByName(featureNames).ToList();

                    var rentItem = new RentItem
                    {
                        RentSubmissionId = dto.RentSubmissionId,
                        User = curUser,
                        ImgSrc = rentSubmission.ImgSrc,
                        AplNr = rentSubmission.AplNr,
                        RegNr = rentSubmission.RegNr,
                        CategoryId = rentSubmission.CategoryId,
                        MarkId = rentSubmission.MarkId,
                        Model = rentSubmission.Model,
                        Mileage = rentSubmission.Mileage,
                        Year = rentSubmission.Year,
                        Price = dto.Price,
                        BodyTypeId = dto.BodyTypeId,
                        ColorId = dto.ColorId,
                        RentCategoryId = dto.RentCategoryId,
                        CarClassId = dto.CarClassId,
                        Seats = dto.Seats,
                        Features = features,
                        Status = "Draft",
                        AvailableStatusTransitions = availableStatusTransitions,
                    };

                    return _rentItemRepository.Create(rentItem);
                });
                return Ok(response);
            }

            return Forbid();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);

            if (curUser == null || await _userManager.IsInRoleAsync(curUser, "Buyer"))
            {
                var buyItem = _rentItemRepository.GetItem(id, null, "Buyer");
                if (buyItem == null) return NotFound();

                var response = new ApiResponseDto(() =>
                {
                    return buyItem;
                });
                return Ok(response);
            }

            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                var buyItem = _rentItemRepository.GetItem(id, curUser, "Seller");
                if (buyItem == null) return NotFound();

                var response = new ApiResponseDto(() =>
                {
                    return buyItem;
                });
                return Ok(response);
            }

            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var buyItem = _rentItemRepository.GetItem(id, null, "Admin");
                if (buyItem == null) return NotFound();

                var response = new ApiResponseDto(() =>
                {
                    return buyItem;
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRentItemDto dto)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                var response = new ApiResponseDto(() =>
                {
                    var rentItem = _rentItemRepository.Get(id, curUser);
                    if (rentItem == null) throw new Exception("Buy item not found");

                    if (rentItem.Status != "Draft") throw new Exception("Only drafts can be edited");

                    if (dto.Price.HasValue)
                    {
                        rentItem.Price = dto.Price.Value;
                    }
                    if (dto.BodyTypeId.HasValue)
                    {
                        var mark = _bodyTypeRepository.Get(dto.BodyTypeId.Value);
                        if (mark == null) throw new Exception("Body type not found");

                        rentItem.BodyTypeId = dto.BodyTypeId.Value;
                    }
                    if (dto.ColorId.HasValue)
                    {
                        var mark = _colorRepository.Get(dto.ColorId.Value);
                        if (mark == null) throw new Exception("Color not found");

                        rentItem.ColorId = dto.ColorId.Value;
                    }
                    if (dto.RentCategoryId.HasValue)
                    {
                        var mark = _rentCategoryRepository.Get(dto.RentCategoryId.Value);
                        if (mark == null) throw new Exception("Rent category not found");

                        rentItem.ColorId = dto.RentCategoryId.Value;
                    }
                    if (dto.CarClassId.HasValue)
                    {
                        var mark = _carClassRepository.Get(dto.CarClassId.Value);
                        if (mark == null) throw new Exception("Car class not found");

                        rentItem.ColorId = dto.CarClassId.Value;
                    }
                    if (dto.Seats.HasValue)
                    {
                        rentItem.Seats = dto.Seats.Value;
                    }
                    if (dto.Features != null)
                    {
                        string[] featureNames = dto.Features.ConvertAll(obj => obj.Name).ToArray();
                        List<Feature> features = _featureRepository.GetByName(featureNames).ToList();

                        rentItem.Features = features;
                    }

                    return _rentItemRepository.Update(rentItem);
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
                    var rentItem = _rentItemRepository.Get(id, curUser);
                    if (rentItem == null) throw new Exception("Rent item not found");

                    if (rentItem.AdminStatus == "Blocked") throw new Exception("You cannot update blocked item");

                    var newStatus = rentItem.AvailableStatusTransitions.Find(x => x.Name.Equals(dto.Status));
                    if (newStatus == null) throw new Exception("Incorrect status transition");

                    string[] statusNames;

                    switch (newStatus.Name)
                    {
                        case "Submitted":
                            statusNames = new string[] { "Cancelled" };
                            break;
                        case "Cancelled":
                            statusNames = new string[] { };
                            rentItem.AdminStatus = null;
                            rentItem.AdminComment = null;
                            break;
                        default:
                            statusNames = new string[] { };
                            break;
                    }

                    List<Status> availableStatusTransitions = _statusRepository.GetByName(statusNames).ToList();

                    rentItem.Status = newStatus.Name;
                    rentItem.AvailableStatusTransitions = availableStatusTransitions;

                    return _rentItemRepository.Update(rentItem);
                });
                return Ok(response);
            }

            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var response = new ApiResponseDto(() =>
                {
                    var rentItem = _rentItemRepository.Get(id, null);
                    if (rentItem == null) throw new Exception("Rent item not found");

                    if (rentItem.Status != "Submitted") throw new Exception("You can only update submitted rent items");

                    var newStatus = _statusRepository.GetByName(new string[] { dto.Status }).FirstOrDefault();
                    if (newStatus == null) throw new Exception("Status not found");

                    string[] adminStatusTransitions = { "Confirmed", "Blocked" };
                    if (!adminStatusTransitions.Contains(newStatus.Name)) throw new Exception("Admin status can only be set to 'Confirmed' or 'Blocked'");

                    rentItem.AdminStatus = newStatus.Name;
                    rentItem.AdminComment = dto.Comment;

                    return _rentItemRepository.Update(rentItem);
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
                    var rentItem = _rentItemRepository.Get(id, null);
                    if (rentItem == null) throw new Exception("Rent item not found");

                    _rentItemRepository.Delete(rentItem);
                });
                return Ok(response);
            }

            return Forbid();
        }
    }
}
