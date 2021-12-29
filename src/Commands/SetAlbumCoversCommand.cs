using System;
using System.Collections.Generic;
using System.Linq;
using gmmcli.gmdb;
using Microsoft.Extensions.Logging;

namespace gmmcli
{
    internal class SetAlbumCoversCommand
    {
        private readonly ILogger<SetAlbumCoversCommand> _logger;

        public SetAlbumCoversCommand(ILogger<SetAlbumCoversCommand> logger)
        {
            _logger = logger;
        }

        public int Run(SetAlbumCoversOptions options)
        {
            var items = Utility.LoadMetadataFromJsonFile(options.MetadataPath);

            SetAlbumCovers(items);

            return 0;
        }

        private void SetAlbumCovers(IList<AlbumContext> items)
        {
            if (items.Any())
            {
                using var context = new GrooveMusicService();

                context.Open();

                var albums = GetAlbumsWithArtist(context);

                foreach (var item in items)
                {
                    // Find the Groove Music album
                    var album = (from a in albums
                                 where Equals(a, item)
                                 select a).SingleOrDefault();

                    if (album == default)
                    {
                        _logger.LogWarning("There is no album in Groove Music with the title '{AlbumTitle}' for the artist '{AlbumArtist}'", item.AlbumTitle, item.Artist);
                    }
                    else
                    {
                        _logger.LogInformation("Setting album image for '{AlbumTitle}' ('{AlbumArtist}') to '{CoverImagePath}'", album.Title, item.Artist, item.ImageFile);

                        context.UpdateAlbumArt(album.Id, item.ImageFile, force: true);
                    }
                }
            }
        }

        private IList<AudioAlbum> GetAlbumsWithArtist(in GrooveMusicService item)
        {
            var albums = item.GetAlbums();
            var people = item.GetPeople();

            foreach (var album in albums)
            {
                album.AlbumArtist = people.SingleOrDefault(p => p.Id == album.ArtistId);

                if (album.AlbumArtist == null)
                {
                    _logger.LogWarning("Album artist not found for album '{AlbumTitle}'", album.Title);
                }
            }

            return albums;
        }

        private static bool Equals(AudioAlbum audioAlbum, AlbumContext albumContext)
        {
            return string.Equals(audioAlbum.Title, albumContext.AlbumTitle, StringComparison.OrdinalIgnoreCase)
                && string.Equals(audioAlbum.AlbumArtist.Name, albumContext.Artist, StringComparison.OrdinalIgnoreCase);
        }
    }
}
