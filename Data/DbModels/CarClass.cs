using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarShop.Data
{
    public class CarClass
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore] public List<RentItem> RentItems { get; set; }
        public CarClass()
        {

        }
    }
}
