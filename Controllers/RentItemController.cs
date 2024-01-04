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
        private readonly ITransactionRepository _transactionRepository;
        private readonly IRentOrderRepository _rentOrderRepository;
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
            ITransactionRepository transactionRepository,
            IRentOrderRepository rentOrderRepository,
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
            _transactionRepository = transactionRepository;
            _rentOrderRepository = rentOrderRepository;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPublicList([FromQuery] RentItemQueryDto query)
        {
            // returns public list that is identical to all users
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
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            // continue if it is seller, otherwise forbid
            if (await _userManager.IsInRoleAsync(curUser, "Seller"))
            {
                // returns list that returns only rent items that belong to this seller
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
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            // continue if it is admin, otherwise forbid
            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                // returns list that returns all non-draft rent items
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
            // get current user
            var currentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _userManager.FindByIdAsync(currentUserID);
            if (curUser == null) throw new Exception("User not found");

            // continue if it is seller, otherwise forbid
            if (await _userManager.IsInRoleAsync(curUser, "Seller")) 
            {
                var response = new ApiResponseDto(() =>
                {
                    // get rent submission that will be used to create rent item
                    // rent submisison must belong to this seller
                    var rentSubmission = _rentSubmissionRepository.Get(dto.RentSubmissionId, curUser);
                    if (rentSubmission == null) throw new Exception("Rent submission not found");

                    // rent submisison must be submitted and confirmed by administartor
                    if (rentSubmission.Status != "Submitted") throw new Exception("You cannot create item if rent submission is not submitted");

                    if (rentSubmission.AdminStatus != "Confirmed") throw new Exception("Your rent sumbission must be confirmed by admin before creating rent item");

                    // check if item with this data is already created
                    if (_rentItemRepository.Exists(dto.RentSubmissionId, null)) throw new Exception("Vehicle is already registered");

                    // check if body type exists
                    var bodyType = _bodyTypeRepository.Get(dto.BodyTypeId);
                    if (bodyType == null) throw new Exception("Body type not found");

                    // check if color exists
                    var color = _colorRepository.Get(dto.ColorId);
                    if (color == null) throw new Exception("Color not found");

                    // check if rent category exists
                    var rentCategory = _rentCategoryRepository.Get(dto.RentCategoryId);
                    if (rentCategory == null) throw new Exception("Rent category not found");

                    // check if car class exists
                    var carClass = _carClassRepository.Get(dto.CarClassId);
                    if (carClass == null) throw new Exception("Car Class not found");

                    // default status will be 'Draft', and available status transitions for 'Draft' are 'Submitted' and 'Cancelled'
                    string[] statusNames = { "Submitted", "Cancelled" };
                    List<Status> availableStatusTransitions = _statusRepository.GetByName(statusNames).ToList();

                    // get all features from dto
                    string[] featureNames = dto.Features.ConvertAll(obj => obj.Name).ToArray();
                    List<Feature> features = _featureRepository.GetByName(featureNames).ToList();

                    // create new rent item using data from rent submission and data from dto
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

                    // add new rent item to db
                    var createdRentItem = _rentItemRepository.Create(rentItem);

                    // change used rent submisison status to 'Used' and remove all available ststus transitions
                    string[] rentSubmissionStatusNames = { };
                    List<Status> rentSubmissionAvailableStatusTransitions = _statusRepository.GetByName(rentSubmissionStatusNames).ToList();

                    rentSubmission.Status = "Used";
                    rentSubmission.AvailableStatusTransitions = rentSubmissionAvailableStatusTransitions;

                    _rentSubmissionRepository.Update(rentSubmission);

                    return createdRentItem;
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

            if (curUser == null || await _userManager.IsInRoleAsync(curUser, "Buyer"))
            {
                // returns item if it is submitted or busy and not blocked
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
                // returns item if it is created by this seller or is submitted or busy and not blocked
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
                // returns item if it is not draft
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
                    var rentItem = _rentItemRepository.Get(id, curUser, "Seller");
                    if (rentItem == null) throw new Exception("Buy item not found");

                    if (rentItem.Status != "Draft") throw new Exception("Only drafts can be edited");

                    // update rent item fields
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

                    // edit selected rent item in db
                    return _rentItemRepository.Update(rentItem);
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
                    // returns rent item only if it belongs to current user 
                    var rentItem = _rentItemRepository.Get(id, curUser, "Seller");
                    if (rentItem == null) throw new Exception("Rent item not found");

                    // check if rent item is not busy or blocked
                    if (rentItem.Status == "Busy" || (rentItem.BusyTill != null && rentItem.BusyTill >= DateTime.Now )) throw new Exception("You cannot update items that are currently in use");
                    
                    if (rentItem.AdminStatus == "Blocked") throw new Exception("You cannot update blocked item");

                    // check if new status exists in available status transitions
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

                    // set new status and status transitions
                    rentItem.Status = newStatus.Name;
                    rentItem.AvailableStatusTransitions = availableStatusTransitions;

                    return _rentItemRepository.Update(rentItem);
                });
                return Ok(response);
            }

            // admin can change rent submission admin status to 'Confirmed' or 'Blocked'
            if (await _userManager.IsInRoleAsync(curUser, "Admin"))
            {
                var response = new ApiResponseDto(() =>
                {
                    // returns item only if it is not draft
                    var rentItem = _rentItemRepository.Get(id, null, "Admin");
                    if (rentItem == null) throw new Exception("Rent item not found");

                    // check if rent item is not busy and is submitted
                    if (rentItem.Status == "Busy" || (rentItem.BusyTill != null && rentItem.BusyTill >= DateTime.Now )) throw new Exception("You cannot update items that are currently in use");

                    if (rentItem.Status != "Submitted") throw new Exception("You can only update submitted rent items");

                    // check if new status exists and is valid
                    var newStatus = _statusRepository.GetByName(new string[] { dto.Status }).FirstOrDefault();
                    if (newStatus == null) throw new Exception("Status not found");

                    string[] adminStatusTransitions = { "Confirmed", "Blocked" };
                    if (!adminStatusTransitions.Contains(newStatus.Name)) throw new Exception("Admin status can only be set to 'Confirmed' or 'Blocked'");

                    // set new admin status and comment
                    rentItem.AdminStatus = newStatus.Name;
                    rentItem.AdminComment = dto.Comment;

                    return _rentItemRepository.Update(rentItem);
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpPost("{id}/rent-carsharing")]
        public async Task<IActionResult> RentCarsharing(int id)
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
                    // returns item only if it is submitted and not blocked
                    var rentItem = _rentItemRepository.Get(id, null, "Buyer");
                    if (rentItem == null) throw new Exception("Rent item not found");

                    // check that item is not busy and is of the right rental category
                    if (rentItem.Status == "Busy" || (rentItem.BusyTill != null && rentItem.BusyTill >= DateTime.Now)) throw new Exception("This rental is currently in use");

                    if (rentItem.RentCategory.Name != "Carsharing") throw new Exception("Wrong rental category. Vehicle must be of type: Carsharing");

                    // get user balance
                    decimal curUserbalance = _transactionRepository.GetBalance(curUser);

                    // user must be able to cover at least 30min of the ride
                    if (curUserbalance < (rentItem.Price * 30)) throw new Exception("Insufficient funds. Your balance must be able to cover at least 30min of the ride");

                    // change item status to busy
                    rentItem.Status = "Busy";
                    _rentItemRepository.Update(rentItem);

                    // create new pending rent order
                    var carsharingRentOrder = new RentOrder
                    {
                        User = curUser,
                        Status = "Pending",
                        RentItemId = rentItem.Id
                    };

                    return _rentOrderRepository.Create(carsharingRentOrder);
                });
                return Ok(response);
            }

            return Forbid();
        }

        [HttpPost("{id}/rent-daily")]
        public async Task<IActionResult> RentDaily(int id, [FromBody] RentDurationDto dto)
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
                    // returns item only if it is submitted and not blocked
                    var rentItem = _rentItemRepository.Get(id, null, "Buyer");
                    if (rentItem == null) throw new Exception("Rent item not found");

                    // check that item is not busy and is of the right rental category
                    if (rentItem.Status == "Busy" || (rentItem.BusyTill != null && rentItem.BusyTill >= DateTime.Now)) throw new Exception("This rental is currently in use");

                    if (rentItem.RentCategory.Name != "Daily") throw new Exception("Wrong rental category. Vehicle must be of type: Daily");

                    // get user balance
                    decimal curUserbalance = _transactionRepository.GetBalance(curUser);

                    if (dto == null) throw new Exception("Please provide rent due date");

                    DateTime endDate = dto.EndDate.AddDays(1);

                    if (endDate < DateTime.Now) throw new Exception("Rent ending time must be in the future");

                    // calculate total rent price, multiplying days by rental price per day
                    TimeSpan difference = endDate - DateTime.Now;
                    int daysDifference = (int)Math.Ceiling(difference.TotalDays);

                    decimal totalPrice = rentItem.Price * daysDifference;

                    // user balance must be higher or equal than rent item price
                    if (curUserbalance < totalPrice) throw new Exception("Insufficient funds");

                    string formattedAmount = totalPrice.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));
                    string vehicleTitle = rentItem.Mark.Name != "Other" ? rentItem.Mark.Name + " " + rentItem.Model : rentItem.Model;

                    // create new transaction from buyer
                    var buyerTransaction = new Transaction
                    {
                        User = curUser,
                        Amount = totalPrice * -1,
                        Description = "Rented " + vehicleTitle + " on " + daysDifference + " day (-s) for " + formattedAmount,
                    };

                    // create new transaction to seller
                    var sellerTransaction = new Transaction
                    {
                        User = rentItem.User,
                        Amount = totalPrice,
                        Description = "Customer rented " + vehicleTitle + " on " + daysDifference + " day (-s) for " + formattedAmount,
                    };

                    _transactionRepository.Create(buyerTransaction);
                    _transactionRepository.Create(sellerTransaction);

                    rentItem.BusyTill = endDate;
                    _rentItemRepository.Update(rentItem);

                    // create new finished rent order
                    var rentOrder = new RentOrder
                    {
                        User = curUser,
                        Status = "Done",
                        EndTime = endDate,
                        RentItemId = rentItem.Id
                    };

                    return _rentOrderRepository.Create(rentOrder);
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
                    var rentItem = _rentItemRepository.Get(id, null, "Admin");
                    if (rentItem == null) throw new Exception("Rent item not found");

                    // delete selected rent item
                    _rentItemRepository.Delete(rentItem);
                });
                return Ok(response);
            }

            return Forbid();
        }
    }
}
