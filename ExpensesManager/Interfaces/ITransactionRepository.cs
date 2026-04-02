using ExpensesManager.Models;

namespace ExpensesManager.Interfaces;

public interface ITransactionRepository
{
    Task AddTransactionAsync(Transaction transaction);
}
