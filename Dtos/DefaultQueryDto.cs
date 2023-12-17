namespace CarShop.Dtos
{
    public class DefaultQueryDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string orderBy { get; set; } = "Created";
        public string orderDirection { get; set; } = "asc";
    }
}
