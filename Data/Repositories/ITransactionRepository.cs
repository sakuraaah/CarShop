using CarShop.Models;

namespace CarShop.Data
{
    public interface ITransactionRepository
    {
        IQueryable<Transaction> GetAll(ApplicationUser user);
        public decimal GetBalance(ApplicationUser user);
        Transaction? Create(Transaction transaction);
    }
}
