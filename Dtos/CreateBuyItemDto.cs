using CarShop.Data;
using System.ComponentModel.DataAnnotations;

namespace CarShop.Dtos
{

    public class CreateBuyItemDto
    {
        [Required]
        public string ImgSrc { get; set; }
        [Required]
        public string AplNr { get; set; }
        [Required]
        public string RegNr { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int MarkId { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int BodyTypeId { get; set; }
        [Required]
        public int ColorId { get; set; }
        [Required]
        public int Seats { get; set; }
        public List<NameDto> Features { get; set; }
        [Required]
        public int Mileage { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int EngPower { get; set; }
    }
}
