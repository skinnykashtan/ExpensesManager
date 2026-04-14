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
    public ICommand ShowTransactionsChartCommand { get; }

    private readonly TransactionsView _transactionsView;
    private readonly TransactionsChartView _transactionsChartView;
    
    public MainViewModel(
        AddTransactionViewModel addTransactionViewModel,
        SidePanelViewModel sidePanelViewModel,
        TransactionsView transactionsView,
        TransactionsChartView transactionsChartView)
    {
        AddTransactionViewModel = addTransactionViewModel;
        SidePanelViewModel = sidePanelViewModel;
        _transactionsView = transactionsView;
        _transactionsChartView = transactionsChartView;
        
        ShowTransactionsCommand = new AsyncRelayCommand(() =>
        {
            ShowTransactions();
            return Task.CompletedTask;
        });
        
        ShowTransactionsChartCommand = new AsyncRelayCommand(() =>
        {
            ShowTransactionsChart();
            return Task.CompletedTask;
        });

        CurrentView = _transactionsChartView;
    }
    
    private void ShowTransactions()
    {
        CurrentView = _transactionsView;
    }
    
    private void ShowTransactionsChart()
    {
        CurrentView = _transactionsChartView;
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}