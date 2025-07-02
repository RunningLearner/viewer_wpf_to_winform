using Dicom;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dicom_viewer_winform
{
    /// <summary>
    /// Simple loader that collects DICOM file paths from a folder.
    /// This avoids any dependency on the WPF viewer helpers.
    /// </summary>
    public class DataSetSelector
    {
        /// <summary>
        /// Gets the list of discovered DICOM file names.
        /// </summary>
        public IReadOnlyList<string> FileNames { get; private set; } = new List<string>();

        /// <summary>
        /// Scans the provided directory for DICOM files.
        /// </summary>
        /// <param name="folderPath">Folder containing DICOM images.</param>
        public void OpenFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                FileNames = new List<string>();
                return;
            }

            var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories)
                                  .Where(IsDicomFile)
                                  .ToList();

            FileNames = files;
        }

        private static bool IsDicomFile(string path)
        {
            try
            {
                using var _ = DicomFile.Open(path, FileReadOption.ReadHeader);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
