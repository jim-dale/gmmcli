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
                    Utility.GetStringOption(args, "--audio-folder"),
                    Utility.GetStringOption(args, "--metadata-file", "audio-metadata.json"),
                    Utility.GetStringOption(args, "--searchPatterns", "**/*.mp3;**/*.flac")
                );

                _logger.LogInformation("{Options}", options.ToString());

                if (options.IsValid())
                {
                    var command = ActivatorUtilities.GetServiceOrCreateInstance<ExportMetadataCommand>(_serviceProvider);

                    command.Run(options);
                }
            }
            else if (args.Contains("set-albumcovers", StringComparer.OrdinalIgnoreCase))
            {
                var options = new SetAlbumCoversOptions(
                    Utility.GetStringOption(args, "--metadata-file", "audio-metadata.json")
                );

                _logger.LogInformation("{Options}", options.ToString());

                if (options.IsValid())
                {
                    var command = ActivatorUtilities.GetServiceOrCreateInstance<SetAlbumCoversCommand>(_serviceProvider);

                    command.Run(options);
                }
            }

            return result;
        }
    }
}
