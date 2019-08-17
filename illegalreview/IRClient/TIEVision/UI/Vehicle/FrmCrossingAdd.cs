using DevExpress.XtraEditors;
using IRVision.DAL;
using IRVision.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TIEVision.COMMON;

namespace IRVision.UI.Vehicle
{
    public partial class FrmCrossingAdd : XtraForm
    {
        public string crossId = "";
        public string crossName = "";
        CrossingInfoDAL dal = new CrossingInfoDAL();
        private CrossingInfo mCrossInfo = null;
        private bool bModified = false;
        public FrmCrossingAdd()
        {
            InitializeComponent();
        }

        private void FrmCrossingAdd_Load(object sender, EventArgs e)
        {
            if (null != mCrossInfo)
            {
                bModified = true;
                textBox_CrossId.Text = mCrossInfo.CROSSING_ID;
                textBox_CrossName.Text = mCrossInfo.CROSSING_NAME;
            }

        }

        public void setModifiedCross(CrossingInfo userInfo)
        {
            this.mCrossInfo = userInfo;
        }


        private void simpleButtonConform_Click(object sender, EventArgs e)
        {
            int retRst = -1;
            crossId = textBox_CrossId.Text.Trim();
            crossName = textBox_CrossName.Text.Trim();
            if (ValidationService.CheckNull(crossId) || ValidationService.CheckNull(crossName))
            {
                XtraMessageBox.Show("通道编号和名称不能为空!");
                return;
            }
            if (bModified==false)
            {
                retRst = dal.AddCrossing(crossId, crossName);
            }
            if(bModified)
            {
                retRst = dal.UpdateCrossing(crossId, crossName, mCrossInfo.ID);
            }
            if (retRst >=0)
            {
                if (bModified)
                    XtraMessageBox.Show("修改成功!");
                else
                    XtraMessageBox.Show("添加成功!");
            }
            else
            {
                if (bModified)
                    XtraMessageBox.Show("修改失败!");
                else
                    XtraMessageBox.Show("添加失败!");
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
