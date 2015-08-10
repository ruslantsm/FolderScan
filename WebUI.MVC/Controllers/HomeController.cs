using Common.Infrastructure;
using System.Web.Mvc;
using WebUI.MVC.Queries.GetWords;
using WebUI.MVC.ViewModels;


namespace WebUI.MVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly IApplicationFacade _facade;

		public HomeController(IApplicationFacade facade)
		{
			_facade = facade;
		}


		public ViewResult Index(int? pageSize, int? page)
		{
			var size = pageSize ?? 15;
			var pageNumber = page ?? 1;
			
			var request = new GetWordsRequest(size, pageNumber);

			var response = _facade.Query<GetWordsResponse, GetWordsRequest>(request);

			var viewModel = new WordsViewModel(response.Words, size, pageNumber, response.Total);

			return View(viewModel);
		}

		public ActionResult Contact()
		{
			return View();
		}
	}
}