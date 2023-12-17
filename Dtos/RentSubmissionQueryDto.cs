namespace CarShop.Dtos
{
    public class RentSubmissionQueryDto : DefaultQueryDto
    {
        public string? Username { get; set; }
        public string? AplNr { get; set; }
        public string? RegNr { get; set; }
        public string? Category { get; set; }
        public string? Mark { get; set; }
        public string? Model { get; set; }
        public string? Status { get; set; }
    }
}
