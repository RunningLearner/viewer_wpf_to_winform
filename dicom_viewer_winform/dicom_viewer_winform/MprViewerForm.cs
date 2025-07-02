using System.Drawing;
using System.Windows.Forms;
using Entities;

namespace dicom_viewer_winform
{
    public partial class MprViewerForm : Form
    {
        private readonly ImageSet _volume;

        public MprViewerForm(ImageSet volume)
        {
            _volume = volume;
            InitializeComponent();
            if (_volume != null)
            {
                pictureBoxAxial.Image = MprRenderer.GenerateAxial(_volume);
                pictureBoxSagittal.Image = MprRenderer.GenerateSagittal(_volume);
                pictureBoxCoronal.Image = MprRenderer.GenerateCoronal(_volume);
            }
        }
    }
}
