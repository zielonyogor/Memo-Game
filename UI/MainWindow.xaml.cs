using NR155910155992.MemoGame.UI.ViewModels;
using System.Windows;

namespace NR155910155992.MemoGame.UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow(MainViewModel mainViewModel)
		{
			DataContext = mainViewModel;
            InitializeComponent();
		}
	}
}