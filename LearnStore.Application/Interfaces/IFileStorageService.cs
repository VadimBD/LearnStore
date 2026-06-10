using LearnStore.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<SavedFileInfo> SaveFileAsync(Stream fileStream, string originalName, string sellerId);

        Stream ReadFile(string sellerId, string storageName);
    }
}
