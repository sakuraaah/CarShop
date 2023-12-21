using CarShop.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarShop.Data
{
    public class RentSubmission
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public ApplicationUser User { get; set; }
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
        public string Status { get; set; }
        public List<Status> AvailableStatusTransitions { get; set; }
        public string? AdminStatus { get; set; }
        public string? AdminComment { get; set; }
        [JsonIgnore] public RentItem RentItem { get; set; }

        public RentSubmission()
        {

        }
    }
}
