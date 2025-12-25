
namespace NR155910155992.MemoGame.UI.Services
{
	public class CloseModalAndNavigateService : INavigationService
	{
		private readonly CloseModalNavigationService _closeModalNavigationService;
		private readonly INavigationService _navigateService;

		public CloseModalAndNavigateService(
			CloseModalNavigationService closeModalNavigationService,
			INavigationService navigateService)
		{
			_closeModalNavigationService = closeModalNavigationService;
			_navigateService = navigateService;
		}

		public void Navigate()
		{
			_closeModalNavigationService.Navigate();
			_navigateService.Navigate();
		}
	}
}
