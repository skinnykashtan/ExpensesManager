using ExpensesManager.Models;

namespace ExpensesManager.Interfaces;

public interface ITransactionRepository
{
    Task AddTransactionAsync(Transaction transaction);
    Task<decimal> GetTotalIncomeAsync();
    Task<decimal> GetTotalExpensesAsync();
    Task<List<Transaction>> GetAllTransactionsAsync();
    Task DeleteTransactionAsync(int id);
}
