namespace ExpensesManager.ViewModels;

public class MainViewModel
{
    public AddTransactionViewModel AddTransactionViewModel { get; }
    public SidePanelViewModel SidePanelViewModel { get; }

    public MainViewModel(
        AddTransactionViewModel addTransactionViewModel,
        SidePanelViewModel sidePanelViewModel)
    {
        AddTransactionViewModel = addTransactionViewModel;
        SidePanelViewModel = sidePanelViewModel;
    }
}