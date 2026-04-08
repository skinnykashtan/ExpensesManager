using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExpensesManager.Interfaces;

namespace ExpensesManager.ViewModels;

public class SidePanelViewModel : INotifyPropertyChanged
{
    private readonly ITransactionRepository _transactionRepository;
    
    public SidePanelViewModel(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    private decimal _totalIncome;
    public decimal TotalIncome
    {
        get => _totalIncome;
        set
        {
            if (_totalIncome == value) return;
            _totalIncome = value;
            NotifyPropertyChanged();
        }
    }

    private decimal _totalExpenses;

    public decimal TotalExpenses
    {
        get => _totalExpenses;
        set
        {
            if (_totalExpenses == value) return;
            _totalExpenses = value;
            NotifyPropertyChanged();
        } 
    }

    private decimal _totalBalance;
    public decimal TotalBalance
    {
        get => _totalBalance;
        set
        {
            if (_totalBalance == value) return;
            _totalBalance = value;
            NotifyPropertyChanged();
        }
    }

    public async Task LoadSumsAsync()
    {
        TotalIncome = await _transactionRepository.GetTotalIncomeAsync();
        TotalExpenses = await _transactionRepository.GetTotalExpensesAsync();
        TotalBalance = _totalIncome - _totalExpenses;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}