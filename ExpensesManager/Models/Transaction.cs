namespace ExpensesManager.Models;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public string Description { get; set; } = string.Empty;
    public IncomeSource? Source { get; set; }
    public TransactionType Type { get; set; }
    public ExpenseCategory? Category { get; set; }
}