using System.Windows;
using System.Windows.Controls;
using ExpensesManager.View.Windows;

namespace ExpensesManager.View.UserControls;

public partial class SidePanel : UserControl
{
    public SidePanel()
    {
        InitializeComponent();
    }

    private void BtnAddTransaction_OnClick(object sender, RoutedEventArgs e)
    {
        SecondWindow win2 = new SecondWindow();
        win2.Show();
    }
}