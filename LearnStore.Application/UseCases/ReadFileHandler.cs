using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class ReadFileHandler(IFileStorageService FileStorageService) : IRequestHandler<ReadFileQuery, Stream>
    {
        public  Task<Stream> Handle(ReadFileQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            var stream = FileStorageService.ReadFile(query.StorageName, query.SellerId);
            return Task.FromResult(stream);
        }
    }
}
