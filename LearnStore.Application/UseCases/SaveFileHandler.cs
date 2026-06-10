using LearnStore.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class SaveFileHandler(IFileStorageService FileStorageService) : IRequestHandler<SaveFileCommand, SavedFileInfo>
    {
        public async Task<SavedFileInfo> Handle(SaveFileCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));

            var savedFileInfo = await FileStorageService.SaveFileAsync(command.FileStream, command.OriginalName, command.SellerId);
            return savedFileInfo;
        }
    }
}
