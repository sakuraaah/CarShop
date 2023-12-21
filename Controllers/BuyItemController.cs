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
    [Route("api/buy-items")]
    [ApiController]
    public class BuyItemController : ControllerBase
    {
        private readonly IBuyItemRepository _buyItemRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMarkRepository _markRepository;
        private readonly IBodyTypeRepository _bodyTypeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IFeatureRepository _featureRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public BuyItemController(
            IBuyItemRepository buyItemRepository,
            IStatusRepository statusRepository,
            ICategoryRepository categoryRepository,
            IMarkRepository markRepository,
            IBodyTypeRepository bodyTypeRepository,
            IColorRepository colorRepository,
            IFeatureRepository featureRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            _buyItemRepository = buyItemRepository;
            _statusRepository = statusRepository;
            _categoryRepository = categoryRepository;
            _markRepository = markRepository;
            _bodyTypeRepository = bodyTypeRepository;
            _colorRepository = colorRepository;
            _featureRepository = featureRepository;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPublicList([FromQuery] BuyItemQueryDto query)
        {
            var response = new ApiResponseDto(() =>
            {
                var buyItems = _buyItemRepository.GetList(query);
                return buyItems;
            });
            return Ok(response);
        }

        [HttpGet("seller")]
        public async Task<IActionResult> GetSellerList([FromQuery] BuyItemQueryDto query)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                var response = new ApiResponseDto(() =>
                {
                    query.Username = curUser.UserName;
                    var buyItems = _buyItemRepository.GetList(query, "Seller");
                    return buyItems;
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminList([FromQuery] BuyItemQueryDto query)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var response = new ApiResponseDto(() =>
                {
                    var buyItems = _buyItemRepository.GetList(query, "Admin");
                    return buyItems;
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBuyItemDto dto)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Seller")) 
            {
                var response = new ApiResponseDto(() =>
                {
                    if(_buyItemRepository.Exists(dto.AplNr, dto.RegNr, null)) throw new Exception("Vehicle is already registered");

                    var category = _categoryRepository.Get(dto.CategoryId);
                    if (category == null) throw new Exception("Category not found");

                    var mark = _markRepository.Get(dto.MarkId);
                    if (mark == null) throw new Exception("Mark not found");

                    var bodyType = _bodyTypeRepository.Get(dto.BodyTypeId);
                    if (bodyType == null) throw new Exception("Body type not found");

                    var color = _colorRepository.Get(dto.ColorId);
                    if (color == null) throw new Exception("Color not found");

                    string[] statusNames = { "Submitted", "Cancelled" };
                    List<Status> availableStatusTransitions = _statusRepository.GetByName(statusNames).ToList();

                    string[] featureNames = dto.Features.ConvertAll(obj => obj.Name).ToArray();
                    List<Feature> features = _featureRepository.GetByName(featureNames).ToList();

                    var buyItem = new BuyItem
                    {
                        ImgSrc = dto.ImgSrc,
                        AplNr = dto.AplNr,
                        RegNr = dto.RegNr,
                        User = curUser,
                        Price = dto.Price,
                        Description = dto.Description,
                        CategoryId = dto.CategoryId,
                        MarkId = dto.MarkId,
                        Model = dto.Model,
                        BodyTypeId = dto.BodyTypeId,
                        ColorId = dto.ColorId,
                        Seats = dto.Seats,
                        Features = features,
                        Mileage = dto.Mileage,
                        Year = dto.Year,
                        Status = "Draft",
                        AvailableStatusTransitions = availableStatusTransitions,
                    };

                    return _buyItemRepository.Create(buyItem);
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
                var buyItem = _buyItemRepository.GetItem(id, null, "Buyer");
                if (buyItem == null) return NotFound();

                var response = new ApiResponseDto(() =>
                {
                    return buyItem;
                });
                return Ok(response);
            }

            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                var buyItem = _buyItemRepository.GetItem(id, curUser, "Seller");
                if (buyItem == null) return NotFound();

                var response = new ApiResponseDto(() =>
                {
                    return buyItem;
                });
                return Ok(response);
            }

            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var buyItem = _buyItemRepository.GetItem(id, null, "Admin");
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
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBuyItemDto dto)
        {
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                var response = new ApiResponseDto(() =>
                {
                    var buyItem = _buyItemRepository.Get(id, curUser);
                    if (buyItem == null) throw new Exception("Buy item not found");

                    if (buyItem.Status != "Draft") throw new Exception("Only drafts can be edited");

                    if (!string.IsNullOrEmpty(dto.ImgSrc))
                    {
                        buyItem.ImgSrc = dto.ImgSrc;
                    }
                    if (!string.IsNullOrEmpty(dto.AplNr))
                    {
                        buyItem.AplNr = dto.AplNr;
                    }
                    if (!string.IsNullOrEmpty(dto.RegNr))
                    {
                        buyItem.RegNr = dto.RegNr;
                    }
                    if (dto.Price.HasValue)
                    {
                        buyItem.Price = dto.Price.Value;
                    }
                    if (!string.IsNullOrEmpty(dto.Description))
                    {
                        buyItem.Description = dto.Description;
                    }
                    if (dto.CategoryId.HasValue)
                    {
                        var category = _categoryRepository.Get(dto.CategoryId.Value);
                        if (category == null) throw new Exception("Category not found");

                        buyItem.CategoryId = dto.CategoryId.Value;
                    }
                    if (dto.MarkId.HasValue)
                    {
                        var mark = _markRepository.Get(dto.MarkId.Value);
                        if (mark == null) throw new Exception("Mark not found");

                        buyItem.MarkId = dto.MarkId.Value;
                    }
                    if (!string.IsNullOrEmpty(dto.Model))
                    {
                        buyItem.Model = dto.Model;
                    }
                    if (dto.BodyTypeId.HasValue)
                    {
                        var mark = _bodyTypeRepository.Get(dto.BodyTypeId.Value);
                        if (mark == null) throw new Exception("Body type not found");

                        buyItem.BodyTypeId = dto.BodyTypeId.Value;
                    }
                    if (dto.ColorId.HasValue)
                    {
                        var mark = _colorRepository.Get(dto.ColorId.Value);
                        if (mark == null) throw new Exception("Color not found");

                        buyItem.ColorId = dto.ColorId.Value;
                    }
                    if (dto.Seats.HasValue)
                    {
                        buyItem.Seats = dto.Seats.Value;
                    }
                    if(dto.Features != null)
                    {
                        string[] featureNames = dto.Features.ConvertAll(obj => obj.Name).ToArray();
                        List<Feature> features = _featureRepository.GetByName(featureNames).ToList();

                        buyItem.Features = features;
                    }
                    if (dto.Mileage.HasValue)
                    {
                        buyItem.Mileage = dto.Mileage.Value;
                    }
                    if (dto.Year.HasValue)
                    {
                        buyItem.Year = dto.Year.Value;
                    }
                    if (dto.EngPower.HasValue)
                    {
                        buyItem.EngPower = dto.EngPower.Value;
                    }

                    if (_buyItemRepository.Exists(buyItem.AplNr, buyItem.RegNr, buyItem.Id)) throw new Exception("Vehicle is already registered");

                    return _buyItemRepository.Update(buyItem);
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
                    var buyItem = _buyItemRepository.Get(id, curUser);
                    if (buyItem == null) throw new Exception("Buy item not found");

                    if (buyItem.AdminStatus == "Blocked") throw new Exception("You cannot update blocked item");

                    var newStatus = buyItem.AvailableStatusTransitions.Find(x => x.Name.Equals(dto.Status));
                    if (newStatus == null) throw new Exception("Incorrect status transition");

                    string[] statusNames;

                    switch (newStatus.Name)
                    {
                        case "Submitted":
                            statusNames = new string[] { "Cancelled" };
                            break;
                        case "Cancelled":
                            statusNames = new string[] { };
                            buyItem.AdminStatus = null;
                            buyItem.AdminComment = null;
                            break;
                        default:
                            statusNames = new string[] { };
                            break;
                    }

                    List<Status> availableStatusTransitions = _statusRepository.GetByName(statusNames).ToList();

                    buyItem.Status = newStatus.Name;
                    buyItem.AvailableStatusTransitions = availableStatusTransitions;

                    return _buyItemRepository.Update(buyItem);
                });
                return Ok(response);
            }

            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var response = new ApiResponseDto(() =>
                {
                    var buyItem = _buyItemRepository.Get(id, null);
                    if (buyItem == null) throw new Exception("Buy item not found");

                    if (buyItem.Status != "Submitted") throw new Exception("You can only update submitted buy items");

                    var newStatus = _statusRepository.GetByName(new string[] { dto.Status }).FirstOrDefault();
                    if (newStatus == null) throw new Exception("Status not found");

                    string[] adminStatusTransitions = { "Confirmed", "Blocked" };
                    if (!adminStatusTransitions.Contains(newStatus.Name)) throw new Exception("Admin status can only be set to 'Confirmed' or 'Blocked'");

                    buyItem.AdminStatus = newStatus.Name;
                    buyItem.AdminComment = dto.Comment;

                    return _buyItemRepository.Update(buyItem);
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
                    var buyItem = _buyItemRepository.Get(id, null);
                    if (buyItem == null) throw new Exception("Buy item not found");

                    _buyItemRepository.Delete(buyItem);
                });
                return Ok(response);
            }

            return Forbid();
        }
    }
}
