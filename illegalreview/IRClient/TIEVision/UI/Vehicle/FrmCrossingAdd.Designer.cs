namespace IRVision.UI.Vehicle
{
    partial class FrmCrossingAdd
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
            this.simpleButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonConform = new DevExpress.XtraEditors.SimpleButton();
            this.textBox_CrossName = new DevExpress.XtraEditors.TextEdit();
            this.textBox_CrossId = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.textBox_CrossName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox_CrossId.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButtonCancel.Appearance.Options.UseFont = true;
            this.simpleButtonCancel.Location = new System.Drawing.Point(174, 160);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(75, 26);
            this.simpleButtonCancel.TabIndex = 7;
            this.simpleButtonCancel.Text = "取消";
            this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
            // 
            // simpleButtonConform
            // 
            this.simpleButtonConform.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButtonConform.Appearance.Options.UseFont = true;
            this.simpleButtonConform.Location = new System.Drawing.Point(59, 160);
            this.simpleButtonConform.Name = "simpleButtonConform";
            this.simpleButtonConform.Size = new System.Drawing.Size(75, 26);
            this.simpleButtonConform.TabIndex = 6;
            this.simpleButtonConform.Text = "确认";
            this.simpleButtonConform.Click += new System.EventHandler(this.simpleButtonConform_Click);
            // 
            // textBox_CrossName
            // 
            this.textBox_CrossName.Location = new System.Drawing.Point(106, 85);
            this.textBox_CrossName.Name = "textBox_CrossName";
            this.textBox_CrossName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_CrossName.Properties.Appearance.Options.UseFont = true;
            this.textBox_CrossName.Size = new System.Drawing.Size(158, 24);
            this.textBox_CrossName.TabIndex = 11;
            // 
            // textBox_CrossId
            // 
            this.textBox_CrossId.Location = new System.Drawing.Point(106, 55);
            this.textBox_CrossId.Name = "textBox_CrossId";
            this.textBox_CrossId.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_CrossId.Properties.Appearance.Options.UseFont = true;
            this.textBox_CrossId.Size = new System.Drawing.Size(158, 24);
            this.textBox_CrossId.TabIndex = 10;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Location = new System.Drawing.Point(30, 88);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(70, 17);
            this.labelControl2.TabIndex = 9;
            this.labelControl2.Text = "通道名称：";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl1.Location = new System.Drawing.Point(30, 58);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(70, 17);
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "通道编号：";
            // 
            // FrmCrossingAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 229);
            this.Controls.Add(this.textBox_CrossName);
            this.Controls.Add(this.textBox_CrossId);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.simpleButtonCancel);
            this.Controls.Add(this.simpleButtonConform);
            this.MaximizeBox = false;
            this.Name = "FrmCrossingAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "通道添加";
            this.Load += new System.EventHandler(this.FrmCrossingAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textBox_CrossName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox_CrossId.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButtonCancel;
        private DevExpress.XtraEditors.SimpleButton simpleButtonConform;
        private DevExpress.XtraEditors.TextEdit textBox_CrossName;
        private DevExpress.XtraEditors.TextEdit textBox_CrossId;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}