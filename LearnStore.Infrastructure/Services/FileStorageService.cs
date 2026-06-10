using LearnStore.Application.Common;
using LearnStore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly string _storagePath;
        public FileStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            _storagePath = _configuration["FileStorage:Path"]?? throw new InvalidOperationException("File storage path is not configured");
        }
        public Stream ReadFile(string sellerId, string storageName)
        {

            var fullPath = Path.Combine(_storagePath, storageName , sellerId);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("File not found", fullPath);

            return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        }

        public async Task<SavedFileInfo> SaveFileAsync(Stream fileStream, string originalName, string sellerId)
        {
            var storageName = $"{Guid.NewGuid()}{Path.GetExtension(originalName)}";
            var sellerPath = Path.Combine(_storagePath, sellerId);

            if (!Directory.Exists(sellerPath))
                Directory.CreateDirectory(sellerPath);

            var fullPath = Path.Combine(sellerPath, storageName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await fileStream.CopyToAsync(stream);
            }

            return new SavedFileInfo { StorageName = storageName, OriginalName = originalName };
        }
    }
}
