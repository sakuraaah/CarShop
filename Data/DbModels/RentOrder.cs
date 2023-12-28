using CarShop.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarShop.Data
{
    public class RentOrder
    {
        [Key]
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime? EndTime { get; set; }
        public int RentItemId { get; set; }
        public RentItem RentItem { get; set; }

        public RentOrder()
        {

        }
    }
}
