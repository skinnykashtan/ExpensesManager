using System.Windows.Controls;
using ExpensesManager.ViewModels;

namespace ExpensesManager.View.UserControls;

public partial class TransactionsChartView : UserControl
{
    public TransactionsChartView(TransactionsListViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        Loaded += async (_, _) =>
        {
            await viewModel.RefreshAsync();
        };
    }
}