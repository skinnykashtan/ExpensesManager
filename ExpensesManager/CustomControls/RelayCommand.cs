using System.Windows.Input;

namespace ExpensesManager.CustomControls;

public class AsyncRelayCommand : ICommand
{
    private readonly Func<Task> _execute;

    public AsyncRelayCommand(Func<Task> execute)
    {
        _execute = execute;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        await _execute();
    }

    public event EventHandler? CanExecuteChanged;
}