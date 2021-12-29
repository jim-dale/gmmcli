using Microsoft.Extensions.Logging;

namespace gmmcli
{
    internal class ExportMetadataCommand
    {
        private readonly AudioFilesService _provider;
        private readonly ILogger<ExportMetadataCommand> _logger;

        public ExportMetadataCommand(AudioFilesService provider, ILogger<ExportMetadataCommand> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        public int Run(ExportMetadataOptions options)
        {
            _logger.LogInformation("{Options}", options);

            var items = _provider.GetAlbumsFromFolder(options.Folder, options.SearchPatterns);

            Utility.SaveMetadataToJsonFile(items, options.MetadataPath);

            return 0;
        }
    }
}
