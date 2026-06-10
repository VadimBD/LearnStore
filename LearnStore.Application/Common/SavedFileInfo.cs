using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Common
{
    public record SavedFileInfo
    {
        public string StorageName {  get; set; }
        public string OriginalName { get; set; }
    }
}
