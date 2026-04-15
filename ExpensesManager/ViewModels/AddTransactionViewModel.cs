using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ExpensesManager.CustomControls;
using ExpensesManager.Interfaces;
using ExpensesManager.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace ExpensesManager.ViewModels;

public class AddTransactionViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly SidePanelViewModel _sidePanelViewModel;
    private readonly TransactionsListViewModel _transactionsListViewModel;

    public AddTransactionViewModel(ITransactionRepository transactionRepository, SidePanelViewModel sidePanelViewModel, TransactionsListViewModel transactionsListViewModel)
    {
        _transactionRepository = transactionRepository;
        _sidePanelViewModel = sidePanelViewModel;
        _transactionsListViewModel = transactionsListViewModel;
        SaveCommand = new AsyncRelayCommand(Save);
    }
    
    public List<TransactionType> TransactionTypes { get; } =
        Enum.GetValues<TransactionType>().ToList();

    public List<IncomeSource> Sources { get; } =
        Enum.GetValues<IncomeSource>().ToList();

    public List<ExpenseCategory> Categories { get; } =
        Enum.GetValues<ExpenseCategory>().ToList();

    private readonly Dictionary<string, List<string>> _errors = new();

    private TransactionType _selectedTransactionType = TransactionType.Expense;
    private IncomeSource? _selectedSource;
    private string _amountText = string.Empty;
    private DateTime? _selectedDate;
    private ExpenseCategory? _selectedCategory;
    private string _description = string.Empty;

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            NotifyPropertyChanged();
        }
    }

    public TransactionType SelectedTransactionType
    {
        get => _selectedTransactionType;

        set
        {
            if (value == _selectedTransactionType) return;

            _selectedTransactionType = value;
            ResetFieldsForTransactionType();
            NotifyPropertyChanged();
        }
    }

    public IncomeSource? SelectedSource
    {
        get => _selectedSource;

        set
        {
            if (value == _selectedSource) return;

            _selectedSource = value;
            ClearErrors(nameof(SelectedSource));

            if (SelectedTransactionType == TransactionType.Income && value == null)
            {
                AddError(nameof(SelectedSource), "source is required");
            }

            NotifyPropertyChanged();
        }
    }

    public ExpenseCategory? SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (value == _selectedCategory) return;

            _selectedCategory = value;
            ClearErrors(nameof(SelectedCategory));

            if (SelectedTransactionType == TransactionType.Expense && value == null)
            {
                AddError(nameof(SelectedCategory), "category is required");
            }

            NotifyPropertyChanged();
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string AmountText
    {
        get => _amountText;

        set
        {
            if (_amountText == value) return;

            _amountText = value;
            ClearErrors(nameof(AmountText));

            if (string.IsNullOrWhiteSpace(value))
            {
                AddError(nameof(AmountText), "field is required");
            }
            else if (!decimal.TryParse(value, out decimal result))
            {
                AddError(nameof(AmountText), "must be a number");
            }
            else if (result <= 0)
            {
                AddError(nameof(AmountText), "must be greater than zero");
            }

            NotifyPropertyChanged();
        }
    }

    public DateTime? SelectedDate
    {
        get => _selectedDate;

        set
        {
            if (value == _selectedDate) return;

            _selectedDate = value;
            ClearErrors(nameof(SelectedDate));

            if (value == null)
            {
                AddError(nameof(SelectedDate), "select valid date");
            }

            NotifyPropertyChanged();
        }
    }

    private void ClearErrors(string propertyName)
    {
        if (_errors.ContainsKey(propertyName))
        {
            _errors.Remove(propertyName);

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }

    private void AddError(string propertyName, string error)
    {
        if (!_errors.ContainsKey(propertyName))
        {
            _errors[propertyName] = new List<string>();
        }

        _errors[propertyName].Add(error);

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        if (propertyName != null && _errors.ContainsKey(propertyName))
        {
            return _errors[propertyName];
        }

        return Enumerable.Empty<string>();
    }

    public bool HasErrors => _errors.Any();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    private void ResetFieldsForTransactionType()
    {
        if (SelectedTransactionType == TransactionType.Income)
        {
            SelectedCategory = null;
            ClearErrors(nameof(SelectedCategory));
        }
        else if (SelectedTransactionType == TransactionType.Expense)
        {
            SelectedSource = null;
            ClearErrors(nameof(SelectedSource));
        }
    }

    public bool Validate()
    {
        ClearErrors(nameof(AmountText));
        ClearErrors(nameof(SelectedDate));
        ClearErrors(nameof(SelectedSource));
        ClearErrors(nameof(SelectedCategory));

        if (string.IsNullOrWhiteSpace(AmountText))
        {
            AddError(nameof(AmountText), "field is required");
        }

        if (SelectedDate == null)
        {
            AddError(nameof(SelectedDate), "date is required");
        }

        if (SelectedTransactionType == TransactionType.Income && SelectedSource == null)
        {
            AddError(nameof(SelectedSource), "source is required");
        }

        if (SelectedTransactionType == TransactionType.Expense && SelectedCategory == null)
        {
            AddError(nameof(SelectedCategory), "category is required");
        }

        return !HasErrors;
    }

    public ICommand SaveCommand { get; }

    private async Task Save()
    {
        if (!Validate())
        {
            return;
        }
        
        decimal amount;

        if (!decimal.TryParse(AmountText, NumberStyles.Any, CultureInfo.CurrentCulture, out amount) &&
            !decimal.TryParse(AmountText, NumberStyles.Any, CultureInfo.InvariantCulture, out amount))
        {
            MessageBox.Show("Invalid amount format");
            return;
        }

        Transaction transaction = new Transaction()
        {
            Amount = amount,
            Category = SelectedCategory,
            Date = SelectedDate.Value.Date,
            Description = Description?.Trim() ?? string.Empty,
            Source = SelectedSource,
            Type = SelectedTransactionType
        };

        await _transactionRepository.AddTransactionAsync(transaction);

        await _transactionsListViewModel.RefreshAsync();
        await _sidePanelViewModel.LoadSumsAsync();
        await _sidePanelViewModel.LoadTopCategoriesAsync();
    }
}