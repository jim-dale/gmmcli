using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gmmcli
{
    internal class Worker
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public int Run(string[] args)
        {
            int result = 0;

            _logger.LogInformation("{args}", args);

            if (args.Contains("delete-images", StringComparer.OrdinalIgnoreCase))
            {
                var command = ActivatorUtilities.GetServiceOrCreateInstance<DeleteImagesCommand>(_serviceProvider);

                command.Run();
            }
            else if (args.Contains("show-groovedata", StringComparer.OrdinalIgnoreCase))
            {
                var command = ActivatorUtilities.GetServiceOrCreateInstance<ShowGrooveDataCommand>(_serviceProvider);

                command.Run();
            }
            else if (args.Contains("export-groovedata", StringComparer.OrdinalIgnoreCase))
            {
                var command = ActivatorUtilities.GetServiceOrCreateInstance<ExportGrooveDataCommand>(_serviceProvider);

                command.Run();
            }
            else if (args.Contains("export-metadata", StringComparer.OrdinalIgnoreCase))
            {
                var options = new ExportMetadataOptions(
                    Utility.GetAndExpandStringOption(args, "--audio-folder"),
                    Utility.GetAndExpandStringOption(args, "--metadata-file", "audio-metadata.json"),
                    Utility.GetStringOption(args, "--searchPatterns", "**/*.mp3;**/*.flac")
                );

                if (options.IsValid())
                {
                    var command = ActivatorUtilities.GetServiceOrCreateInstance<ExportMetadataCommand>(_serviceProvider);

                    command.Run(options);
                }
            }
            else if (args.Contains("set-albumcovers", StringComparer.OrdinalIgnoreCase))
            {
                var options = new SetAlbumCoversOptions(
                    Utility.GetAndExpandStringOption(args, "--metadata-file", "audio-metadata.json")
                );

                if (options.IsValid())
                {
                    var command = ActivatorUtilities.GetServiceOrCreateInstance<SetAlbumCoversCommand>(_serviceProvider);

                    command.Run(options);
                }
            }
            else
            {
                Console.WriteLine("gmmcli - Groove Music Manager command line interface");
                Console.WriteLine();
                Console.WriteLine("gmmcli <command> [<args>]");
                Console.WriteLine();
                Console.WriteLine("   export-metadata --audio-folder <folder> --metadata-file <JSON metadata file> --searchPatterns <file spec>");
                Console.WriteLine("      Scan a folder for albums and extract ID3 metadata.");
                Console.WriteLine("         --audio-folder   Folder containing the albums to be scanned (default is audio-metadata.json)");
                Console.WriteLine();
                Console.WriteLine("   set-albumcovers --metadata-file <JSON metadata file>");
                Console.WriteLine("      Load the album metadata file and apply the album cover");
                Console.WriteLine("      image stored for each album to the Groove Music database.");
                Console.WriteLine();
            }

            return result;
        }
    }
}
