using DevExpress.XtraEditors;
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
using TIEVision.COMMON;
using TIEVision.Model;
using TIEVision.UI;

namespace TIEVision
{
    public partial class FrmSystemSet : XtraForm
    {
        private Thread thLoadData = null;
        public MongoHelper mongo = new MongoHelper();
        private List<UserInfo> mUserInfoList = new List<UserInfo>();
        public delegate void DelegateBindHandler( );

        public FrmSystemSet()
        {
            InitializeComponent();
        }

        private void FrmSystemSet_Load(object sender, EventArgs e)
        {
            //this.gridView1.RowHeight = 100;
            string ShowLogo = System.Configuration.ConfigurationManager.AppSettings["ShowLogo"].ToString();
            if(ShowLogo =="false")
            {
                labelControl1.Visible = false;
                labelControl2.Visible = false;
            }
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
                thLoadData = new Thread(new ThreadStart(GetData));
                thLoadData.Start();
            }
            catch
            {

            }
        }

        public void GetData()
        {
            mUserInfoList = MongoHelper.GetInstance().GetAllUser();
            try
            {
                this.Invoke(new DelegateBindHandler(BindDataSource));
            }
            catch
            { }
        }

        private void BindDataSource()
        {
            this.gridControl1.DataSource = null;
            this.gridControl1.DataSource = mUserInfoList;
        }

        private void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "UserRole")
            {
                switch (e.Value.ToString().Trim())
                {
                    case "1":
                        e.DisplayText = "普通用户";
                        break;
                    case "0":
                        e.DisplayText = "超级管理员";
                        break;
                    default:
                        e.DisplayText = "";
                        break;
                }
            }
            if(e.Column.FieldName == "CreateTime")
            {
                DateTime dt = Convert.ToDateTime(e.Value.ToString());
                e.DisplayText = dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private void simpleButtonAdd_Click(object sender, EventArgs e)
        {
            FrmSystemSetAddUser frmAddUser = new FrmSystemSetAddUser();
            frmAddUser.ShowDialog();
            if (frmAddUser.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                loadData();
            }
        }

        private void simpleButtonDel_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("确认删除!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                int nIndex = this.gridView1.FocusedRowHandle;
                UserInfo delUser = mUserInfoList[nIndex];
                if (delUser.UserRole == "0")
                {
                    XtraMessageBox.Show("管理员帐户不能删除！");
                    return;
                }

                bool retVal = MongoHelper.GetInstance().DeleteUserInfo(delUser);
                if (retVal)
                {
                    loadData();
                }
            }
            
        }

        private void simpleButtonModify_Click(object sender, EventArgs e)
        {
            int nIndex = this.gridView1.FocusedRowHandle;
            UserInfo delUser = mUserInfoList[nIndex];
            FrmSystemSetAddUser frmAddUser = new FrmSystemSetAddUser();
            frmAddUser.setModifiedUser(delUser);
            frmAddUser.ShowDialog();
            if (frmAddUser.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                loadData();
            }

        }
    }
}
