using CarShop.Data;

namespace CarShop.Dtos
{
    public class RentOrderResponseDto
    {
        public int Id { get; set; }
        public UserResponseDto User { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int RentItemId { get; set; }
        public string RentItem { get; set; }

        public RentOrderResponseDto(RentOrder rentOrder)
        {
            Id = rentOrder.Id;
            
            if(rentOrder.User != null)
            {
                User = new UserResponseDto(rentOrder.User);
            }

            Status = rentOrder.Status;

            StartTime = rentOrder.StartTime;

            if (rentOrder.EndTime != null)
            {
                EndTime = rentOrder.EndTime;
            }

            RentItemId = rentOrder.RentItemId;

            string vehicleTitle = rentOrder.RentItem.Mark?.Name != "Other" ? rentOrder.RentItem.Mark?.Name + " " + rentOrder.RentItem.Model : rentOrder.RentItem.Model;

            RentItem = vehicleTitle;
        }
    }
}
