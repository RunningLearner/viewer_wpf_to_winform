namespace dicom_viewer_winform
{
    partial class DicomViewerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonHome;
        private System.Windows.Forms.Button buttonMpr;
        private System.Windows.Forms.Button buttonMprVtk;

        private void InitializeComponent()
        {
            this.buttonOpen = new System.Windows.Forms.Button();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonHome = new System.Windows.Forms.Button();
            this.buttonMpr = new System.Windows.Forms.Button();
            this.buttonMprVtk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(12, 12);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(94, 29);
            this.buttonOpen.TabIndex = 0;
            this.buttonOpen.Text = "Open Folder";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            //
            // buttonHome
            //
            this.buttonHome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonHome.Location = new System.Drawing.Point(12, 415);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(94, 29);
            this.buttonHome.TabIndex = 3;
            this.buttonHome.Text = "Home";
            this.buttonHome.UseVisualStyleBackColor = true;
            this.buttonHome.Click += new System.EventHandler(this.buttonHome_Click);
            //
            // buttonMpr
            //
            this.buttonMpr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMpr.Location = new System.Drawing.Point(112, 415);
            this.buttonMpr.Name = "buttonMpr";
            this.buttonMpr.Size = new System.Drawing.Size(94, 29);
            this.buttonMpr.TabIndex = 4;
            this.buttonMpr.Text = "MPR";
            this.buttonMpr.UseVisualStyleBackColor = true;
            this.buttonMpr.Click += new System.EventHandler(this.buttonMpr_Click);
            //
            // buttonMprVtk
            //
            this.buttonMprVtk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMprVtk.Location = new System.Drawing.Point(212, 415);
            this.buttonMprVtk.Name = "buttonMprVtk";
            this.buttonMprVtk.Size = new System.Drawing.Size(94, 29);
            this.buttonMprVtk.TabIndex = 5;
            this.buttonMprVtk.Text = "MPR VTK";
            this.buttonMprVtk.UseVisualStyleBackColor = true;
            this.buttonMprVtk.Click += new System.EventHandler(this.buttonMprVtk_Click);
            //
            // pictureBox1
            //
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                        | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(12, 47);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(776, 362);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // textBoxPath
            // 
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.Location = new System.Drawing.Point(112, 14);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.ReadOnly = true;
            this.textBoxPath.Size = new System.Drawing.Size(676, 27);
            this.textBoxPath.TabIndex = 1;
            // 
            // DicomViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonMprVtk);
            this.Controls.Add(this.buttonMpr);
            this.Controls.Add(this.buttonHome);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.buttonOpen);
            this.Name = "DicomViewerForm";
            this.Text = "DICOM Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
