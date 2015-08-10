using Common.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Infrastructure
{
	public class BaseApiController : ApiController
	{
		protected IApplicationFacade Facade;

		public BaseApiController(IApplicationFacade facade)
		{
			Facade = facade;
		}

		protected HttpResponseMessage CreateHttpResponse(object data, HttpStatusCode statusCode)
		{
			return new HttpResponseMessage(statusCode)
			{
				Content = GetJSON(data, statusCode)
			};
		}

		protected JsonContent GetJSON(object data, HttpStatusCode statusCode)
		{
			return new JsonContent(new { status = statusCode.ToString().ToLower(), type = "data", data = data });
		}
	}
}