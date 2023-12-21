using CarShop.Data;

namespace CarShop.Dtos
{
    public class BuyItemResponseDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public UserResponseDto User { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string ImgSrc { get; set; }
        public string AplNr { get; set; }
        public string RegNr { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int MarkId { get; set; }
        public Mark Mark { get; set; }
        public string Model { get; set; }
        public int BodyTypeId { get; set; }
        public BodyType BodyType { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
        public int Seats { get; set; }
        public List<Feature> Features { get; set; }
        public int Mileage { get; set; }
        public int Year { get; set; }
        public int EngPower { get; set; }
        public string Status { get; set; }
        public List<Status> AvailableStatusTransitions { get; set; }
        public string? AdminStatus { get; set; }
        public string? AdminComment { get; set; }

        public BuyItemResponseDto(BuyItem buyItem)
        {
            Id = buyItem.Id;
            Created = buyItem.Created;
            
            if(buyItem.User != null)
            {
                User = new UserResponseDto(buyItem.User);
            }

            Price = buyItem.Price;
            Description = buyItem.Description;
            ImgSrc = buyItem.ImgSrc;
            AplNr = buyItem.AplNr;
            RegNr = buyItem.RegNr;
            CategoryId = buyItem.CategoryId;
            Category = buyItem.Category;
            MarkId = buyItem.MarkId;
            Mark = buyItem.Mark;
            Model = buyItem.Model;
            BodyTypeId = buyItem.BodyTypeId;
            BodyType = buyItem.BodyType;
            ColorId = buyItem.ColorId;
            Color = buyItem.Color;
            Seats = buyItem.Seats;
            Features = buyItem.Features;
            Mileage = buyItem.Mileage;
            Year = buyItem.Year;
            EngPower = buyItem.EngPower;
            Status = buyItem.Status;
            AvailableStatusTransitions = buyItem.AvailableStatusTransitions;
            AdminStatus = buyItem.AdminStatus;
            AdminComment = buyItem.AdminComment;
        }
    }
}
