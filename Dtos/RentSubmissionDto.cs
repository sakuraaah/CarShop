namespace CarShop.Dtos
{
    public class RentSubmissionDto
    {
        public string AplNr { get; set; }
        public string RegNr { get; set; }
        public int CategoryId { get; set; }
        public int MarkId { get; set; }
        public string Model { get; set; }
        public int Mileage { get; set; }
        public int Year { get; set; }
    }
}
