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
            using var dialog = new OpenFileDialog();
            dialog.Filter = "All files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Text = dialog.FileName;
                // TODO: load the selected file using WPF components
            }
        }
    }
}
