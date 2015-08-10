using Common.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Infrastructure;
using WebApi.Queries.GetWords;

namespace WebApi.Controllers
{
	public class WordsController : BaseApiController
    {
		public WordsController(IApplicationFacade facade)
			: base(facade)
	    {
	    }

		[Route("api/words")]
		[HttpGet]
		[EnableCors(origins: "*", headers: "*", methods: "*")]
		public HttpResponseMessage Get()
		{
			var request = new GetWordsRequest();
			var response = Facade.Query<GetWordsResponse, GetWordsRequest>(request);
			
			return CreateHttpResponse(response, HttpStatusCode.OK);
		}
    }
}
