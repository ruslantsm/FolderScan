using Common.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Infrastructure;
using WebApi.Queries.GetFiles;

namespace WebApi.Controllers
{
    public class FilesController : BaseApiController
    {
	    public FilesController(IApplicationFacade facade)
			: base(facade)
	    {
	    }

		[Route("api/files")]
		[HttpGet]
		[EnableCors(origins: "*", headers: "*", methods: "*")]
		public HttpResponseMessage Get()
		{
			var request = new GetFilesRequest();
			var response = Facade.Query<GetFilesResponse, GetFilesRequest>(request);
			
			return CreateHttpResponse(response, HttpStatusCode.OK);
		}

    }
}
