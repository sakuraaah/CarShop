using CarShop.Dtos;
using CarShop.Models;

namespace CarShop.Data
{
    public interface ITransactionRepository
    {
        IQueryable<Transaction> GetAll(ApplicationUser user);
        List<TransactionResponseDto> GetList(ApplicationUser user);
        public decimal GetBalance(ApplicationUser user);
        TransactionResponseDto? Create(Transaction transaction);
    }
}
