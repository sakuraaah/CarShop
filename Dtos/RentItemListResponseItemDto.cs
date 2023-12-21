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
        public decimal Price { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string AdminStatus { get; set; }

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
            Price = rentItem.Price;
            Year = rentItem.Year;
            Mileage = rentItem.Mileage;
            AdminStatus = rentItem.AdminStatus;
        }
    }
}
