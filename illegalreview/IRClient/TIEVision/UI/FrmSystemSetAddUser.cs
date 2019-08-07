using DevExpress.XtraEditors;
using MongoDB.Bson;
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
using TIEVision.Model;
using TIEVison.COMMON;

namespace TIEVision.UI
{
    public partial class FrmSystemSetAddUser : XtraForm
    {
        public string userName = "";
        public string password = "";
        private UserInfo mUserInfo = null;
        private bool bModified = false;
        public FrmSystemSetAddUser()
        {
            InitializeComponent();
        }


        public void setModifiedUser(UserInfo userInfo)
        {
            this.mUserInfo = userInfo;
        }

        private void FrmSystemSetAddUser_Load(object sender, EventArgs e)
        {
            if(null != mUserInfo)
            {
                bModified = true;
                simpleButtonConform.Text = "修改";
                textBox_UserName.Enabled = false;
                textBox_UserName.Text = mUserInfo.UserName;
                textBox_Password.Text = EncodeHelper.DesDecrypt(mUserInfo.Password);
                textBox_AccountName.Text = mUserInfo.AccountName;
                textBox_Email.Text = mUserInfo.EmailAddress;
                textBox_Phone.Text = mUserInfo.PhoneNumber;
            }
        }

        private void simpleButtonConform_Click(object sender, EventArgs e)
        {
            userName = textBox_UserName.Text.Trim();
            password = textBox_Password.Text.Trim();
            if (ValidationService.CheckNull(userName) || ValidationService.CheckNull(password))
            {
                XtraMessageBox.Show("用户名或密码不能为空!");
                return;
            }
            if(null != mUserInfo)
            {
                MongoHelper.GetInstance().DeleteUserInfo(mUserInfo);
            }
            UserInfo userInfo = new UserInfo();
            userInfo.UserName = userName;
            userInfo.Password = EncodeHelper.DesEncrypt(password);
            userInfo.CreateTime = new BsonDateTime(DateTime.Now);
            userInfo.AccountName = textBox_AccountName.Text;
            userInfo.PhoneNumber = textBox_Phone.Text;
            userInfo.EmailAddress = textBox_Email.Text;
            userInfo.UserRole = "1";
            bool ret = MongoHelper.GetInstance().AddUser(userInfo);
            if(ret)
            {
                if (bModified)
                    XtraMessageBox.Show("修改成功!");
                else
                    XtraMessageBox.Show("添加成功!");
            }
            else
            {
                if(bModified)
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
