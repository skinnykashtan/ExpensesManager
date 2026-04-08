using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ExpensesManager.CustomControls;
using ExpensesManager.Interfaces;
using ExpensesManager.Models;

namespace ExpensesManager.ViewModels;

public class TransactionsListViewModel : INotifyPropertyChanged
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly SidePanelViewModel _sidePanelViewModel;

    public ObservableCollection<Transaction> Transactions { get; } = new();
    
    public ICommand DeleteCommand { get; }

    public TransactionsListViewModel(ITransactionRepository transactionRepository,
        SidePanelViewModel sidePanelViewModel)
    {
        _transactionRepository = transactionRepository;
        _sidePanelViewModel = sidePanelViewModel;

        DeleteCommand = new AsyncRelayCommand(() => DeleteTransactionAsync(SelectedTransaction));
    }

    private Transaction? _selectedTransaction;

    public Transaction? SelectedTransaction
    {
        get => _selectedTransaction;
        set
        {
            _selectedTransaction = value;
            NotifyPropertyChanged();
        }
    }

    public async Task LoadTransactionsAsync()
    {
        Transactions.Clear();

        var items = await _transactionRepository.GetAllTransactionsAsync();

        foreach (var item in items)
        {
            Transactions.Add(item);
        }
    }

    private async Task DeleteTransactionAsync(Transaction? transaction)
    {
        if (transaction == null) return;

        MessageBoxResult messageBoxResult =
            MessageBox.Show("Are you sure?", "Delete confirmation", MessageBoxButton.YesNo);
        if (messageBoxResult == MessageBoxResult.Yes)
        {
            await _transactionRepository.DeleteTransactionAsync(transaction.Id);
            Transactions.Remove(transaction);
            await _sidePanelViewModel.LoadSumsAsync();
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}