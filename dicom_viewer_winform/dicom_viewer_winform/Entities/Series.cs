using System;
using System.Drawing;

namespace dicom_viewer_winform.Entities
{
    public class Series
    {
        public string SeriesInstanceUid { get; set; } = string.Empty;
        public string SopClassUid { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public int NumberOfImages { get; set; }
        public DateTime Time { get; set; }
        public bool Is3D { get; set; }
        public Image? Thumbnail { get; set; }
    }
}
