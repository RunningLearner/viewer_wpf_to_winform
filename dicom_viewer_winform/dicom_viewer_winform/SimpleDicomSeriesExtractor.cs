using Dicom;
using dicom_viewer_winform.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dicom_viewer_winform
{
    public static class SimpleDicomSeriesExtractor
    {
        public static IEnumerable<DicomSeries> ExtractSeriesFromDirectory(string path)
        {
            var seriesMap = new Dictionary<string, DicomSeries>();
            foreach (var file in Directory.EnumerateFiles(path))
            {
                if (!DicomFile.HasValidHeader(file))
                    continue;

                var dicom = DicomFile.Open(file, FileReadOption.ReadLargeOnDemand);
                var uid = dicom.Dataset.GetSingleValueOrDefault(DicomTag.SeriesInstanceUID, string.Empty);
                if (string.IsNullOrEmpty(uid))
                    continue;

                if (!seriesMap.TryGetValue(uid, out var series))
                {
                    series = new DicomSeries
                    {
                        SeriesInstanceUid = uid,
                        SopClassUid = dicom.Dataset.GetSingleValueOrDefault(DicomTag.SOPClassUID, string.Empty),
                        Number = dicom.Dataset.GetSingleValueOrDefault(DicomTag.SeriesNumber, string.Empty)
                    };
                    seriesMap[uid] = series;
                }
                series.FileNames.Add(file);
                series.Files.Add(dicom);
            }

            foreach (var s in seriesMap.Values)
            {
                s.NumberOfImages = s.Files.Count;
                s.Is3D = s.Files.Count > 1;
            }

            return seriesMap.Values;
        }

        public static IEnumerable<DicomSeries> ExtractSeriesFromSingleFile(string path)
        {
            if (!DicomFile.HasValidHeader(path))
                return Enumerable.Empty<DicomSeries>();

            var dicom = DicomFile.Open(path, FileReadOption.ReadLargeOnDemand);
            var uid = dicom.Dataset.GetSingleValueOrDefault(DicomTag.SeriesInstanceUID, string.Empty);
            if (string.IsNullOrEmpty(uid))
                return Enumerable.Empty<DicomSeries>();

            var series = new DicomSeries
            {
                SeriesInstanceUid = uid,
                SopClassUid = dicom.Dataset.GetSingleValueOrDefault(DicomTag.SOPClassUID, string.Empty),
                Number = dicom.Dataset.GetSingleValueOrDefault(DicomTag.SeriesNumber, string.Empty),
                NumberOfImages = 1,
                Is3D = false
            };
            series.FileNames.Add(path);
            series.Files.Add(dicom);
            return new[] { series };
        }

        public static IEnumerable<DicomSeries> ExtractSeriesFromDicomDir(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(directory))
                return Enumerable.Empty<DicomSeries>();

            return ExtractSeriesFromDirectory(directory);
        }
    }
}
