using CarShop.Data;

namespace CarShop.Dtos
{
    public class BuyItemListResponseItemDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public UserResponseDto User { get; set; }
        public string ImgSrc { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }

        public BuyItemListResponseItemDto(BuyItem buyItem)
        {
            Id = buyItem.Id;
            Created = buyItem.Created;
            
            if(buyItem.User != null)
            {
                User = new UserResponseDto(buyItem.User);
            }

            ImgSrc = buyItem.ImgSrc;
            Mark = buyItem.Mark.Name;
            Model = buyItem.Model;
            Price = buyItem.Price;
            Year = buyItem.Year;
            Mileage = buyItem.Mileage;
        }
    }
}
