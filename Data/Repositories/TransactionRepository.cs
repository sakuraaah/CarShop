using CarShop.Dtos;
using CarShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class TransactionRepository : Repository, ITransactionRepository
    {
        public TransactionRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<Transaction> GetAll(ApplicationUser user)
        {
            return _context.Transactions.Where(x => x.User == user);
        }

        public List<TransactionResponseDto> GetList(ApplicationUser user)
        {
            return GetAll(user)
                .OrderByDescending(x => x.Created)
                .Select(x => new TransactionResponseDto(x))
                .ToArray()
                .ToList();
        }

        public decimal GetBalance(ApplicationUser user)
        {
            return GetAll(user)
                .Sum(x => x.Amount);
        }

        public TransactionResponseDto? Create(Transaction transaction)
        {
            transaction = _context.Transactions.Add(transaction).Entity;
            _context.SaveChanges();

            TransactionResponseDto transactionResponseDto = new TransactionResponseDto(transaction);

            return transactionResponseDto;
        }
    }
}
