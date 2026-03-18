using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExpensesManager.Models;

namespace ExpensesManager.ViewModels;

public class AddTransactionViewModel : INotifyPropertyChanged
{
    public List<TransactionType> TransactionTypes { get; }

    private TransactionType? _selectedTransactionType;
    public TransactionType? SelectedTransactionType
    {
        get => _selectedTransactionType;
        
        set
        {
            if (value != _selectedTransactionType)
            {
                _selectedTransactionType = value;
                NotifyPropertyChanged();
            }
        }
    }

    public AddTransactionViewModel()
    {
        TransactionTypes = Enum.GetValues(typeof(TransactionType))
            .Cast<TransactionType>()
            .ToList();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}