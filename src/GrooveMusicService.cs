using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using gmmcli.gmdb;
using Microsoft.Isam.Esent.Interop;

namespace gmmcli
{
    internal class GrooveMusicService : IDisposable
    {
        private const string DatabaseName = "EntClientDb.edb";
        private const string AlbumsTableName = "tblAudioAlbum";
        private const string AlbumIdIndexName = "idxAlbumId";
        private const string PeopleTableName = "tblPerson";
        private const string AlbumTitleIndexName = "idxTitle";
        private Instance _instance = default;
        private Session _session = default;
        private JET_DBID _databaseId = JET_DBID.Nil;
        private Action<JET_dbstate> _checkDbState;

        internal void SetCheckDbStateAction(Action<JET_dbstate> checkDbState)
        {
            _checkDbState = checkDbState;
        }

        internal void Open()
        {
            var folder = EseUtility.GetDatabaseFolder();
            if (folder == default)
            {
                throw new DirectoryNotFoundException("Groove database folder has not been created. Run Groove Music first to create a database." + Environment.NewLine + "Path: \"" + folder + "\"");
            }

            var path = Path.Combine(folder, DatabaseName);

            Api.JetGetDatabaseFileInfo(path, out int pageSize, JET_DbInfo.PageSize);
            SystemParameters.DatabasePageSize = pageSize;

            Api.JetGetDatabaseFileInfo(path, out JET_DBINFOMISC info, JET_DbInfo.Misc);
            _checkDbState?.Invoke(info.dbstate);

            _instance = new Instance(Guid.NewGuid().ToString(), "Groove Manager");
            _instance.Parameters.LogFileDirectory = folder;
            _instance.Parameters.SystemDirectory = folder;
            _instance.Parameters.CircularLog = true;

            _instance.Init();

            _session = new Session(_instance);

            Api.JetAttachDatabase(_session, path, AttachDatabaseGrbit.None);
            Api.JetOpenDatabase(_session, path, null, out _databaseId, OpenDatabaseGrbit.None);
        }

        public IEnumerable<string> GetTableNames()
        {
            return Api.GetTableNames(_session, _databaseId);

            //    foreach (var name in names)
            //    {
            //        ShowTableSchema(name);
            //    }
        }

        //public void ShowTableSchema(string name)
        //{
        //    using var table = new Table(_session, _databaseId, name, OpenTableGrbit.ReadOnly);

        //    Console.WriteLine();
        //    Console.WriteLine("\"" + name + "\"");

        //    var columns = Api.GetTableColumns(_session, table);
        //    foreach (var column in columns)
        //    {
        //        Console.WriteLine($"\t{column.Columnid}, \"{column.Name}\", {column.Coltyp}, {column.Cp}");
        //    }
        //}

        internal AudioAlbum GetAlbum(int id)
        {
            AudioAlbum result = default;

            using var table = new Table(_session, _databaseId, AlbumsTableName, OpenTableGrbit.ReadOnly);

            Api.JetSetCurrentIndex(_session, table, AlbumIdIndexName);
            Api.MakeKey(_session, table, id, MakeKeyGrbit.NewKey);
            if (Api.TrySeek(_session, table, SeekGrbit.SeekEQ))
            {
                var columns = Api.GetTableColumns(_session, table);

                result = EseUtility.ConvertToAudioAlbum(_session, table, columns);
            }
            return result;
        }

        //public AudioAlbum GetAlbum(string title, string albumArtist)
        //{
        //    AudioAlbum result = default;

        //    using var table = new Table(_session, _databaseId, AlbumsTableName, OpenTableGrbit.ReadOnly);

        //    Api.JetSetCurrentIndex(_session, table, AlbumTitleIndexName);
        //    Api.MakeKey(_session, table, title, Encoding.Unicode, MakeKeyGrbit.NewKey);
        //    if (Api.TrySeek(_session, table, SeekGrbit.SeekEQ))
        //    {
        //        var columns = Api.GetTableColumns(_session, table);

        //        result = EseUtility.ConvertToAudioAlbum(_session, table, columns);
        //    }
        //    return result;
        //}

        internal void UpdateAlbumArt(int id, string url, bool force)
        {
            var value = url.ToCString();

            using var table = new Table(_session, _databaseId, AlbumsTableName, OpenTableGrbit.Updatable);

            Api.JetSetCurrentIndex(_session, table, AlbumIdIndexName);
            Api.MakeKey(_session, table, id, MakeKeyGrbit.NewKey);
            if (Api.TrySeek(_session, table, SeekGrbit.SeekEQ))
            {
                var columns = Api.GetTableColumns(_session, table);
                var imageUrlColumn = columns.FirstOrDefault(c => c.Name.Equals(nameof(AudioAlbum.ImageUrl), StringComparison.OrdinalIgnoreCase));
                var musicMatchImageUrlColumn = columns.FirstOrDefault(c => c.Name.Equals(nameof(AudioAlbum.MusicMatchImageUrl), StringComparison.OrdinalIgnoreCase));

                var currentValue = EseUtility.ConvertToString(_session, table, imageUrlColumn);
                if (currentValue == default || force)
                {
                    using var t = new Transaction(_session);

                    using (var u = new Update(_session, table, JET_prep.Replace))
                    {
                        Api.SetColumn(_session, table, imageUrlColumn.Columnid, value, Encoding.Unicode);
                        Api.SetColumn(_session, table, musicMatchImageUrlColumn.Columnid, null, Encoding.Unicode);

                        u.Save();
                    }
                    t.Commit(CommitTransactionGrbit.None);
                }
            }
        }

        internal IList<AudioAlbum> GetAlbums()
        {
            var result = new List<AudioAlbum>();

            using var table = new Table(_session, _databaseId, AlbumsTableName, OpenTableGrbit.ReadOnly);

            var columns = Api.GetTableColumns(_session, table);

            if (Api.TryMoveFirst(_session, table))
            {
                do
                {
                    var item = EseUtility.ConvertToAudioAlbum(_session, table, columns);

                    result.Add(item);

                } while (Api.TryMoveNext(_session, table));
            }

            return result;
        }

        internal IList<Person> GetPeople()
        {
            var result = new List<Person>();

            using var table = new Table(_session, _databaseId, PeopleTableName, OpenTableGrbit.ReadOnly);

            var columns = Api.GetTableColumns(_session, table);

            if (Api.TryMoveFirst(_session, table))
            {
                do
                {
                    var item = EseUtility.ConvertToPerson(_session, table, columns);

                    result.Add(item);

                } while (Api.TryMoveNext(_session, table));
            }

            return result;
        }

        internal void Close()
        {
            _instance?.Term();
        }

        public void Dispose()
        {
            Close();
        }


        internal static void RecoverDatabase(string path)
        {
            var pi = new ProcessStartInfo
            {
                FileName = "esentutl.exe",
                Arguments = "/r edb",
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = Path.GetDirectoryName(path)
            };
            using var t = new Process
            {
                StartInfo = pi
            };

            t.Start();
            t.WaitForExit();
        }
    }
}
