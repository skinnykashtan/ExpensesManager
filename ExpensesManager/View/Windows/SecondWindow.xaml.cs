using System.Windows;
using ExpensesManager.Models;
using ExpensesManager.View.UserControls;
using ExpensesManager.ViewModels;

namespace ExpensesManager.View.Windows;

public partial class SecondWindow : Window
{
    public SecondWindow()
    {
        InitializeComponent();
        DataContext = new AddTransactionViewModel();
    }
    
    
}