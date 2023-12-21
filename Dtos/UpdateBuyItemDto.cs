using CarShop.Data;
using System.ComponentModel.DataAnnotations;

namespace CarShop.Dtos
{

    public class UpdateBuyItemDto
    {
        public string? ImgSrc { get; set; }
        public string? AplNr { get; set; }
        public string? RegNr { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public int? MarkId { get; set; }
        public string? Model { get; set; }
        public int? BodyTypeId { get; set; }
        public int? ColorId { get; set; }
        public int? Seats { get; set; }
        public List<NameDto>? Features { get; set; }
        public int? Mileage { get; set; }
        public int? Year { get; set; }
        public int? EngPower { get; set; }
    }
}
