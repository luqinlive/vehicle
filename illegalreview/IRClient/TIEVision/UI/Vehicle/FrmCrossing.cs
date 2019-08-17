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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRVision.UI.Vehicle
{
    public partial class FrmCrossing : XtraForm
    {
        private Thread thLoadData = null;
        public delegate void DelegateBindHandler();
        CrossingInfoDAL dal = new CrossingInfoDAL();
        List<CrossingInfo> mCrossingList = new List<CrossingInfo>();
        public FrmCrossing()
        {
            InitializeComponent();
        }

        private void FrmCrossing_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void simpleButtonRefresh_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
        {
            try
            {
                if (thLoadData != null)
                {
                    thLoadData.Abort();
                }
                thLoadData = new Thread(new ThreadStart(GetListData));
                thLoadData.Start();
            }
            catch
            {

            }
        }

        public void GetListData()
        {
            mCrossingList = dal.GetCrossingInfos();
            try
            {
                this.Invoke(new DelegateBindHandler(BindDataSource));
            }
            catch
            { }
        }

        private void BindDataSource()
        {
            try
            {
                this.gridControl1.DataSource = null;
                if (null != mCrossingList)
                    this.gridControl1.DataSource = mCrossingList;
            }
            catch { }
        }

        private void simpleBtnAdd_Click(object sender, EventArgs e)
        {
            FrmCrossingAdd frmCrossingAdd = new FrmCrossingAdd();
            frmCrossingAdd.ShowDialog();
            if (frmCrossingAdd.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                loadData();
            }
        }

        private void simpleBtnDelete_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("确认删除!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                int nIndex = this.gridView1.FocusedRowHandle;
                CrossingInfo delCross = mCrossingList[nIndex];

                int  retVal = dal.DeleteCrossing(delCross.ID);
                if (retVal>=0)
                {
                    loadData();
                }
            }
        }

        private void simpleBtnModify_Click(object sender, EventArgs e)
        {
            int nIndex = this.gridView1.FocusedRowHandle;
            CrossingInfo modifyCross = mCrossingList[nIndex];
            FrmCrossingAdd frmCrossInfoAdd = new FrmCrossingAdd();
            frmCrossInfoAdd.setModifiedCross(modifyCross);
            frmCrossInfoAdd.ShowDialog();
            if (frmCrossInfoAdd.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                loadData();
            }
        }

    }
}
