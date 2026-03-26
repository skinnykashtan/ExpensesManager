using System.Windows;
using ExpensesManager.Models;
using ExpensesManager.View.UserControls;
using ExpensesManager.ViewModels;

namespace ExpensesManager.View.Windows;

public partial class SecondWindow : Window
{
    private AddTransactionViewModel _viewModel;
    
    public SecondWindow()
    {
        InitializeComponent();
        _viewModel = new AddTransactionViewModel();
        DataContext = _viewModel;
    }


    private void BtnSaveTransaction_OnClick(object sender, RoutedEventArgs e)
    {
        if (!_viewModel.Validate())
        {
            return;
        }
    }
}