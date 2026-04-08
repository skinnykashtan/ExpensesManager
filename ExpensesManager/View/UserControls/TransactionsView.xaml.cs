using System.Windows;
using System.Windows.Controls;
using ExpensesManager.ViewModels;

namespace ExpensesManager.View.UserControls;

public partial class TransactionsView : UserControl
{
    public TransactionsView(TransactionsListViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
        
        Loaded += async (_, _) =>
        {
            await vm.LoadTransactionsAsync();
        };
    }
}