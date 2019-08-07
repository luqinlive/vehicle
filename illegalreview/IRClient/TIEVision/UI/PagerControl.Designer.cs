namespace TIEVision.UI
{
    partial class PagerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PagerControl));
            this.btnGo = new DevExpress.XtraEditors.SimpleButton();
            this.txtPageNum = new DevExpress.XtraEditors.TextEdit();
            this.lnkLast = new System.Windows.Forms.LinkLabel();
            this.lnkNext = new System.Windows.Forms.LinkLabel();
            this.lnkPrev = new System.Windows.Forms.LinkLabel();
            this.lnkFirst = new System.Windows.Forms.LinkLabel();
            this.lblCurrentPage = new DevExpress.XtraEditors.LabelControl();
            this.lblMsg2 = new DevExpress.XtraEditors.LabelControl();
            this.lblTotalCount = new DevExpress.XtraEditors.LabelControl();
            this.lblMsg1 = new DevExpress.XtraEditors.LabelControl();
            this.lblMsg3 = new DevExpress.XtraEditors.LabelControl();
            this.lblRecPerPg = new DevExpress.XtraEditors.LabelControl();
            this.lblMsg4 = new DevExpress.XtraEditors.LabelControl();
            this.lblSept = new DevExpress.XtraEditors.LabelControl();
            this.lblPageCount = new DevExpress.XtraEditors.LabelControl();
            this.simpBtnFirst = new DevExpress.XtraEditors.SimpleButton();
            this.simpBtnPrev = new DevExpress.XtraEditors.SimpleButton();
            this.simpBtnNext = new DevExpress.XtraEditors.SimpleButton();
            this.simpBtnLast = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtPageNum.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGo
            // 
            this.btnGo.Image = ((System.Drawing.Image)(resources.GetObject("btnGo.Image")));
            this.btnGo.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnGo.Location = new System.Drawing.Point(260, 3);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(24, 27);
            this.btnGo.TabIndex = 48;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtPageNum
            // 
            this.txtPageNum.Location = new System.Drawing.Point(210, 7);
            this.txtPageNum.Name = "txtPageNum";
            this.txtPageNum.Size = new System.Drawing.Size(44, 20);
            this.txtPageNum.TabIndex = 46;
            this.txtPageNum.TextChanged += new System.EventHandler(this.txtPageNum_TextChanged);
            this.txtPageNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPageNum_KeyPress);
            // 
            // lnkLast
            // 
            this.lnkLast.AutoSize = true;
            this.lnkLast.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lnkLast.LinkColor = System.Drawing.Color.Black;
            this.lnkLast.Location = new System.Drawing.Point(873, 12);
            this.lnkLast.Name = "lnkLast";
            this.lnkLast.Size = new System.Drawing.Size(29, 12);
            this.lnkLast.TabIndex = 45;
            this.lnkLast.TabStop = true;
            this.lnkLast.Text = "尾页";
            this.lnkLast.Visible = false;
            this.lnkLast.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLast_LinkClicked);
            // 
            // lnkNext
            // 
            this.lnkNext.AutoSize = true;
            this.lnkNext.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lnkNext.LinkColor = System.Drawing.Color.Black;
            this.lnkNext.Location = new System.Drawing.Point(819, 12);
            this.lnkNext.Name = "lnkNext";
            this.lnkNext.Size = new System.Drawing.Size(41, 12);
            this.lnkNext.TabIndex = 44;
            this.lnkNext.TabStop = true;
            this.lnkNext.Text = "下一页";
            this.lnkNext.Visible = false;
            this.lnkNext.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNext_LinkClicked);
            // 
            // lnkPrev
            // 
            this.lnkPrev.AutoSize = true;
            this.lnkPrev.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lnkPrev.LinkColor = System.Drawing.Color.Black;
            this.lnkPrev.Location = new System.Drawing.Point(764, 12);
            this.lnkPrev.Name = "lnkPrev";
            this.lnkPrev.Size = new System.Drawing.Size(41, 12);
            this.lnkPrev.TabIndex = 43;
            this.lnkPrev.TabStop = true;
            this.lnkPrev.Text = "上一页";
            this.lnkPrev.Visible = false;
            this.lnkPrev.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPrev_LinkClicked);
            // 
            // lnkFirst
            // 
            this.lnkFirst.AutoSize = true;
            this.lnkFirst.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lnkFirst.LinkColor = System.Drawing.Color.Black;
            this.lnkFirst.Location = new System.Drawing.Point(723, 12);
            this.lnkFirst.Name = "lnkFirst";
            this.lnkFirst.Size = new System.Drawing.Size(29, 12);
            this.lnkFirst.TabIndex = 42;
            this.lnkFirst.TabStop = true;
            this.lnkFirst.Text = "首页";
            this.lnkFirst.Visible = false;
            this.lnkFirst.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFirst_LinkClicked);
            // 
            // lblCurrentPage
            // 
            this.lblCurrentPage.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblCurrentPage.Location = new System.Drawing.Point(326, 10);
            this.lblCurrentPage.Name = "lblCurrentPage";
            this.lblCurrentPage.Size = new System.Drawing.Size(7, 14);
            this.lblCurrentPage.TabIndex = 49;
            this.lblCurrentPage.Text = "1";
            // 
            // lblMsg2
            // 
            this.lblMsg2.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblMsg2.Location = new System.Drawing.Point(530, 10);
            this.lblMsg2.Name = "lblMsg2";
            this.lblMsg2.Size = new System.Drawing.Size(36, 14);
            this.lblMsg2.TabIndex = 50;
            this.lblMsg2.Text = "条记录";
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblTotalCount.Location = new System.Drawing.Point(448, 10);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(7, 14);
            this.lblTotalCount.TabIndex = 51;
            this.lblTotalCount.Text = "1";
            // 
            // lblMsg1
            // 
            this.lblMsg1.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblMsg1.Location = new System.Drawing.Point(421, 10);
            this.lblMsg1.Name = "lblMsg1";
            this.lblMsg1.Size = new System.Drawing.Size(12, 14);
            this.lblMsg1.TabIndex = 52;
            this.lblMsg1.Text = "共";
            // 
            // lblMsg3
            // 
            this.lblMsg3.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblMsg3.Location = new System.Drawing.Point(585, 10);
            this.lblMsg3.Name = "lblMsg3";
            this.lblMsg3.Size = new System.Drawing.Size(24, 14);
            this.lblMsg3.TabIndex = 53;
            this.lblMsg3.Text = "每页";
            // 
            // lblRecPerPg
            // 
            this.lblRecPerPg.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblRecPerPg.Location = new System.Drawing.Point(626, 10);
            this.lblRecPerPg.Name = "lblRecPerPg";
            this.lblRecPerPg.Size = new System.Drawing.Size(7, 14);
            this.lblRecPerPg.TabIndex = 54;
            this.lblRecPerPg.Text = "1";
            // 
            // lblMsg4
            // 
            this.lblMsg4.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblMsg4.Location = new System.Drawing.Point(645, 10);
            this.lblMsg4.Name = "lblMsg4";
            this.lblMsg4.Size = new System.Drawing.Size(12, 14);
            this.lblMsg4.TabIndex = 55;
            this.lblMsg4.Text = "条";
            // 
            // lblSept
            // 
            this.lblSept.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblSept.Location = new System.Drawing.Point(355, 10);
            this.lblSept.Name = "lblSept";
            this.lblSept.Size = new System.Drawing.Size(5, 14);
            this.lblSept.TabIndex = 56;
            this.lblSept.Text = "/";
            // 
            // lblPageCount
            // 
            this.lblPageCount.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblPageCount.Location = new System.Drawing.Point(370, 10);
            this.lblPageCount.Name = "lblPageCount";
            this.lblPageCount.Size = new System.Drawing.Size(7, 14);
            this.lblPageCount.TabIndex = 57;
            this.lblPageCount.Text = "1";
            // 
            // simpBtnFirst
            // 
            this.simpBtnFirst.Image = ((System.Drawing.Image)(resources.GetObject("simpBtnFirst.Image")));
            this.simpBtnFirst.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpBtnFirst.Location = new System.Drawing.Point(23, 5);
            this.simpBtnFirst.Name = "simpBtnFirst";
            this.simpBtnFirst.Size = new System.Drawing.Size(25, 25);
            this.simpBtnFirst.TabIndex = 58;
            this.simpBtnFirst.Text = "first";
            this.simpBtnFirst.Click += new System.EventHandler(this.simpBtnFirst_Click);
            // 
            // simpBtnPrev
            // 
            this.simpBtnPrev.Image = ((System.Drawing.Image)(resources.GetObject("simpBtnPrev.Image")));
            this.simpBtnPrev.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpBtnPrev.Location = new System.Drawing.Point(57, 5);
            this.simpBtnPrev.Name = "simpBtnPrev";
            this.simpBtnPrev.Size = new System.Drawing.Size(25, 25);
            this.simpBtnPrev.TabIndex = 59;
            this.simpBtnPrev.Text = "prev";
            this.simpBtnPrev.Click += new System.EventHandler(this.simpBtnPrev_Click);
            // 
            // simpBtnNext
            // 
            this.simpBtnNext.Image = ((System.Drawing.Image)(resources.GetObject("simpBtnNext.Image")));
            this.simpBtnNext.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpBtnNext.Location = new System.Drawing.Point(91, 5);
            this.simpBtnNext.Name = "simpBtnNext";
            this.simpBtnNext.Size = new System.Drawing.Size(25, 25);
            this.simpBtnNext.TabIndex = 60;
            this.simpBtnNext.Text = "next";
            this.simpBtnNext.Click += new System.EventHandler(this.simpBtnNext_Click);
            // 
            // simpBtnLast
            // 
            this.simpBtnLast.Image = ((System.Drawing.Image)(resources.GetObject("simpBtnLast.Image")));
            this.simpBtnLast.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpBtnLast.Location = new System.Drawing.Point(125, 5);
            this.simpBtnLast.Name = "simpBtnLast";
            this.simpBtnLast.Size = new System.Drawing.Size(25, 25);
            this.simpBtnLast.TabIndex = 61;
            this.simpBtnLast.Text = "last";
            this.simpBtnLast.Click += new System.EventHandler(this.simpBtnLast_Click);
            // 
            // PagerControl
            // 
            this.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(78)))), ((int)(((byte)(151)))));
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.simpBtnLast);
            this.Controls.Add(this.simpBtnNext);
            this.Controls.Add(this.simpBtnPrev);
            this.Controls.Add(this.simpBtnFirst);
            this.Controls.Add(this.lblPageCount);
            this.Controls.Add(this.lblSept);
            this.Controls.Add(this.lblMsg4);
            this.Controls.Add(this.lblRecPerPg);
            this.Controls.Add(this.lblMsg3);
            this.Controls.Add(this.lblMsg1);
            this.Controls.Add(this.lblTotalCount);
            this.Controls.Add(this.lblMsg2);
            this.Controls.Add(this.lblCurrentPage);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.txtPageNum);
            this.Controls.Add(this.lnkLast);
            this.Controls.Add(this.lnkNext);
            this.Controls.Add(this.lnkPrev);
            this.Controls.Add(this.lnkFirst);
            this.LookAndFeel.SkinName = "Visual Studio 2013 Dark";
            this.Name = "PagerControl";
            this.Size = new System.Drawing.Size(1015, 38);
            ((System.ComponentModel.ISupportInitialize)(this.txtPageNum.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnGo;
        private DevExpress.XtraEditors.TextEdit txtPageNum;
        private System.Windows.Forms.LinkLabel lnkLast;
        private System.Windows.Forms.LinkLabel lnkNext;
        private System.Windows.Forms.LinkLabel lnkPrev;
        private System.Windows.Forms.LinkLabel lnkFirst;
        private DevExpress.XtraEditors.LabelControl lblCurrentPage;
        private DevExpress.XtraEditors.LabelControl lblMsg2;
        private DevExpress.XtraEditors.LabelControl lblTotalCount;
        private DevExpress.XtraEditors.LabelControl lblMsg1;
        private DevExpress.XtraEditors.LabelControl lblMsg3;
        private DevExpress.XtraEditors.LabelControl lblRecPerPg;
        private DevExpress.XtraEditors.LabelControl lblMsg4;
        private DevExpress.XtraEditors.LabelControl lblSept;
        //private System.Windows.Forms.Label lblPageCount;
        private DevExpress.XtraEditors.LabelControl lblPageCount;
        private DevExpress.XtraEditors.SimpleButton simpBtnFirst;
        private DevExpress.XtraEditors.SimpleButton simpBtnPrev;
        private DevExpress.XtraEditors.SimpleButton simpBtnNext;
        private DevExpress.XtraEditors.SimpleButton simpBtnLast;

    }
}
