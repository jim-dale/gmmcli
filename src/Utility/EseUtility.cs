using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using gmmcli.gmdb;
using Microsoft.Isam.Esent.Interop;

namespace gmmcli
{
    internal static class EseUtility
    {
        const string GrooveMusicSubFolder = @"Packages\Microsoft.ZuneMusic_8wekyb3d8bbwe";

        internal static string GetDatabaseFolder()
        {
            string result = default;

            var parent = Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), GrooveMusicSubFolder, @"LocalState\Database");
            if (Directory.Exists(parent))
            {
                // If there is more than one database we can't automatically decide which one to modify
                // so throw an exception if we find more than one sub-directory
                result = Directory.EnumerateDirectories(parent).SingleOrDefault();
            }

            return result;
        }

        internal static string GetGrooveMusicFolder()
        {
            string result = default;

            var folder = Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), GrooveMusicSubFolder);
            if (Directory.Exists(folder))
            {
                result = folder;
            }

            return result;
        }

        internal static Person ConvertToPerson(Session session, Table table, IEnumerable<ColumnInfo> columns)
        {
            var result = new Person();

            foreach (var column in columns)
            {
                switch (column.Name.ToLowerInvariant())
                {
                    case "bingid":
                        result.BingId = Api.RetrieveColumnAsGuid(session, table, column.Columnid);
                        break;
                    case "description":
                        result.Description = ConvertToString(session, table, column);
                        break;
                    case "id":
                        result.Id = Api.RetrieveColumnAsInt32(session, table, column.Columnid).Value;
                        break;
                    case "metadatastate":
                        result.MetadataState = Api.RetrieveColumnAsUInt32(session, table, column.Columnid);
                        break;
                    case "name":
                        result.Name = ConvertToString(session, table, column);
                        break;
                    case "rowversion":
                        result.RowVersion = Api.RetrieveColumnAsInt32(session, table, column.Columnid);
                        break;
                    case "servicemediaid":
                        result.ServiceMediaId = Api.RetrieveColumnAsGuid(session, table, column.Columnid);
                        break;
                    case "sortname":
                        result.SortName = ConvertToString(session, table, column);
                        break;
                    case "sortnamegroupindex":
                        result.SortNameGroupIndex = Api.RetrieveColumnAsInt32(session, table, column.Columnid);
                        break;
                    case "sortnameoverride":
                        result.SortNameOverride = ConvertToString(session, table, column);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        internal static AudioAlbum ConvertToAudioAlbum(Session session, Table table, IEnumerable<ColumnInfo> columns)
        {
            var result = new AudioAlbum();

            foreach (var column in columns)
            {
                switch (column.Name.ToLowerInvariant())
                {
                    case "artistid":
                        result.ArtistId = Api.RetrieveColumnAsInt32(session, table, column.Columnid);
                        break;
                    case "audioalbumurl":
                        result.AudioAlbumUrl = ConvertToString(session, table, column);
                        break;
                    case "bingid":
                        result.BingId = Api.RetrieveColumnAsGuid(session, table, column.Columnid);
                        break;
                    case "collectionstate":
                        result.CollectionState = Api.RetrieveColumnAsByte(session, table, column.Columnid);
                        break;
                    case "dateadded":
                        result.DateAdded = Api.RetrieveColumnAsDateTime(session, table, column.Columnid);
                        break;
                    case "description":
                        result.Description = ConvertToString(session, table, column);
                        break;
                    case "genreid":
                        result.GenreId = Api.RetrieveColumnAsInt32(session, table, column.Columnid);
                        break;
                    case "id":
                        result.Id = Api.RetrieveColumnAsInt32(session, table, column.Columnid).Value;
                        break;
                    case "imageurl":
                        result.ImageUrl = ConvertToString(session, table, column);
                        break;
                    case "internaltrackcount":
                        result.InternalTrackCount = Api.RetrieveColumnAsInt32(session, table, column.Columnid);
                        break;
                    case "localonlytrackscount":
                        result.LocalOnlyTracksCount = Api.RetrieveColumnAsInt32(session, table, column.Columnid);
                        break;
                    case "localremotetrackscount":
                        result.LocalRemoteTracksCount = Api.RetrieveColumnAsInt32(session, table, column.Columnid);
                        break;
                    case "metadatastate":
                        result.MetadataState = Api.RetrieveColumnAsUInt32(session, table, column.Columnid);
                        break;
                    case "missingartist":
                        result.MissingArtist = Api.RetrieveColumnAsBoolean(session, table, column.Columnid);
                        break;
                    case "musicmatchimageurl":
                        result.MusicMatchImageUrl = ConvertToString(session, table, column);
                        break;
                    case "releasedate":
                        result.ReleaseDate = Api.RetrieveColumnAsDateTime(session, table, column.Columnid);
                        break;
                    case "remoteonlytrackscount":
                        result.RemoteOnlyTracksCount = Api.RetrieveColumnAsInt32(session, table, column.Columnid);
                        break;
                    case "removabletrackcount":
                        result.RemovableTrackCount = Api.RetrieveColumnAsInt32(session, table, column.Columnid);
                        break;
                    case "rowversion":
                        result.RowVersion = Api.RetrieveColumnAsInt32(session, table, column.Columnid);
                        break;
                    case "servicemediaid":
                        result.ServiceMediaId = Api.RetrieveColumnAsGuid(session, table, column.Columnid);
                        break;
                    case "sorttitle":
                        result.SortTitle = ConvertToString(session, table, column);
                        break;
                    case "sorttitlegroupindex":
                        result.SortTitleGroupIndex = Api.RetrieveColumnAsInt32(session, table, column.Columnid);
                        break;
                    case "sorttitleoverride":
                        result.SortTitleOverride = ConvertToString(session, table, column);
                        break;
                    case "title":
                        result.Title = ConvertToString(session, table, column);
                        break;
                    case "toc":
                        result.TOC = ConvertToString(session, table, column);
                        break;
                    case "totaltrackcount":
                        result.TotalTrackCount = Api.RetrieveColumnAsInt32(session, table, column.Columnid);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        internal static string ConvertToString(Session session, Table table, ColumnInfo column)
        {
            return column.Cp switch
            {
                JET_CP.Unicode => Api.RetrieveColumnAsString(session, table, column.Columnid, Encoding.Unicode).FromCString(),
                JET_CP.ASCII => Api.RetrieveColumnAsString(session, table, column.Columnid, Encoding.ASCII).FromCString(),
                _ => null,
            };
        }
    }
}
