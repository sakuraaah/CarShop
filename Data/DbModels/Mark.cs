using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarShop.Data
{
    public class Mark
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore] public List<RentSubmission> RentSubmissions { get; set; }
        [JsonIgnore] public List<RentItem> RentItems { get; set; }
        [JsonIgnore] public List<BuyItem> BuyItems { get; set; }
        public Mark()
        {

        }
    }
}
