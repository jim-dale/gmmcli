using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using gmmcli.gmdb;
using Microsoft.Extensions.Logging;

namespace gmmcli
{
    internal class GrooveMusicData
    {
        public IList<AudioAlbum> Albums { get; set; }
        public IList<Person> People { get; set; }
    }

    internal class ExportGrooveDataCommand
    {
        private readonly ILogger<SetAlbumCoversCommand> _logger;

        public ExportGrooveDataCommand(ILogger<SetAlbumCoversCommand> logger)
        {
            _logger = logger;
        }

        public int Run()
        {
            using var context = new GrooveMusicService();

            context.Open();

            var item = GetAlbumsWithArtist(context);

            _logger.LogInformation("Album count: {AlbumCount}", item.Albums.Count);
            _logger.LogInformation("People count: {PeopleCount}", item.People.Count);

            var json = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText("GrooveMusicData.json", json);

            return 0;
        }

        private GrooveMusicData GetAlbumsWithArtist(in GrooveMusicService item)
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

            return new GrooveMusicData { Albums = albums, People = people };
        }
    }
}
