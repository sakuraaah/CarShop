using System.ComponentModel.DataAnnotations;

namespace CarShop.Data
{
    public class RentSubmission
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string AplNr { get; set; }
        public string RegNr { get; set; }
        public int CategoryId { get; set; }
        public int MarkId { get; set; }
        public string Model { get; set; }
        public int Mileage { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
        public List<Status> AvailableStatusTransitions { get; set; }
        public string? AdminStatus { get; set; }
        public string? AdminComment { get; set; }

        public RentSubmission()
        {

        }
    }
}
