using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace gmmcli
{
    internal static class Utility
    {
        public static string GetStringOption(string[] args, string argName, string defaultValue = null)
        {
            return GetStringOption(args, argName, false, defaultValue);
        }

        public static string GetAndExpandStringOption(string[] args, string argName, string defaultValue = null)
        {
            return GetStringOption(args, argName, true, defaultValue);
        }

        public static string GetStringOption(string[] args, string argName, bool expandEnvironmentVariables, string defaultValue = null)
        {
            string result = defaultValue;

            int index = Array.FindIndex(args, i => string.Equals(i, argName, StringComparison.OrdinalIgnoreCase));
            if (index >= 0 && index < (args.Length) - 1)
            {
                result = args[index + 1];

                if (expandEnvironmentVariables && string.IsNullOrWhiteSpace(result) == false)
                {
                    result = Environment.ExpandEnvironmentVariables(result);
                }
            }

            return result;
        }

        public static void SaveMetadataToJsonFile(IList<AlbumContext> items, string path)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(path, json);
        }

        public static IList<AlbumContext> LoadMetadataFromJsonFile(string path)
        {
            var result = new List<AlbumContext>();

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);

                result = JsonSerializer.Deserialize<List<AlbumContext>>(json);
            }

            return result;
        }
    }
}
