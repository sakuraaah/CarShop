using System.ComponentModel.DataAnnotations;

namespace CarShop.Dtos
{
    public class CreateRentSubmissionDto
    {
        [Required]
        public string AplNr { get; set; }
        [Required]
        public string RegNr { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int MarkId { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int Mileage { get; set; }
        [Required]
        public int Year { get; set; }
    }
}
