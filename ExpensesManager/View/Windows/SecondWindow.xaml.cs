using System.Windows;
using ExpensesManager.Models;
using ExpensesManager.View.UserControls;

namespace ExpensesManager.View.Windows;

public partial class SecondWindow : Window
{
    public SecondWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    public List<TransactionType> TransactionTypes
    {
        get => Enum.GetValues(typeof(TransactionType))
            .Cast<TransactionType>()
            .ToList();
    }

    public TransactionType SelectedTransactionType { get; set; }
    

}