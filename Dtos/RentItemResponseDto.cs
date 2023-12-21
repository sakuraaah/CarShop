using CarShop.Data;

namespace CarShop.Dtos
{
    public class RentItemResponseDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public UserResponseDto User { get; set; }
        public decimal Price { get; set; }
        public string ImgSrc { get; set; }
        public string AplNr { get; set; }
        public string RegNr { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int MarkId { get; set; }
        public Mark Mark { get; set; }
        public string Model { get; set; }
        public int Mileage { get; set; }
        public int Year { get; set; }
        public int BodyTypeId { get; set; }
        public BodyType BodyType { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
        public int Seats { get; set; }
        public List<Feature> Features { get; set; }
        public int RentCategoryId { get; set; }
        public RentCategory RentCategory { get; set; }
        public int CarClassId { get; set; }
        public CarClass CarClass { get; set; }
        public string Status { get; set; }
        public List<Status> AvailableStatusTransitions { get; set; }
        public string? AdminStatus { get; set; }
        public string? AdminComment { get; set; }

        public RentItemResponseDto(RentItem rentItem)
        {
            Id = rentItem.Id;
            Created = rentItem.Created;
            
            if(rentItem.User != null)
            {
                User = new UserResponseDto(rentItem.User);
            }

            Price = rentItem.Price;
            ImgSrc = rentItem.ImgSrc;
            AplNr = rentItem.AplNr;
            RegNr = rentItem.RegNr;
            CategoryId = rentItem.CategoryId;
            Category = rentItem.Category;
            MarkId = rentItem.MarkId;
            Mark = rentItem.Mark;
            Model = rentItem.Model;
            Mileage = rentItem.Mileage;
            Year = rentItem.Year;
            BodyTypeId = rentItem.BodyTypeId;
            BodyType = rentItem.BodyType;
            ColorId = rentItem.ColorId;
            Color = rentItem.Color;
            Seats = rentItem.Seats;
            Features = rentItem.Features;
            RentCategoryId = rentItem.RentCategoryId;
            RentCategory = rentItem.RentCategory;
            CarClassId = rentItem.CarClassId;
            CarClass = rentItem.CarClass;
            Status = rentItem.Status;
            AvailableStatusTransitions = rentItem.AvailableStatusTransitions;
            AdminStatus = rentItem.AdminStatus;
            AdminComment = rentItem.AdminComment;
        }
    }
}
