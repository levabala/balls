namespace BallsModelDrawer
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTimeScale = new System.Windows.Forms.Label();
            this.trackBarTimeScale = new System.Windows.Forms.TrackBar();
            this.checkBoxLightMode = new System.Windows.Forms.CheckBox();
            this.labelFocusing = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTimeScale)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelTimeScale);
            this.panel1.Controls.Add(this.trackBarTimeScale);
            this.panel1.Controls.Add(this.checkBoxLightMode);
            this.panel1.Location = new System.Drawing.Point(997, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(207, 112);
            this.panel1.TabIndex = 0;
            // 
            // labelTimeScale
            // 
            this.labelTimeScale.AutoSize = true;
            this.labelTimeScale.Location = new System.Drawing.Point(4, 71);
            this.labelTimeScale.Name = "labelTimeScale";
            this.labelTimeScale.Size = new System.Drawing.Size(93, 16);
            this.labelTimeScale.TabIndex = 2;
            this.labelTimeScale.Text = "TimeScale: x1";
            // 
            // trackBarTimeScale
            // 
            this.trackBarTimeScale.Location = new System.Drawing.Point(4, 30);
            this.trackBarTimeScale.Maximum = 200;
            this.trackBarTimeScale.Name = "trackBarTimeScale";
            this.trackBarTimeScale.Size = new System.Drawing.Size(198, 58);
            this.trackBarTimeScale.TabIndex = 1;
            this.trackBarTimeScale.Value = 100;
            // 
            // checkBoxLightMode
            // 
            this.checkBoxLightMode.AutoSize = true;
            this.checkBoxLightMode.Location = new System.Drawing.Point(3, 3);
            this.checkBoxLightMode.Name = "checkBoxLightMode";
            this.checkBoxLightMode.Size = new System.Drawing.Size(89, 20);
            this.checkBoxLightMode.TabIndex = 0;
            this.checkBoxLightMode.Text = "lightMode";
            this.checkBoxLightMode.UseVisualStyleBackColor = true;
            // 
            // labelFocusing
            // 
            this.labelFocusing.AutoSize = true;
            this.labelFocusing.Location = new System.Drawing.Point(13, 12);
            this.labelFocusing.Name = "labelFocusing";
            this.labelFocusing.Size = new System.Drawing.Size(0, 16);
            this.labelFocusing.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1216, 771);
            this.Controls.Add(this.labelFocusing);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTimeScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBoxLightMode;
        private System.Windows.Forms.Label labelFocusing;
        private System.Windows.Forms.Label labelTimeScale;
        private System.Windows.Forms.TrackBar trackBarTimeScale;
    }
}

