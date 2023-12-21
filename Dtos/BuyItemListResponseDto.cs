﻿using CarShop.Data;

namespace CarShop.Dtos
{
    public class BuyItemListResponseDto
    {
        public int Total { get; set; } = 0;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public List<BuyItemListResponseItemDto> Results { get; set; }
    }
}
