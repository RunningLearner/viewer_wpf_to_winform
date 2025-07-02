using FellowOakDicom;
using System.Collections.Generic;

namespace dicom_viewer_winform.Entities
{
    public class DicomSeries : Series
    {
        public List<DicomFile> Files { get; } = new();
        public List<string> FileNames { get; } = new();
    }
}
