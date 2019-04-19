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
            this.checkBoxShowFrame = new System.Windows.Forms.CheckBox();
            this.checkBoxShowVector = new System.Windows.Forms.CheckBox();
            this.labelTimeScale = new System.Windows.Forms.Label();
            this.trackBarTimeScale = new System.Windows.Forms.TrackBar();
            this.checkBoxShowCenter = new System.Windows.Forms.CheckBox();
            this.labelFocusing = new System.Windows.Forms.Label();
            this.labelBounceGate = new System.Windows.Forms.Label();
            this.trackBarBounceGate = new System.Windows.Forms.TrackBar();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTimeScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBounceGate)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelBounceGate);
            this.panel1.Controls.Add(this.trackBarBounceGate);
            this.panel1.Controls.Add(this.checkBoxShowFrame);
            this.panel1.Controls.Add(this.checkBoxShowVector);
            this.panel1.Controls.Add(this.labelTimeScale);
            this.panel1.Controls.Add(this.trackBarTimeScale);
            this.panel1.Controls.Add(this.checkBoxShowCenter);
            this.panel1.Location = new System.Drawing.Point(796, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(207, 231);
            this.panel1.TabIndex = 0;
            // 
            // checkBoxShowFrame
            // 
            this.checkBoxShowFrame.AutoSize = true;
            this.checkBoxShowFrame.Location = new System.Drawing.Point(3, 55);
            this.checkBoxShowFrame.Name = "checkBoxShowFrame";
            this.checkBoxShowFrame.Size = new System.Drawing.Size(100, 20);
            this.checkBoxShowFrame.TabIndex = 4;
            this.checkBoxShowFrame.Text = "showFrame";
            this.checkBoxShowFrame.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowVector
            // 
            this.checkBoxShowVector.AutoSize = true;
            this.checkBoxShowVector.Location = new System.Drawing.Point(3, 29);
            this.checkBoxShowVector.Name = "checkBoxShowVector";
            this.checkBoxShowVector.Size = new System.Drawing.Size(100, 20);
            this.checkBoxShowVector.TabIndex = 3;
            this.checkBoxShowVector.Text = "showVector";
            this.checkBoxShowVector.UseVisualStyleBackColor = true;
            // 
            // labelTimeScale
            // 
            this.labelTimeScale.AutoSize = true;
            this.labelTimeScale.Location = new System.Drawing.Point(4, 139);
            this.labelTimeScale.Name = "labelTimeScale";
            this.labelTimeScale.Size = new System.Drawing.Size(93, 16);
            this.labelTimeScale.TabIndex = 2;
            this.labelTimeScale.Text = "TimeScale: x1";
            // 
            // trackBarTimeScale
            // 
            this.trackBarTimeScale.Location = new System.Drawing.Point(4, 98);
            this.trackBarTimeScale.Maximum = 200;
            this.trackBarTimeScale.Name = "trackBarTimeScale";
            this.trackBarTimeScale.Size = new System.Drawing.Size(198, 58);
            this.trackBarTimeScale.TabIndex = 1;
            this.trackBarTimeScale.Value = 100;
            // 
            // checkBoxShowCenter
            // 
            this.checkBoxShowCenter.AutoSize = true;
            this.checkBoxShowCenter.Location = new System.Drawing.Point(3, 3);
            this.checkBoxShowCenter.Name = "checkBoxShowCenter";
            this.checkBoxShowCenter.Size = new System.Drawing.Size(100, 20);
            this.checkBoxShowCenter.TabIndex = 0;
            this.checkBoxShowCenter.Text = "showCenter";
            this.checkBoxShowCenter.UseVisualStyleBackColor = true;
            // 
            // labelFocusing
            // 
            this.labelFocusing.AutoSize = true;
            this.labelFocusing.Location = new System.Drawing.Point(13, 12);
            this.labelFocusing.Name = "labelFocusing";
            this.labelFocusing.Size = new System.Drawing.Size(0, 16);
            this.labelFocusing.TabIndex = 1;
            // 
            // labelBounceGate
            // 
            this.labelBounceGate.AutoSize = true;
            this.labelBounceGate.Location = new System.Drawing.Point(7, 203);
            this.labelBounceGate.Name = "labelBounceGate";
            this.labelBounceGate.Size = new System.Drawing.Size(113, 16);
            this.labelBounceGate.TabIndex = 6;
            this.labelBounceGate.Text = "BounceGate: 0.1s";
            // 
            // trackBarBounceGate
            // 
            this.trackBarBounceGate.Location = new System.Drawing.Point(7, 162);
            this.trackBarBounceGate.Maximum = 1000;
            this.trackBarBounceGate.Name = "trackBarBounceGate";
            this.trackBarBounceGate.Size = new System.Drawing.Size(198, 58);
            this.trackBarBounceGate.TabIndex = 5;
            this.trackBarBounceGate.Value = 100;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 771);
            this.Controls.Add(this.labelFocusing);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTimeScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBounceGate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBoxShowCenter;
        private System.Windows.Forms.Label labelFocusing;
        private System.Windows.Forms.Label labelTimeScale;
        private System.Windows.Forms.TrackBar trackBarTimeScale;
        private System.Windows.Forms.CheckBox checkBoxShowFrame;
        private System.Windows.Forms.CheckBox checkBoxShowVector;
        private System.Windows.Forms.Label labelBounceGate;
        private System.Windows.Forms.TrackBar trackBarBounceGate;
    }
}

