using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.Commands
{
	public class RelayCommand : ICommand
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

	public class RelayCommand<T> : ICommand
	{
		public event EventHandler? CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		private readonly Action<T> _execute;
		private readonly Predicate<T>? _canExecute;

		public RelayCommand(Action<T> execute, Predicate<T>? canExecute = null)
		{
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
			_canExecute = canExecute;
		}

		public bool CanExecute(object? parameter)
		{
			if (parameter is T t)
				return _canExecute?.Invoke(t) ?? true;
			if (parameter is null && default(T) is null)
				return _canExecute?.Invoke((T)parameter) ?? true;
			return false;
		}

		public void Execute(object? parameter)
		{
			if (parameter is T t)
			{
				_execute(t);
				return;
			}
			if (parameter is null && default(T) is null)
			{
				_execute((T)parameter);
				return;
			}

			throw new InvalidOperationException(
				$"Command parameter must be of type {typeof(T)}");
		}
	}
}
