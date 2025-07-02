using System.Windows.Forms;

namespace dicom_viewer_winform
{
    partial class MprVtkViewerForm
    {
        private System.ComponentModel.IContainer components = null;
        private PictureBox pictureBoxAxial;
        private PictureBox pictureBoxSagittal;
        private PictureBox pictureBoxCoronal;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pictureBoxAxial = new PictureBox();
            this.pictureBoxSagittal = new PictureBox();
            this.pictureBoxCoronal = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAxial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSagittal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCoronal)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxAxial
            // 
            this.pictureBoxAxial.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxAxial.Name = "pictureBoxAxial";
            this.pictureBoxAxial.Size = new System.Drawing.Size(256, 256);
            this.pictureBoxAxial.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxAxial.TabIndex = 0;
            this.pictureBoxAxial.TabStop = false;
            // 
            // pictureBoxSagittal
            // 
            this.pictureBoxSagittal.Location = new System.Drawing.Point(12, 274);
            this.pictureBoxSagittal.Name = "pictureBoxSagittal";
            this.pictureBoxSagittal.Size = new System.Drawing.Size(256, 256);
            this.pictureBoxSagittal.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxSagittal.TabIndex = 1;
            this.pictureBoxSagittal.TabStop = false;
            // 
            // pictureBoxCoronal
            // 
            this.pictureBoxCoronal.Location = new System.Drawing.Point(274, 12);
            this.pictureBoxCoronal.Name = "pictureBoxCoronal";
            this.pictureBoxCoronal.Size = new System.Drawing.Size(256, 256);
            this.pictureBoxCoronal.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxCoronal.TabIndex = 2;
            this.pictureBoxCoronal.TabStop = false;
            // 
            // MprVtkViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 542);
            this.Controls.Add(this.pictureBoxCoronal);
            this.Controls.Add(this.pictureBoxSagittal);
            this.Controls.Add(this.pictureBoxAxial);
            this.Name = "MprVtkViewerForm";
            this.Text = "MPR Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAxial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSagittal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCoronal)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
