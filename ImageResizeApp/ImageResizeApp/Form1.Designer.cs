namespace ImageResizeApp
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
        private new void InitializeComponent()
        {
            this.OriginalPictureBox = new System.Windows.Forms.PictureBox();
            this.SequentialPictureBox = new System.Windows.Forms.PictureBox();
            this.ParallelPictureBox = new System.Windows.Forms.PictureBox();
            this.DownscaleFactorTextBox = new System.Windows.Forms.TextBox();
            this.OpenButton = new System.Windows.Forms.Button();
            this.DownscaleButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BetterQualityPictureBox = new System.Windows.Forms.PictureBox();
            this.BetterQualityLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.OriginalPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SequentialPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParallelPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BetterQualityPictureBox)).BeginInit();
            this.SuspendLayout();
            
            this.OriginalPictureBox.Location = new System.Drawing.Point(52, 12);
            this.OriginalPictureBox.Name = "OriginalPictureBox";
            this.OriginalPictureBox.Size = new System.Drawing.Size(200, 200);
            this.OriginalPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OriginalPictureBox.TabIndex = 0;
            this.OriginalPictureBox.TabStop = false;
            
            this.SequentialPictureBox.Location = new System.Drawing.Point(379, 12);
            this.SequentialPictureBox.Name = "SequentialPictureBox";
            this.SequentialPictureBox.Size = new System.Drawing.Size(200, 200);
            this.SequentialPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SequentialPictureBox.TabIndex = 1;
            this.SequentialPictureBox.TabStop = false;
            
            this.ParallelPictureBox.Location = new System.Drawing.Point(694, 12);
            this.ParallelPictureBox.Name = "ParallelPictureBox";
            this.ParallelPictureBox.Size = new System.Drawing.Size(200, 200);
            this.ParallelPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ParallelPictureBox.TabIndex = 2;
            this.ParallelPictureBox.TabStop = false;
            
            this.DownscaleFactorTextBox.Location = new System.Drawing.Point(337, 417);
            this.DownscaleFactorTextBox.Name = "DownscaleFactorTextBox";
            this.DownscaleFactorTextBox.Size = new System.Drawing.Size(190, 26);
            this.DownscaleFactorTextBox.TabIndex = 3;
            this.DownscaleFactorTextBox.Text = "Downscale size (%)";
           
            this.OpenButton.Location = new System.Drawing.Point(122, 468);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(166, 42);
            this.OpenButton.TabIndex = 4;
            this.OpenButton.Text = "Open Image";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            
            this.DownscaleButton.Location = new System.Drawing.Point(337, 468);
            this.DownscaleButton.Name = "DownscaleButton";
            this.DownscaleButton.Size = new System.Drawing.Size(189, 42);
            this.DownscaleButton.TabIndex = 5;
            this.DownscaleButton.Text = "Downscale";
            this.DownscaleButton.UseVisualStyleBackColor = true;
            this.DownscaleButton.Click += new System.EventHandler(this.DownscaleButton_Click);
            
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 236);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Original Image";
            
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(398, 236);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Consequential Image";
            
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(745, 236);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Parallel Image";
            
            this.BetterQualityPictureBox.Location = new System.Drawing.Point(1006, 12);
            this.BetterQualityPictureBox.Name = "BetterQualityPictureBox";
            this.BetterQualityPictureBox.Size = new System.Drawing.Size(200, 200);
            this.BetterQualityPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.BetterQualityPictureBox.TabIndex = 11;
            this.BetterQualityPictureBox.TabStop = false;
            
            this.BetterQualityLabel.AutoSize = true;
            this.BetterQualityLabel.Location = new System.Drawing.Point(1002, 236);
            this.BetterQualityLabel.Name = "BetterQualityLabel";
            this.BetterQualityLabel.Size = new System.Drawing.Size(356, 20);
            this.BetterQualityLabel.TabIndex = 12;
            this.BetterQualityLabel.Text = "Image without quality improvement (For Example)";
           
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1724, 705);
            this.Controls.Add(this.BetterQualityPictureBox);
            this.Controls.Add(this.BetterQualityLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DownscaleButton);
            this.Controls.Add(this.OpenButton);
            this.Controls.Add(this.DownscaleFactorTextBox);
            this.Controls.Add(this.ParallelPictureBox);
            this.Controls.Add(this.SequentialPictureBox);
            this.Controls.Add(this.OriginalPictureBox);
            this.Name = "Form1";
            this.Text = "Image Resize App";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OriginalPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SequentialPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParallelPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BetterQualityPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
