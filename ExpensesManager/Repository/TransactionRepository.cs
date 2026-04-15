using ExpensesManager.Data;
using ExpensesManager.DTOs;
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

    public async Task<List<Transaction>> GetAllTransactionsAsync()
    {
        return await _context.Transactions
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task DeleteTransactionAsync(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);

        if (transaction == null) return;

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<List<TopCategoryDto>> GetTopExpenseCategoriesAsync(int count)
    {
        var data = await _context.Transactions
            .Where(t => t.Type == TransactionType.Expense)
            .GroupBy(t => t.Category)
            .Select(g => new TopCategoryDto
            {
                CategoryName = g.Key.ToString(),
                TotalAmount = g.Sum(x => x.Amount)
            })
            .ToListAsync();

        return data
            .OrderByDescending(x => x.TotalAmount)
            .Take(count)
            .ToList();
    }
}