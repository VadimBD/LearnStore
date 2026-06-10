using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Queries
{
    public record ReadFileQuery(string StorageName, string SellerId):IRequest<Stream>;
}
