namespace TIEVision.UI.Vehicle
{
    partial class FrmMarking
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
            this.simpleButtonChoosePic = new DevExpress.XtraEditors.SimpleButton();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButtonRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonSave = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEditCross = new DevExpress.XtraEditors.ComboBoxEdit();
            this.pictureEdit4 = new DevExpress.XtraEditors.PictureEdit();
            this.pictureEdit3 = new DevExpress.XtraEditors.PictureEdit();
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.simpleBtnParams = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditCross.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButtonChoosePic
            // 
            this.simpleButtonChoosePic.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButtonChoosePic.Appearance.Options.UseFont = true;
            this.simpleButtonChoosePic.Location = new System.Drawing.Point(310, 26);
            this.simpleButtonChoosePic.Name = "simpleButtonChoosePic";
            this.simpleButtonChoosePic.Size = new System.Drawing.Size(84, 24);
            this.simpleButtonChoosePic.TabIndex = 2;
            this.simpleButtonChoosePic.Text = "选择图片";
            this.simpleButtonChoosePic.Click += new System.EventHandler(this.simpleButtonChoosePic_Click);
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Location = new System.Drawing.Point(13, 62);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.AllowFocused = false;
            this.pictureEdit1.Properties.ShowMenu = false;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit1.Size = new System.Drawing.Size(518, 339);
            this.pictureEdit1.TabIndex = 3;
            this.pictureEdit1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureEdit1_Paint);
            this.pictureEdit1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureEdit1_MouseDown);
            this.pictureEdit1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureEdit1_MouseUp);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleBtnParams);
            this.panelControl1.Controls.Add(this.simpleButtonRefresh);
            this.panelControl1.Controls.Add(this.simpleButtonSave);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.comboBoxEditCross);
            this.panelControl1.Controls.Add(this.pictureEdit4);
            this.panelControl1.Controls.Add(this.pictureEdit3);
            this.panelControl1.Controls.Add(this.pictureEdit2);
            this.panelControl1.Controls.Add(this.pictureEdit1);
            this.panelControl1.Controls.Add(this.simpleButtonChoosePic);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1070, 803);
            this.panelControl1.TabIndex = 4;
            this.panelControl1.Resize += new System.EventHandler(this.panelControl1_Resize);
            // 
            // simpleButtonRefresh
            // 
            this.simpleButtonRefresh.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButtonRefresh.Appearance.Options.UseFont = true;
            this.simpleButtonRefresh.Location = new System.Drawing.Point(490, 26);
            this.simpleButtonRefresh.Name = "simpleButtonRefresh";
            this.simpleButtonRefresh.Size = new System.Drawing.Size(84, 24);
            this.simpleButtonRefresh.TabIndex = 14;
            this.simpleButtonRefresh.Text = "刷新";
            this.simpleButtonRefresh.Click += new System.EventHandler(this.simpleButtonRefresh_Click);
            // 
            // simpleButtonSave
            // 
            this.simpleButtonSave.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButtonSave.Appearance.Options.UseFont = true;
            this.simpleButtonSave.Location = new System.Drawing.Point(400, 26);
            this.simpleButtonSave.Name = "simpleButtonSave";
            this.simpleButtonSave.Size = new System.Drawing.Size(84, 24);
            this.simpleButtonSave.TabIndex = 13;
            this.simpleButtonSave.Text = "保存";
            this.simpleButtonSave.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(16, 29);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(33, 17);
            this.labelControl2.TabIndex = 10;
            this.labelControl2.Text = "通道:";
            // 
            // comboBoxEditCross
            // 
            this.comboBoxEditCross.Location = new System.Drawing.Point(54, 26);
            this.comboBoxEditCross.Name = "comboBoxEditCross";
            this.comboBoxEditCross.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxEditCross.Properties.Appearance.Options.UseFont = true;
            this.comboBoxEditCross.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditCross.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEditCross.Size = new System.Drawing.Size(160, 24);
            this.comboBoxEditCross.TabIndex = 9;
            this.comboBoxEditCross.SelectedIndexChanged += new System.EventHandler(this.comboBoxEditCross_SelectedIndexChanged);
            // 
            // pictureEdit4
            // 
            this.pictureEdit4.Location = new System.Drawing.Point(540, 413);
            this.pictureEdit4.Name = "pictureEdit4";
            this.pictureEdit4.Properties.AllowFocused = false;
            this.pictureEdit4.Properties.ShowMenu = false;
            this.pictureEdit4.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit4.Size = new System.Drawing.Size(518, 339);
            this.pictureEdit4.TabIndex = 6;
            // 
            // pictureEdit3
            // 
            this.pictureEdit3.Location = new System.Drawing.Point(13, 413);
            this.pictureEdit3.Name = "pictureEdit3";
            this.pictureEdit3.Properties.AllowFocused = false;
            this.pictureEdit3.Properties.ShowMenu = false;
            this.pictureEdit3.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit3.Size = new System.Drawing.Size(518, 339);
            this.pictureEdit3.TabIndex = 5;
            this.pictureEdit3.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureEdit3_Paint);
            // 
            // pictureEdit2
            // 
            this.pictureEdit2.Location = new System.Drawing.Point(540, 62);
            this.pictureEdit2.Name = "pictureEdit2";
            this.pictureEdit2.Properties.AllowFocused = false;
            this.pictureEdit2.Properties.ShowMenu = false;
            this.pictureEdit2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit2.Size = new System.Drawing.Size(518, 339);
            this.pictureEdit2.TabIndex = 4;
            this.pictureEdit2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureEdit2_Paint);
            // 
            // simpleBtnParams
            // 
            this.simpleBtnParams.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleBtnParams.Appearance.Options.UseFont = true;
            this.simpleBtnParams.Location = new System.Drawing.Point(220, 26);
            this.simpleBtnParams.Name = "simpleBtnParams";
            this.simpleBtnParams.Size = new System.Drawing.Size(84, 24);
            this.simpleBtnParams.TabIndex = 16;
            this.simpleBtnParams.Text = "参数设置";
            this.simpleBtnParams.Click += new System.EventHandler(this.simpleBtnParams_Click);
            // 
            // FrmMarking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 803);
            this.Controls.Add(this.panelControl1);
            this.Name = "FrmMarking";
            this.Text = "FrmMarking";
            this.Load += new System.EventHandler(this.FrmMarking_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditCross.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButtonChoosePic;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit2;
        private DevExpress.XtraEditors.PictureEdit pictureEdit4;
        private DevExpress.XtraEditors.PictureEdit pictureEdit3;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditCross;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButtonSave;
        private DevExpress.XtraEditors.SimpleButton simpleButtonRefresh;
        private DevExpress.XtraEditors.SimpleButton simpleBtnParams;
    }
}