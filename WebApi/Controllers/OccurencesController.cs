using Common.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Infrastructure;
using WebApi.Queries.GetOccurences;

namespace WebApi.Controllers
{
	public class OccurencesController : BaseApiController
    {
		public OccurencesController(IApplicationFacade facade)
			: base(facade)
	    {
	    }

		[Route("api/occurences/{word}")]
		[HttpGet]
		[EnableCors(origins: "*", headers: "*", methods: "*")]
		public HttpResponseMessage Get(string word)
		{
			var request = new GetOccurencesRequest(word);
			var response = Facade.Query<GetOccurencesResponse, GetOccurencesRequest>(request);
			
			return CreateHttpResponse(response, HttpStatusCode.OK);
		}
    }
}
