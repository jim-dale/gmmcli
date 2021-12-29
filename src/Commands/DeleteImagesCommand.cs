﻿using Microsoft.Extensions.Logging;
using System.IO;

namespace gmmcli
{
    internal class DeleteImagesCommand
    {

        private readonly ILogger<SetAlbumCoversCommand> _logger;

        public DeleteImagesCommand(ILogger<SetAlbumCoversCommand> logger)
        {
            _logger = logger;
        }

        public int Run()
        {
            var folder = EseUtility.GetGrooveMusicFolder();
            if (folder == default)
            {
                _logger.LogInformation("No images to delete as Groove Music has not been used yet.");
            }
            else
            {
                DeleteAllFilesInFolder(Path.Combine(folder, @"LocalState\ImageCache\20"));
                DeleteAllFilesInFolder(Path.Combine(folder, @"LocalState\ImageRetrievalFailure"));
                DeleteAllFilesInFolder(Path.Combine(folder, @"LocalState\ImageStore"));
            }

            return 0;
        }

        private void DeleteAllFilesInFolder(string folder)
        {
            if (Directory.Exists(folder))
            {
                var items = Directory.GetFiles(folder);
                foreach (var item in items)
                {
                    _logger.LogInformation("Deleting: '{FilePath}'", item);

                    File.Delete(item);
                }
            }
        }
    }
}
