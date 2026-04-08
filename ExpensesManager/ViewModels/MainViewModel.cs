using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ExpensesManager.CustomControls;
using ExpensesManager.View.UserControls;

namespace ExpensesManager.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public AddTransactionViewModel AddTransactionViewModel { get; }
    public SidePanelViewModel SidePanelViewModel { get; }

    private object _currentView;
    public object CurrentView
    {
        get => _currentView;
        set
        {
            if (_currentView == value) return;
            _currentView = value;
            NotifyPropertyChanged();
        }
    }
    
    public ICommand ShowTransactionsCommand { get; }

    private readonly TransactionsView _transactionsView;
    
    public MainViewModel(
        AddTransactionViewModel addTransactionViewModel,
        SidePanelViewModel sidePanelViewModel,
        TransactionsView transactionsView)
    {
        AddTransactionViewModel = addTransactionViewModel;
        SidePanelViewModel = sidePanelViewModel;
        _transactionsView = transactionsView;
        
        ShowTransactionsCommand = new AsyncRelayCommand(() =>
        {
            ShowTransactions();
            return Task.CompletedTask;
        });
    }
    
    private void ShowTransactions()
    {
        CurrentView = _transactionsView;
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}