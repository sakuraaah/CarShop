﻿using System.ComponentModel.DataAnnotations;

namespace CarShop.Data
{
    public class BodyType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public BodyType()
        {

        }
    }
}