using CarShop.Data;

namespace CarShop.Dtos
{
    public class RentSubmissionListResponseDto
    {
        public int Total { get; set; } = 0;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public List<RentSubmissionListResponseItemDto> Results { get; set; }
    }
}
