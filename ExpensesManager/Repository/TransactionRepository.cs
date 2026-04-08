using ExpensesManager.Data;
using ExpensesManager.Interfaces;
using ExpensesManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.Repository;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<decimal> GetTotalIncomeAsync()
    {
        return await _context.Transactions
            .Where(t => t.Type == TransactionType.Income)
            .SumAsync(a => a.Amount);
    }

    public async Task<decimal> GetTotalExpensesAsync()
    {
        return await _context.Transactions
            .Where(t => t.Type == TransactionType.Expense)
            .SumAsync(a => a.Amount);
    }
}