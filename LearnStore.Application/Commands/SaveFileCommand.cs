using LearnStore.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Commands
{
    public record SaveFileCommand : IRequest<SavedFileInfo>
    {
        public Stream FileStream { get; init; }
        public string OriginalName { get; init; }
        public string SellerId { get; init; }

      
    }
}
