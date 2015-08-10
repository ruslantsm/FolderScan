using Common.Queries;
using WordEntryDal.Repositories;

namespace WebApi.Queries.GetFiles
{
	public class GetFilesQuery : IQuery<GetFilesResponse, GetFilesRequest>
	{
		private readonly IFileRepository _fileRepository;

		public GetFilesQuery(IFileRepository fileRepository)
		{
			_fileRepository = fileRepository;
		}

		public GetFilesResponse Handle(GetFilesRequest request)
		{
			var files = _fileRepository.GetFiles();

			return new GetFilesResponse(files);
		}
	}
}