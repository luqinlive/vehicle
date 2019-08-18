namespace IRVision.UI.Vehicle
{
    partial class FrmMarkingTraffic
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
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.simpleBtnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.simpleBtnSave = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Location = new System.Drawing.Point(8, 4);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.AllowFocused = false;
            this.pictureEdit1.Properties.ShowMenu = false;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit1.Size = new System.Drawing.Size(818, 493);
            this.pictureEdit1.TabIndex = 0;
            this.pictureEdit1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureEdit1_Paint);
            this.pictureEdit1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureEdit1_MouseDown);
            // 
            // simpleBtnCancel
            // 
            this.simpleBtnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleBtnCancel.Appearance.Options.UseFont = true;
            this.simpleBtnCancel.Location = new System.Drawing.Point(411, 514);
            this.simpleBtnCancel.Name = "simpleBtnCancel";
            this.simpleBtnCancel.Size = new System.Drawing.Size(75, 23);
            this.simpleBtnCancel.TabIndex = 52;
            this.simpleBtnCancel.Text = "取消";
            this.simpleBtnCancel.Click += new System.EventHandler(this.simpleBtnCancel_Click);
            // 
            // simpleBtnSave
            // 
            this.simpleBtnSave.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleBtnSave.Appearance.Options.UseFont = true;
            this.simpleBtnSave.Location = new System.Drawing.Point(309, 514);
            this.simpleBtnSave.Name = "simpleBtnSave";
            this.simpleBtnSave.Size = new System.Drawing.Size(75, 23);
            this.simpleBtnSave.TabIndex = 51;
            this.simpleBtnSave.Text = "保存";
            this.simpleBtnSave.Click += new System.EventHandler(this.simpleBtnSave_Click);
            // 
            // FrmMarkingTraffic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 549);
            this.Controls.Add(this.simpleBtnCancel);
            this.Controls.Add(this.simpleBtnSave);
            this.Controls.Add(this.pictureEdit1);
            this.MaximizeBox = false;
            this.Name = "FrmMarkingTraffic";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "红绿灯调整";
            this.Load += new System.EventHandler(this.FrmMarkingTraffic_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleBtnCancel;
        private DevExpress.XtraEditors.SimpleButton simpleBtnSave;
    }
}