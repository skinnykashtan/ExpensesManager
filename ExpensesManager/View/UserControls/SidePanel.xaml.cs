using System.Windows;
using System.Windows.Controls;
using ExpensesManager.View.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesManager.View.UserControls;

public partial class SidePanel : UserControl
{
    public SidePanel()
    {
        InitializeComponent();
    }

    private void BtnAddTransaction_OnClick(object sender, RoutedEventArgs e)
    {
        var win2 = App.Services.GetRequiredService<SecondWindow>();
        win2.Show();
    }
}