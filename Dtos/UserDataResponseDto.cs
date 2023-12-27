using CarShop.Models;

namespace CarShop.Dtos
{
    public class UserDataResponseDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string ImgSrc { get; set; }
        public decimal Balance { get; set; }

        public UserDataResponseDto(ApplicationUser user, string role, decimal balance)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            ImgSrc = user.ImgSrc;
            Balance = balance;
            Role = role;
        }
    }
}
