using Entities;

namespace dicom_viewer_winform
{
    public class Scan
    {
        public Patient Patient { get; set; }
        public ImageSet Volume { get; set; }
    }
}
