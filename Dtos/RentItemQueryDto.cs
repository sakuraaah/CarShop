namespace CarShop.Dtos
{
    public class RentItemQueryDto : DefaultQueryDto
    {
        public string? Username { get; set; }
        public string? Category { get; set; }
        public string? Mark { get; set; }
        public string? Model { get; set; }
        public string? BodyType { get; set; }
        public string? Color { get; set; }
        public int? Seats { get; set; }
        public string? FeatureList { get; set; }
        public string? RentCategory { get; set; }
        public string? CarClass { get; set; }
        public int? PriceFrom { get; set; }
        public int? PriceTo { get; set; }
        public int? MileageFrom { get; set; }
        public int? MileageTo { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
        public string? Status { get; set; }
    }
}
