using CarShop.Data;
using System.ComponentModel.DataAnnotations;

namespace CarShop.Dtos
{
    public class CreateRentItemDto
    {
        [Required]
        public int RentSubmissionId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int BodyTypeId { get; set; }
        [Required]
        public int ColorId { get; set; }
        [Required]
        public int RentCategoryId { get; set; }
        [Required]
        public int CarClassId { get; set; }
        [Required]
        public int Seats { get; set; }
        public List<NameDto> Features { get; set; }
    }
}
