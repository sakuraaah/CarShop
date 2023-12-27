using CarShop.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarShop.Data
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public Transaction()
        {

        }
    }
}
