using FellowOakDicom.Imaging;

using System.Linq;

namespace dicom_viewer_winform
{
    public partial class DicomViewerForm : Form
    {
        private Entities.DicomSeries? currentSeries;

        public DicomViewerForm()
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
                    var dicomImage = new FellowOakDicom.Imaging.DicomImage(middleFile);
                    var frame = dicomImage.NumberOfFrames > 1 ? dicomImage.NumberOfFrames / 2 : 0;
                    using var rendered = dicomImage.RenderImage(frame);
                    var bitmap = rendered.AsClonedBitmap();
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = bitmap;
                    currentSeries = firstSeries;
                }
            }
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            if (currentSeries != null && currentSeries.FileNames.Count > 0)
            {
                var middleFile = currentSeries.FileNames[currentSeries.FileNames.Count / 2];
                var dicomImage = new FellowOakDicom.Imaging.DicomImage(middleFile);
                var frame = dicomImage.NumberOfFrames > 1 ? dicomImage.NumberOfFrames / 2 : 0;
                using var rendered = dicomImage.RenderImage(frame);
                var bitmap = rendered.AsClonedBitmap();
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = bitmap;
            }
        }

        private void buttonMpr_Click(object sender, EventArgs e)
        {
            if (currentSeries?.Volume != null)
            {
                using var form = new MprViewerForm(currentSeries.Volume);
                form.ShowDialog(this);
            }
        }
    }
}
