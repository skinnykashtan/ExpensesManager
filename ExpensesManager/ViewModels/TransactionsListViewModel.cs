using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ExpensesManager.CustomControls;
using ExpensesManager.Interfaces;
using ExpensesManager.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace ExpensesManager.ViewModels;

public class TransactionsListViewModel : INotifyPropertyChanged
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly SidePanelViewModel _sidePanelViewModel;

    public ObservableCollection<Transaction> Transactions { get; } = new();

    private ISeries[] _series = Array.Empty<ISeries>();
    public ISeries[] Series
    {
        get => _series;
        set
        {
            _series = value;
            NotifyPropertyChanged();
        }
    }

    private Axis[] _xAxes = Array.Empty<Axis>();
    public Axis[] XAxes
    {
        get => _xAxes;
        set
        {
            _xAxes = value;
            NotifyPropertyChanged();
        }
    }
    
    private Axis[] _yAxes = Array.Empty<Axis>();
    public Axis[] YAxes
    {
        get => _yAxes;
        set
        {
            _yAxes = value;
            NotifyPropertyChanged();
        }
    }
    
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
    
    private bool _isLoading;
    public async Task RefreshAsync()
    {
        if (_isLoading) return;

        _isLoading = true;

        try
        {
            await LoadTransactionsAsync();
            BuildChartFromTransactions();
        }
        finally
        {
            _isLoading = false;
        }
    }

    public async Task LoadTransactionsAsync()
    {
        Transactions.Clear();

        var items = await _transactionRepository.GetAllTransactionsAsync();
        foreach (var item in items.OrderBy(t => t.Date))
        {
            Transactions.Add(item);
        }
    }

    private void BuildChartFromTransactions()
    {
        var groupedByDay = Transactions
            .GroupBy(t => t.Date.Date)
            .OrderBy(g => g.Key)
            .ToList();

        var labels = groupedByDay
            .Select(g => g.Key.ToString("dd.MM"))
            .ToArray();

        var incomeValues = groupedByDay
            .Select(g => (double)g
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount))
            .ToArray();

        var expenseValues = groupedByDay
            .Select(g => (double)g
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount))
            .ToArray();

        Series =
        [
            new ColumnSeries<double>
            {
                Name = "Income",
                Values = incomeValues
            },
            new ColumnSeries<double>
            {
                Name = "Expense",
                Values = expenseValues
            }
        ];

        XAxes =
        [
            new Axis
            {
                Labels = labels
            }
        ];

        YAxes =
        [
            new Axis
            {
                Name = "PLN",
                MinLimit = 0
            }
        ];
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
            await RefreshAsync();
            await _sidePanelViewModel.LoadSumsAsync();
            await _sidePanelViewModel.LoadTopCategoriesAsync();
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}