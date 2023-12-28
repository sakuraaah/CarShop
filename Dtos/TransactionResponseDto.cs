using CarShop.Data;

namespace CarShop.Dtos
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public UserResponseDto User { get; set; }
        public DateTime Created { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public TransactionResponseDto(Transaction transaction)
        {
            Id = transaction.Id;
            Created = transaction.Created;
            
            if(transaction.User != null)
            {
                User = new UserResponseDto(transaction.User);
            }

            Amount = transaction.Amount;
            Description = transaction.Description;
        }
    }
}
