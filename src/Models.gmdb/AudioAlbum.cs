using System;

namespace gmmcli.gmdb
{
    /// <summary>
    /// Mapped to the rows in the tblAudioAlbum table in the Groove Music database
    /// </summary>
    internal class AudioAlbum
    {
        public int Id { get; set; }                         // JET_COLUMNID(0x1), "Id", Long, None
        public int? ArtistId { get; set; }                  // JET_COLUMNID(0x2), "ArtistId", Long, None
        public string AudioAlbumUrl { get; set; }           // JET_COLUMNID(0x100), "AudioAlbumUrl", LongText, Unicode
        public Guid? BingId { get; set; }                   // JET_COLUMNID(0x4), "BingId", 16, None
        public byte? CollectionState { get; set; }          // JET_COLUMNID(0xc), "CollectionState", UnsignedByte, None
        public DateTime? DateAdded { get; set; }            // JET_COLUMNID(0x6), "DateAdded", DateTime, None
        public string Description { get; set; }             // JET_COLUMNID(0x106), "Description", LongText, Unicode
        public int? GenreId { get; set; }                   // JET_COLUMNID(0x8), "GenreId", Long, None
        public string ImageUrl { get; set; }                // JET_COLUMNID(0x101), "ImageUrl", LongText, Unicode
        public int? InternalTrackCount { get; set; }        // JET_COLUMNID(0xf), "InternalTrackCount", Long, None
        public int? LocalOnlyTracksCount { get; set; }      // JET_COLUMNID(0x9), "LocalOnlyTracksCount", Long, None
        public int? LocalRemoteTracksCount { get; set; }    // JET_COLUMNID(0xb), "LocalRemoteTracksCount", Long, None
        public uint? MetadataState { get; set; }            // JET_COLUMNID(0x7), "MetadataState", 14, None
        public bool? MissingArtist { get; set; }            // JET_COLUMNID(0x12), "MissingArtist", Bit, None
        public string MusicMatchImageUrl { get; set; }      // JET_COLUMNID(0x107), "MusicMatchImageUrl", LongText, Unicode
        public DateTime? ReleaseDate { get; set; }          // JET_COLUMNID(0x5), "ReleaseDate", DateTime, None
        public int? RemoteOnlyTracksCount { get; set; }     // JET_COLUMNID(0xa), "RemoteOnlyTracksCount", Long, None
        public int? RemovableTrackCount { get; set; }       // JET_COLUMNID(0x10), "RemovableTrackCount", Long, None
        public int? RowVersion { get; set; }                // JET_COLUMNID(0xd), "RowVersion", Long, None
        public Guid? ServiceMediaId { get; set; }           // JET_COLUMNID(0x3), "ServiceMediaId", 16, None
        public string SortTitle { get; set; }               // JET_COLUMNID(0x104), "SortTitle", LongText, Unicode
        public int? SortTitleGroupIndex { get; set; }       // JET_COLUMNID(0x11), "SortTitleGroupIndex", Long, None
        public string SortTitleOverride { get; set; }       // JET_COLUMNID(0x105), "SortTitleOverride", LongText, Unicode
        public string Title { get; set; }                   // JET_COLUMNID(0x103), "Title", LongText, Unicode
        public string TOC { get; set; }                     // JET_COLUMNID(0x102), "TOC", LongText, Unicode
        public int? TotalTrackCount { get; set; }           // JET_COLUMNID(0xe), "TotalTrackCount", Long, None

        public Person AlbumArtist { get; set; }

        public override string ToString()
        {
            return new { Id, ReleaseDate, AlbumArtist?.Name, Title, ImageUrl }.ToString();
        }
    }
}
