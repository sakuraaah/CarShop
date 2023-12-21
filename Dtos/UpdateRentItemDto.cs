using CarShop.Data;
using System.ComponentModel.DataAnnotations;

namespace CarShop.Dtos
{
    public class UpdateRentItemDto
    {
        public decimal? Price { get; set; }
        public int? BodyTypeId { get; set; }
        public int? ColorId { get; set; }
        public int? RentCategoryId { get; set; }
        public int? CarClassId { get; set; }
        public int? Seats { get; set; }
        public List<NameDto>? Features { get; set; }
    }
}
