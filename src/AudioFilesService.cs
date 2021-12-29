using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Logging;

namespace gmmcli
{
    internal class AudioFilesService
    {
        private readonly ILogger<AudioFilesService> _logger;

        public AudioFilesService(ILogger<AudioFilesService> logger)
        {
            _logger = logger;
        }

        public IList<AlbumContext> GetAlbumsFromFolder(string folder, string patterns)
        {
            var result = new List<AlbumContext>();

            var matcher = new Matcher(StringComparison.Ordinal);
            matcher.AddIncludePatterns(patterns.Split(';', StringSplitOptions.RemoveEmptyEntries));

            var subFolder = string.Empty;

            var files = matcher.GetResultsInFullPath(folder);
            foreach (var file in files)
            {
                var currentFolder = Path.GetDirectoryName(file);

                try
                {
                    var tagFile = TagLib.File.Create(file);
                    var mimeType = tagFile.MimeType;
                    var album = tagFile.Tag.Album;
                    var artist = tagFile.Tag.FirstAlbumArtist;
                    var imageFile = Path.Combine(currentFolder, "folder.jpg");

                    if (string.IsNullOrWhiteSpace(album))
                    {
                        _logger.LogError("No title for the album in '{AlbumPath}'.", currentFolder);
                    }

                    if (string.IsNullOrWhiteSpace(album) == false
                        && string.Equals(subFolder, currentFolder) == false)
                    {
                        _logger.LogInformation("'{MimeType}' '{Artist}' '{AlbumTitle}' '{CoverImagePath}'", mimeType, artist, album, imageFile);

                        if (File.Exists(imageFile) == false)
                        {
                            var cover = tagFile.Tag.Pictures.FirstOrDefault();
                            if (cover is null)
                            {
                                _logger.LogError("No cover image for the album in '{AlbumPath}'.", currentFolder);
                            }
                            if (cover != null && cover.MimeType == "image/jpeg")
                            {
                                File.WriteAllBytes(imageFile, cover.Data.Data);
                            }
                        }

                        if (File.Exists(imageFile))
                        {
                            var item = new AlbumContext
                            {
                                Folder = currentFolder,
                                Artist = artist,
                                AlbumTitle = album,
                                ImageFile = imageFile
                            };

                            result.Add(item);

                            subFolder = currentFolder;
                        }
                        else
                        {
                            _logger.LogWarning("No cover image for audio file '{FilePath}'.", file);
                        }
                    }
                }
                catch (TagLib.UnsupportedFormatException ex)
                {
                    _logger.LogError(ex, "Error while searching for audio files.");
                }
            }
            _logger.LogInformation("{AlbumCount} albums found.", result.Count);

            return result;
        }
    }
}
