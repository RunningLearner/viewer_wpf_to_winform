using Dicom.Imaging;
using System.Linq;

namespace dicom_viewer_winform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Text = dialog.SelectedPath;

                var loader = new DataSetSelector();
                loader.Open(dialog.SelectedPath);

                var firstSeries = loader.Series?.FirstOrDefault() as dicom_viewer_winform.Entities.DicomSeries;
                if (firstSeries != null && firstSeries.FileNames.Count > 0)
                {
                    var middleFile = firstSeries.FileNames[firstSeries.FileNames.Count / 2];
                    var dicomImage = new Dicom.Imaging.DicomImage(middleFile);
                    var frame = dicomImage.NumberOfFrames > 1 ? dicomImage.NumberOfFrames / 2 : 0;
                    using var rendered = dicomImage.RenderImage(frame);
                    var bitmap = rendered.AsClonedBitmap();
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = bitmap;
                }
            }
        }
    }
}
