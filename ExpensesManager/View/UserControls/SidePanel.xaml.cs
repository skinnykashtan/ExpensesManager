using System.Windows;
using System.Windows.Controls;
using ExpensesManager.View.Windows;
using ExpensesManager.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesManager.View.UserControls;

public partial class SidePanel : UserControl
{
    public SidePanel()
    {
        InitializeComponent();
    }
    
    private SecondWindow? _addTransactionWindow;

    private void BtnAddTransaction_OnClick(object sender, RoutedEventArgs e)
    {
        if (_addTransactionWindow != null)
        {
            if (_addTransactionWindow.WindowState == WindowState.Minimized)
                _addTransactionWindow.WindowState = WindowState.Normal;

            _addTransactionWindow.Activate();
            return;
        }

        _addTransactionWindow = App.Services.GetRequiredService<SecondWindow>();
        _addTransactionWindow.Owner = Window.GetWindow(this);

        _addTransactionWindow.Closed += (_, _) =>
        {
            _addTransactionWindow = null;
        };

        _addTransactionWindow.Show();
    }
}