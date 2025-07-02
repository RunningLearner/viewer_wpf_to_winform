using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Entities;
using RenderEngine;

namespace dicom_viewer_winform
{
    public partial class MprVtkViewerForm : Form
    {
        private readonly ImageSet _volume;

        public MprVtkViewerForm(ImageSet volume)
        {
            _volume = volume;
            InitializeComponent();
            if (_volume != null)
            {
                ShowMpr();
            }
        }

        private static Bitmap RenderViewport(ViewportRenderer viewport, int width, int height)
        {
            byte[] buffer = new byte[width * height * 4];
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                viewport.SetSize(width, height);
                viewport.Render(handle.AddrOfPinnedObject());
                var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                var data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
                Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);
                bmp.UnlockBits(data);
                return bmp;
            }
            finally
            {
                handle.Free();
            }
        }

        private void ShowMpr()
        {
            var axial = new ViewportRenderer();
            var visAxial = new SlabVisual(_volume);
            visAxial.AddTo(axial);
            axial.SetCameraTransformation(new Matrix());
            axial.SetZoom(_volume.DimensionsInPatientSpace.Y * 0.5);
            pictureBoxAxial.Image = RenderViewport(axial, pictureBoxAxial.Width, pictureBoxAxial.Height);

            var sagittal = new ViewportRenderer();
            var visSagittal = new SlabVisual(_volume);
            visSagittal.AddTo(sagittal);
            var sagTransform = Matrix.RotationAngleAxis(-Math.PI / 2, new Vector3(0, 0, 1)) *
                               Matrix.RotationAngleAxis(-Math.PI / 2, new Vector3(1, 0, 0));
            sagittal.SetCameraTransformation(sagTransform);
            sagittal.SetZoom(_volume.DimensionsInPatientSpace.Z * 0.5);
            pictureBoxSagittal.Image = RenderViewport(sagittal, pictureBoxSagittal.Width, pictureBoxSagittal.Height);

            var coronal = new ViewportRenderer();
            var visCoronal = new SlabVisual(_volume);
            visCoronal.AddTo(coronal);
            var corTransform = Matrix.RotationAngleAxis(-Math.PI / 2, new Vector3(1, 0, 0));
            coronal.SetCameraTransformation(corTransform);
            coronal.SetZoom(_volume.DimensionsInPatientSpace.Z * 0.5);
            pictureBoxCoronal.Image = RenderViewport(coronal, pictureBoxCoronal.Width, pictureBoxCoronal.Height);
        }
    }
}
