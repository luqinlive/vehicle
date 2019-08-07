namespace TIEVision.UI
{
    partial class UserControlPlayWnd
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.simpleBtnPrevsFrame = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonPlay = new DevExpress.XtraEditors.SimpleButton();
            this.simpleBtnNextFrame = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonStop = new DevExpress.XtraEditors.SimpleButton();
            this.trackBarControl1 = new DevExpress.XtraEditors.TrackBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(400, 400);
            this.panelControl1.TabIndex = 9;
            this.panelControl1.MouseEnter += new System.EventHandler(this.panelControl1_MouseEnter);
            this.panelControl1.MouseLeave += new System.EventHandler(this.panelControl1_MouseLeave);
            // 
            // panelControl2
            // 
            this.panelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl2.Controls.Add(this.simpleBtnPrevsFrame);
            this.panelControl2.Controls.Add(this.simpleButtonPlay);
            this.panelControl2.Controls.Add(this.simpleBtnNextFrame);
            this.panelControl2.Controls.Add(this.simpleButtonStop);
            this.panelControl2.Controls.Add(this.trackBarControl1);
            this.panelControl2.Location = new System.Drawing.Point(0, 368);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(400, 32);
            this.panelControl2.TabIndex = 10;
            // 
            // simpleBtnPrevsFrame
            // 
            this.simpleBtnPrevsFrame.AllowFocus = false;
            this.simpleBtnPrevsFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.simpleBtnPrevsFrame.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleBtnPrevsFrame.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleBtnPrevsFrame.Location = new System.Drawing.Point(62, 14);
            this.simpleBtnPrevsFrame.Name = "simpleBtnPrevsFrame";
            this.simpleBtnPrevsFrame.Size = new System.Drawing.Size(21, 15);
            this.simpleBtnPrevsFrame.TabIndex = 11;
            this.simpleBtnPrevsFrame.Click += new System.EventHandler(this.simpleBtnPrevsFrame_Click);
            // 
            // simpleButtonPlay
            // 
            this.simpleButtonPlay.AllowFocus = false;
            this.simpleButtonPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.simpleButtonPlay.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleButtonPlay.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonPlay.Location = new System.Drawing.Point(35, 14);
            this.simpleButtonPlay.Name = "simpleButtonPlay";
            this.simpleButtonPlay.Size = new System.Drawing.Size(21, 15);
            this.simpleButtonPlay.TabIndex = 9;
            this.simpleButtonPlay.Click += new System.EventHandler(this.simpleButtonPlay_Click);
            // 
            // simpleBtnNextFrame
            // 
            this.simpleBtnNextFrame.AllowFocus = false;
            this.simpleBtnNextFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.simpleBtnNextFrame.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleBtnNextFrame.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleBtnNextFrame.Location = new System.Drawing.Point(89, 14);
            this.simpleBtnNextFrame.Name = "simpleBtnNextFrame";
            this.simpleBtnNextFrame.Size = new System.Drawing.Size(21, 15);
            this.simpleBtnNextFrame.TabIndex = 12;
            this.simpleBtnNextFrame.Click += new System.EventHandler(this.simpleBtnNextFrame_Click);
            // 
            // simpleButtonStop
            // 
            this.simpleButtonStop.AllowFocus = false;
            this.simpleButtonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.simpleButtonStop.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleButtonStop.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonStop.Location = new System.Drawing.Point(8, 14);
            this.simpleButtonStop.Name = "simpleButtonStop";
            this.simpleButtonStop.Size = new System.Drawing.Size(21, 15);
            this.simpleButtonStop.TabIndex = 10;
            this.simpleButtonStop.Click += new System.EventHandler(this.simpleButtonStop_Click);
            // 
            // trackBarControl1
            // 
            this.trackBarControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarControl1.EditValue = null;
            this.trackBarControl1.Location = new System.Drawing.Point(3, -1);
            this.trackBarControl1.Name = "trackBarControl1";
            this.trackBarControl1.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.trackBarControl1.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.trackBarControl1.Size = new System.Drawing.Size(395, 45);
            this.trackBarControl1.TabIndex = 13;
            // 
            // UserControlPlayWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.Name = "UserControlPlayWnd";
            this.Size = new System.Drawing.Size(400, 400);
            this.Load += new System.EventHandler(this.UserControlPlayWnd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleBtnPrevsFrame;
        private DevExpress.XtraEditors.SimpleButton simpleButtonPlay;
        private DevExpress.XtraEditors.SimpleButton simpleBtnNextFrame;
        private DevExpress.XtraEditors.SimpleButton simpleButtonStop;
        private DevExpress.XtraEditors.TrackBarControl trackBarControl1;
    }
}
