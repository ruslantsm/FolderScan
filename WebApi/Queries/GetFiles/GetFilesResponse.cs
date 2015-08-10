using Common.Queries;
using System.Collections.Generic;
using WordEntryDal.Models;

namespace WebApi.Queries.GetFiles
{
	public class GetFilesResponse : IQueryResponse
    {
        public IEnumerable<FEntry> Files { get; private set; }


		public GetFilesResponse(IEnumerable<FEntry> files)
		{
			Files = files;
		}
    }
}