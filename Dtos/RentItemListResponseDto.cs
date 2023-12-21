using CarShop.Data;

namespace CarShop.Dtos
{
    public class RentItemListResponseDto
    {
        public int Total { get; set; } = 0;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public List<RentItemListResponseItemDto> Results { get; set; }
    }
}
