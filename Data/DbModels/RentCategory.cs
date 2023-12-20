using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarShop.Data
{
    public class RentCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public RentCategory()
        {

        }
    }
}
