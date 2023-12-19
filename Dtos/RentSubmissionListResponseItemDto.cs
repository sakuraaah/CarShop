using CarShop.Data;

namespace CarShop.Dtos
{
    public class RentSubmissionListResponseItemDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public UserResponseDto User { get; set; }
        public string ImgSrc { get; set; }
        public string AplNr { get; set; }
        public string RegNr { get; set; }
        public string Category { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public string Status { get; set; }

        public RentSubmissionListResponseItemDto(RentSubmission rentsubmission)
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
            Category = rentsubmission.Category.Name;
            Mark = rentsubmission.Mark.Name;
            Model = rentsubmission.Model;
            Status = rentsubmission.Status;
        }
    }
}
