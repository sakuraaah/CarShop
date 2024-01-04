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
        private readonly ITransactionRepository _transactionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public BuyItemController(
            IBuyItemRepository buyItemRepository,
            IStatusRepository statusRepository,
            ICategoryRepository categoryRepository,
            IMarkRepository markRepository,
            IBodyTypeRepository bodyTypeRepository,
            IColorRepository colorRepository,
            IFeatureRepository featureRepository,
            ITransactionRepository transactionRepository,
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
            _transactionRepository = transactionRepository;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPublicList([FromQuery] BuyItemQueryDto query)
        {
            // returns public list that is identical to all users
            var response = new ApiResponseDto(() =>
            {
                var buyItems = _buyItemRepository.GetList(query, null);
                return buyItems;
            });
            return Ok(response);
        }

        [HttpGet("buyer")]
        public async Task<IActionResult> GetBuyerList([FromQuery] BuyItemQueryDto query)
        {
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            // continue if it is buyer, otherwise forbid
            if (await _userManager.IsInRoleAsync(curUser, "Buyer"))
            {
                // returns list that returns only buy items that this buyer bought
                var response = new ApiResponseDto(() =>
                {
                    var buyItems = _buyItemRepository.GetList(query, curUser, "Buyer");
                    return buyItems;
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpGet("seller")]
        public async Task<IActionResult> GetSellerList([FromQuery] BuyItemQueryDto query)
        {
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            // continue if it is seller, otherwise forbid
            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                // returns list that returns only buy items that belong to this seller
                var response = new ApiResponseDto(() =>
                {
                    var buyItems = _buyItemRepository.GetList(query, curUser, "Seller");
                    return buyItems;
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminList([FromQuery] BuyItemQueryDto query)
        {
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            // continue if it is admin, otherwise forbid
            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                // returns list that returns all non-draft buy items
                var response = new ApiResponseDto(() =>
                {
                    var buyItems = _buyItemRepository.GetList(query, null, "Admin");
                    return buyItems;
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBuyItemDto dto)
        {
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            // continue if it is seller, otherwise forbid
            if (await _userManager.IsInRoleAsync(curUser, "Seller")) 
            {
                var response = new ApiResponseDto(() =>
                {
                    // check if item with this data is already created
                    if (_buyItemRepository.Exists(dto.AplNr, dto.RegNr, null)) throw new Exception("Vehicle is already registered");

                    // check if category exists
                    var category = _categoryRepository.Get(dto.CategoryId);
                    if (category == null) throw new Exception("Category not found");

                    // check if mark exists
                    var mark = _markRepository.Get(dto.MarkId);
                    if (mark == null) throw new Exception("Mark not found");

                    // check if body type exists
                    var bodyType = _bodyTypeRepository.Get(dto.BodyTypeId);
                    if (bodyType == null) throw new Exception("Body type not found");

                    // check if color exists
                    var color = _colorRepository.Get(dto.ColorId);
                    if (color == null) throw new Exception("Color not found");

                    // default status will be 'Draft', and available status transitions for 'Draft' are 'Submitted' and 'Cancelled'
                    string[] statusNames = { "Submitted", "Cancelled" };
                    List<Status> availableStatusTransitions = _statusRepository.GetByName(statusNames).ToList();

                    // get all features from dto
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
                        EngPower = dto.EngPower,
                        Status = "Draft",
                        AvailableStatusTransitions = availableStatusTransitions,
                    };

                    // add new buy item to db
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
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);

            if (curUser == null)
            {
                // returns item if it is submitted and not blocked
                var buyItem = _buyItemRepository.GetItem(id, null, "Public");
                if (buyItem == null) return NotFound();

                var response = new ApiResponseDto(() =>
                {
                    return buyItem;
                });
                return Ok(response);
            }

            if (await _userManager.IsInRoleAsync(curUser, "Buyer"))
            {
                // returns item if it is bought by this buyer or is submitted and not blocked
                var buyItem = _buyItemRepository.GetItem(id, curUser, "Buyer");
                if (buyItem == null) return NotFound();

                var response = new ApiResponseDto(() =>
                {
                    return buyItem;
                });
                return Ok(response);
            }

            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                // returns item if it is created by this seller or is submitted and not blocked
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
                // returns item if it is not draft
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
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            // continue if it is seller, otherwise forbid
            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                var response = new ApiResponseDto(() =>
                {
                    // returns item only if it belongs to current user 
                    var buyItem = _buyItemRepository.Get(id, curUser, "Seller");
                    if (buyItem == null) throw new Exception("Buy item not found");

                    if (buyItem.Status != "Draft") throw new Exception("Only drafts can be edited");

                    // update buy item fields
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

                    // check if item with this data is already created
                    if (_buyItemRepository.Exists(buyItem.AplNr, buyItem.RegNr, buyItem.Id)) throw new Exception("Vehicle is already registered");

                    // edit selected buy item in db
                    return _buyItemRepository.Update(buyItem);
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateDto dto)
        {
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            // seller can change buy item status to 'Submitted' or 'Cancelled'
            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                var response = new ApiResponseDto(() =>
                {
                    // returns buy item only if it belongs to current user 
                    var buyItem = _buyItemRepository.Get(id, curUser, "Seller");
                    if (buyItem == null) throw new Exception("Buy item not found");

                    // check if buy item is not sold or blocked
                    if (buyItem.Status == "Sold") throw new Exception("You cannot update sold items");

                    if (buyItem.AdminStatus == "Blocked") throw new Exception("You cannot update blocked item");

                    // check if new status exists in available status transitions
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

                    // set new status and status transitions
                    buyItem.Status = newStatus.Name;
                    buyItem.AvailableStatusTransitions = availableStatusTransitions;

                    return _buyItemRepository.Update(buyItem);
                });
                return Ok(response);
            }

            // admin can change rent submission admin status to 'Confirmed' or 'Blocked'
            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var response = new ApiResponseDto(() =>
                {
                    // returns item only if it is not draft
                    var buyItem = _buyItemRepository.Get(id, null, "Admin");
                    if (buyItem == null) throw new Exception("Buy item not found");

                    // check if buy item is not sold and is submitted
                    if (buyItem.Status == "Sold") throw new Exception("You cannot update sold items");

                    if (buyItem.Status != "Submitted") throw new Exception("You can only update submitted buy items");

                    // check if new status exists and is valid
                    var newStatus = _statusRepository.GetByName(new string[] { dto.Status }).FirstOrDefault();
                    if (newStatus == null) throw new Exception("Status not found");

                    string[] adminStatusTransitions = { "Confirmed", "Blocked" };
                    if (!adminStatusTransitions.Contains(newStatus.Name)) throw new Exception("Admin status can only be set to 'Confirmed' or 'Blocked'");

                    // set new admin status and comment
                    buyItem.AdminStatus = newStatus.Name;
                    buyItem.AdminComment = dto.Comment;

                    return _buyItemRepository.Update(buyItem);
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpPost("{id}/buy")]
        public async Task<IActionResult> Buy(int id)
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
                    // returns item only if it is submitted and not blocked and not sold
                    var buyItem = _buyItemRepository.Get(id, curUser, "Buyer");
                    if (buyItem == null) throw new Exception("Buy item not found");

                    // get user balance
                    decimal curUserbalance = _transactionRepository.GetBalance(curUser);

                    // user balance must be higher or equal than buy item price
                    if (curUserbalance < buyItem.Price) throw new Exception("Insufficient funds");

                    string formattedAmount = buyItem.Price.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));
                    string vehicleTitle = buyItem.Mark.Name != "Other" ? buyItem.Mark.Name + " " + buyItem.Model : buyItem.Model;

                    // create new transaction from buyer
                    var buyerTransaction = new Transaction
                    {
                        User = curUser,
                        Amount = buyItem.Price * -1,
                        Description = "Bought " + vehicleTitle + " for " + formattedAmount,
                    };

                    // create new transaction to seller
                    var sellerTransaction = new Transaction
                    {
                        User = buyItem.User,
                        Amount = buyItem.Price,
                        Description = "Sold " + vehicleTitle + " for " + formattedAmount,
                    };

                    _transactionRepository.Create(buyerTransaction);
                    _transactionRepository.Create(sellerTransaction);

                    // remove all status transitions
                    string[] statusNames = { };
                    List<Status> availableStatusTransitions = _statusRepository.GetByName(statusNames).ToList();

                    // update buyer info and status
                    buyItem.BuyerId = curUser.Id;
                    buyItem.Status = "Sold";
                    buyItem.AvailableStatusTransitions = availableStatusTransitions;

                    return _buyItemRepository.Update(buyItem);
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            // continue if it is admin, otherwise forbid
            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var response = new ApiResponseDto(() =>
                {
                    // returns item only if it is not draft
                    var buyItem = _buyItemRepository.Get(id, null, "Admin");
                    if (buyItem == null) throw new Exception("Buy item not found");

                    // delete selected buy item
                    _buyItemRepository.Delete(buyItem);
                });
                return Ok(response);
            }

            return Forbid();
        }
    }
}
