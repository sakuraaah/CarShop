using CarShop.Data;

namespace CarShop.Dtos
{
    public class RentSubmissionResponseDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public UserResponseDto User { get; set; }
        public string ImgSrc { get; set; }
        public string AplNr { get; set; }
        public string RegNr { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int MarkId { get; set; }
        public Mark Mark { get; set; }
        public string Model { get; set; }
        public int Mileage { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
        public List<Status> AvailableStatusTransitions { get; set; }
        public string? AdminStatus { get; set; }
        public string? AdminComment { get; set; }

        public RentSubmissionResponseDto(RentSubmission rentsubmission)
        {
            Id = rentsubmission.Id;
            Created = rentsubmission.Created;
            
            if(rentsubmission.User != null)
            {
                User = new UserResponseDto(rentsubmission.User);
            }

            ImgSrc = rentsubmission.ImgSrc;
            AplNr = rentsubmission.AplNr;
            RegNr = rentsubmission.RegNr;
            CategoryId = rentsubmission.CategoryId;
            Category = rentsubmission.Category;
            MarkId = rentsubmission.MarkId;
            Mark = rentsubmission.Mark;
            Model = rentsubmission.Model;
            Mileage = rentsubmission.Mileage;
            Year = rentsubmission.Year;
            Status = rentsubmission.Status;
            AvailableStatusTransitions = rentsubmission.AvailableStatusTransitions;
            AdminStatus = rentsubmission.AdminStatus;
            AdminComment = rentsubmission.AdminComment;
        }
    }
}
