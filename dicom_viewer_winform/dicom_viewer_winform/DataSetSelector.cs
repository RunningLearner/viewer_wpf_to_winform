using dicom_viewer_winform.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace dicom_viewer_winform
{
    /// <summary>
    /// Simplified dataset loader inspired by the WPF DataSetSelector.
    /// Loads DICOM series from a folder or file using a simplified
    /// DICOM series extractor that does not rely on the WPF solution.
    /// </summary>
    public class DataSetSelector
    {
        private readonly ILogger _logger;
        public IEnumerable<Series>? Series { get; private set; }

        public DataSetSelector()
        {
            _logger = Logging.GetLogger<DataSetSelector>();
        }

        public void Open(string path)
        {
            IEnumerable<Series>? series = null;
            if (Directory.Exists(path))
            {
                _logger.LogInformation("User opened directory");
                series = SimpleDicomSeriesExtractor.ExtractSeriesFromDirectory(path);
            }
            else
            {
                if (string.Compare(Path.GetFileName(path), "DICOMDIR", true, CultureInfo.InvariantCulture) == 0)
                {
                    _logger.LogInformation("Opened dicomdir");
                    series = SimpleDicomSeriesExtractor.ExtractSeriesFromDicomDir(path);
                }
                else
                {
                    _logger.LogInformation("Opened single dicom file");
                    series = SimpleDicomSeriesExtractor.ExtractSeriesFromSingleFile(path);
                }
            }

            if (series == null || !series.Any())
            {
                _logger.LogWarning("Series null or empty");
                return;
            }

            foreach (var s in series.OfType<DicomSeries>())
            {
                var scan = DicomVolumeLoader.Load(s);
                s.Volume = scan?.Volume;
            }

            Series = series;
        }
    }
}
