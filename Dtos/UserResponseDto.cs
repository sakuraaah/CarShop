﻿using CarShop.Models;

namespace CarShop.Dtos
{
    public class UserResponseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ImgSrc { get; set; }

        public UserResponseDto(ApplicationUser user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            ImgSrc = user.ImgSrc;
        }
    }
}
