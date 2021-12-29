using System.IO;

namespace gmmcli
{
    internal record ExportMetadataOptions(string Folder, string MetadataPath, string SearchPatterns);
    internal static class ExportMetadataOptionsExtensions
    {
        public static bool IsValid(this ExportMetadataOptions item)
        {
            return string.IsNullOrWhiteSpace(item.Folder) == false
                && string.IsNullOrWhiteSpace(item.MetadataPath) == false
                && string.IsNullOrWhiteSpace(item.SearchPatterns) == false
                && Directory.Exists(item.Folder);
        }
    }

    internal record SetAlbumCoversOptions(string MetadataPath);
    internal static class SetAlbumCoversOptionsExtensions
    {
        public static bool IsValid(this SetAlbumCoversOptions item)
        {
            return string.IsNullOrWhiteSpace(item.MetadataPath) == false;
        }
    }
}
