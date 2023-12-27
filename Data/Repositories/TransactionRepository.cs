using CarShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class TransactionRepository : Repository, ITransactionRepository
    {
        public TransactionRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<Transaction> GetAll(ApplicationUser user)
        {
            return _context.Transactions
                .Where(x => x.User == user)
                .OrderByDescending(x => x.Created);
        }

        public decimal GetBalance(ApplicationUser user)
        {
            return GetAll(user)
                .Sum(x => x.Amount);
        }

        public Transaction? Create(Transaction transaction)
        {
            transaction = _context.Transactions.Add(transaction).Entity;
            _context.SaveChanges();

            return transaction;
        }
    }
}
