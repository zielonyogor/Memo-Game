using System.Windows.Input;

namespace NR155910155992.MemoGame.UI
{
	internal class RelayCommand : ICommand
	{
		public event EventHandler? CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
		private readonly Action<object> execute;
		private readonly Predicate<object> canExecute;
		public bool CanExecute(object? parameter)
		{
			return canExecute == null ? true : canExecute(parameter);
		}
		public void Execute(object? parameter)
		{
			execute(parameter);
		}
		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}
		public RelayCommand(Action<object> _execute) : this(_execute, null)
		{
		}
	}
}
