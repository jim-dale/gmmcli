using System.Collections.Generic;
using System.Linq;
using gmmcli.gmdb;
using Microsoft.Extensions.Logging;

namespace gmmcli
{
    internal class ShowGrooveDataCommand
    {
        private readonly ILogger<SetAlbumCoversCommand> _logger;

        public ShowGrooveDataCommand(ILogger<SetAlbumCoversCommand> logger)
        {
            _logger = logger;
        }

        public int Run()
        {
            using var context = new GrooveMusicService();

            context.Open();

            var albums = GetAlbumsWithArtist(context);

            _logger.LogInformation("Album count: {AlbumCount}", albums.Count);

            foreach (var album in albums)
            {
                _logger.LogInformation("Album: {Album}", album);
            }

            return 0;
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
    }
}
