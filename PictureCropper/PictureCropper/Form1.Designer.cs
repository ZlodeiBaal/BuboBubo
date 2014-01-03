namespace PictureCropper
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PictureWindow = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureWindow)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureWindow
            // 
            this.PictureWindow.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.PictureWindow.Location = new System.Drawing.Point(3, 3);
            this.PictureWindow.Name = "PictureWindow";
            this.PictureWindow.Size = new System.Drawing.Size(1296, 776);
            this.PictureWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureWindow.TabIndex = 2;
            this.PictureWindow.TabStop = false;
            this.PictureWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureWindow_MouseDown);
            this.PictureWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureWindow_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1305, 786);
            this.Controls.Add(this.PictureWindow);
            this.Name = "Form1";
            this.Text = "CropPic";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.PictureWindow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox PictureWindow;
    }
}

