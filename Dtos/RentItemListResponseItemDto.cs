using CarShop.Data;

namespace CarShop.Dtos
{
    public class RentItemListResponseItemDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public UserResponseDto User { get; set; }
        public string ImgSrc { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public string CarClass { get; set; }
        public string RentCategory { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public string? AdminStatus { get; set; }

        public RentItemListResponseItemDto(RentItem rentItem)
        {
            Id = rentItem.Id;
            Created = rentItem.Created;
            
            if(rentItem.User != null)
            {
                User = new UserResponseDto(rentItem.User);
            }

            ImgSrc = rentItem.ImgSrc;
            Mark = rentItem.Mark.Name;
            Model = rentItem.Model;
            CarClass = rentItem.CarClass.Name;
            RentCategory = rentItem.RentCategory.Name;
            Price = rentItem.Price;
            Status = rentItem.Status;
            if (!string.IsNullOrWhiteSpace(rentItem.AdminStatus))
            {
                AdminStatus = rentItem.AdminStatus;
            }
        }
    }
}
