using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExpensesManager.ViewModels;
using Microsoft.Win32;

namespace ExpensesManager.View.Windows;

public partial class MainWindow : Window 
{
    public MainWindow(AddTransactionViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
    