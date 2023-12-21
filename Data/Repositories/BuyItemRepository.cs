using CarShop.Dtos;
using CarShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class BuyItemRepository : Repository, IBuyItemRepository
    {
        public BuyItemRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<BuyItem> GetAll()
        {
            return _context.BuyItems;
        }

        public BuyItemListResponseDto GetList(BuyItemQueryDto query, string userRole = "Buyer")
        {
            IQueryable<BuyItem> buyItemQuery = GetAll()
                .Include(x => x.User)
                .Include(x => x.Mark);

            if (!string.IsNullOrWhiteSpace(query.Username))
            {
                buyItemQuery = buyItemQuery.Where(x => x.User.UserName.Equals(query.Username));
            }
            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                buyItemQuery = buyItemQuery.Where(x => x.Category.Name.Contains(query.Category));
            }
            if (!string.IsNullOrWhiteSpace(query.Mark))
            {
                buyItemQuery = buyItemQuery.Where(x => x.Mark.Name.Contains(query.Mark));
            }
            if (!string.IsNullOrWhiteSpace(query.Model))
            {
                buyItemQuery = buyItemQuery.Where(x => x.Model.Contains(query.Model));
            }
            if (!string.IsNullOrWhiteSpace(query.BodyType))
            {
                buyItemQuery = buyItemQuery.Where(x => x.BodyType.Name.Contains(query.BodyType));
            }
            if (!string.IsNullOrWhiteSpace(query.Color))
            {
                buyItemQuery = buyItemQuery.Where(x => x.Color.Name.Contains(query.Color));
            }
            if (query.Seats != null)
            {
                buyItemQuery = buyItemQuery.Where(x => x.Seats.Equals(query.Seats));
            }
            if (!string.IsNullOrWhiteSpace(query.FeatureList))
            {
                string[] requiredFeatures = query.FeatureList.Split(',');

                buyItemQuery = buyItemQuery.Where(
                    x => requiredFeatures.All(
                        reqFeature => x.Features.Any(feature => feature.Name.Equals(reqFeature))
                    )
                );
            }
            if (query.PriceFrom != null)
            {
                buyItemQuery = buyItemQuery.Where(x => x.Price >= query.PriceFrom);
            }
            if (query.PriceTo != null)
            {
                buyItemQuery = buyItemQuery.Where(x => x.Price <= query.PriceTo);
            }
            if (query.MileageFrom != null)
            {
                buyItemQuery = buyItemQuery.Where(x => x.Mileage >= query.MileageFrom);
            }
            if (query.MileageTo != null)
            {
                buyItemQuery = buyItemQuery.Where(x => x.Mileage <= query.MileageTo);
            }
            if (query.YearFrom != null)
            {
                buyItemQuery = buyItemQuery.Where(x => x.Year >= query.YearFrom);
            }
            if (query.YearTo != null)
            {
                buyItemQuery = buyItemQuery.Where(x => x.Year <= query.YearTo);
            }
            if (query.EngPowerFrom != null)
            {
                buyItemQuery = buyItemQuery.Where(x => x.EngPower >= query.EngPowerFrom);
            }
            if (query.EngPowerTo != null)
            {
                buyItemQuery = buyItemQuery.Where(x => x.EngPower <= query.EngPowerTo);
            }

            if (query.StartDate != null)
            {
                buyItemQuery = buyItemQuery.Where(x => x.Created >= query.StartDate);
            }
            if (query.EndDate != null)
            {
                buyItemQuery = buyItemQuery.Where(x => x.Created <= query.EndDate);
            }

            switch (userRole)
            {
                case "Buyer":
                    buyItemQuery = buyItemQuery
                        .Where(x => x.Status.Equals("Submitted"))
                        .Where(x => x.AdminStatus != "Blocked");
                    break;

                case "Seller":
                    if (!string.IsNullOrWhiteSpace(query.Status))
                    {
                        buyItemQuery = buyItemQuery.Where(x => x.Status.Equals(query.Status));
                    }
                    break;

                case "Admin":
                    if (!string.IsNullOrWhiteSpace(query.Status))
                    {
                        buyItemQuery = buyItemQuery.Where(x => x.Status.Equals(query.Status));
                    }
                    buyItemQuery = buyItemQuery.Where(x => x.Status != "Draft");
                    break;

                default:
                    break;
            }

            switch (query.orderDirection)
            {
                case "asc":
                    buyItemQuery = buyItemQuery.OrderBy(x => EF.Property<object>(x, query.orderBy));
                    break;
                case "desc":
                    buyItemQuery = buyItemQuery.OrderByDescending(x => EF.Property<object>(x, query.orderBy));
                    break;
                default:
                    // Default to ascending order if orderDirection is not specified or invalid
                    buyItemQuery = buyItemQuery.OrderBy(x => EF.Property<object>(x, query.orderBy));
                    break;
            }

            var totalCount = buyItemQuery.Count();

            var buyItems = buyItemQuery
                .Select(x => new BuyItemListResponseItemDto(x))
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToArray()
                .ToList();


            return new BuyItemListResponseDto
            {
                Total = totalCount,
                Page = query.Page,
                PageSize = query.PageSize,
                Results = buyItems
            };
        }

        public bool Exists(string aplNr, string regNr, int? id)
        {
            IQueryable<BuyItem> buyItemQuery = GetAll()
                .Where(x => x.AplNr.Equals(aplNr))
                .Where(x => x.RegNr.Equals(regNr))
                .Where(x => x.Status != "Cancelled");

            if (id != null)
            {
                buyItemQuery = buyItemQuery.Where(x => x.Id != id);
            }

            return (buyItemQuery.Count() > 0);
        }

        public BuyItemResponseDto? Create(BuyItem buyItem)
        {
            buyItem = _context.BuyItems.Add(buyItem).Entity;
            _context.SaveChanges();

            BuyItemResponseDto buyItemResponseDto = new BuyItemResponseDto(buyItem);

            return buyItemResponseDto;
        }

        public BuyItem? Get(int id, ApplicationUser? user)
        {
            IQueryable<BuyItem> buyItemQuery = GetAll()
                .Include(x => x.Category)
                .Include(x => x.Mark)
                .Include(x => x.BodyType)
                .Include(x => x.Color)
                .Include(x => x.Features)
                .Include(x => x.AvailableStatusTransitions)
                .Where(x => x.Id.Equals(id));

            if (user != null)
            {
                buyItemQuery = buyItemQuery.Where(x => x.User.Equals(user));
            }
            else
            {
                buyItemQuery = buyItemQuery.Where(x => x.Status != "Draft");
            }

            return buyItemQuery.FirstOrDefault();
        }

        public BuyItemResponseDto? GetItem(int id, ApplicationUser? user, string userRole = "Buyer")
        {
            IQueryable<BuyItem> buyItemQuery = GetAll()
                .Include(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.Mark)
                .Include(x => x.BodyType)
                .Include(x => x.Color)
                .Include(x => x.Features)
                .Include(x => x.AvailableStatusTransitions)
                .Where(x => x.Id.Equals(id));

            switch (userRole)
            {
                case "Buyer":
                    buyItemQuery = buyItemQuery
                        .Where(x => x.Status.Equals("Submitted"))
                        .Where(x => x.AdminStatus != "Blocked");
                    break;

                case "Seller":
                    buyItemQuery = buyItemQuery.Where(x => x.Status.Equals("Submitted") || x.User.Equals(user));
                    break;

                case "Admin":
                    buyItemQuery = buyItemQuery.Where(x => x.Status != "Draft");
                    break;

                default:
                    break;
            }

            return buyItemQuery
                .Select(x => new BuyItemResponseDto(x))
                .FirstOrDefault();
        }

        public BuyItemResponseDto? Update(BuyItem buyItem)
        {
            _context.BuyItems.Update(buyItem);
            _context.SaveChanges();

            BuyItemResponseDto buyItemResponseDto = new BuyItemResponseDto(buyItem);

            return buyItemResponseDto;
        }

        public void Delete(BuyItem buyItem)
        {
            _context.BuyItems.Remove(buyItem);
            _context.SaveChanges();
        }
    }
}
