using CarShop.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarShop.Data
{
    public class RentItem
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public ApplicationUser User { get; set; }
        public int RentSubmissionId { get; set; }
        public RentSubmission RentSubmission { get; set; }
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
        public decimal Price { get; set; }
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
        public string AdminStatus { get; set; } = "Unconfirmed";
        public string? AdminComment { get; set; }

        public RentItem()
        {

        }
    }
}
