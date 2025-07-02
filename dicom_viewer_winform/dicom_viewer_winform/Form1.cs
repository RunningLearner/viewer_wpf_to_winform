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
                // In a full implementation the loaded series would be
                // displayed using embedded WPF controls.
            }
        }
    }
}
