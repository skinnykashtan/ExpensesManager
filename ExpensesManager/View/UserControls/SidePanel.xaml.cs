using System.Windows;
using System.Windows.Controls;

namespace ExpensesManager.View.UserControls;

public partial class SidePanel : UserControl
{
    public SidePanel()
    {
        InitializeComponent();
    }

    private void BtnAddTransaction_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Marysia to maly smrut");
    }
}