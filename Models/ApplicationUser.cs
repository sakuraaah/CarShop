using CarShop.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CarShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(max)")]
        public string ImgSrc { get; set; }

        [JsonIgnore] public List<RentSubmission> RentSubmissions { get; set; }
        [JsonIgnore] public List<RentItem> RentItems { get; set; }
        [JsonIgnore] public List<BuyItem> BuyItems { get; set; }
    }
}