using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using IniParser;
using IniParser.Model;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TIEVision.COMMON;
using TIEVision.Model;
using TIEVison.COMMON;

namespace TIEVision
{
    public partial class FrmUserLogin : XtraForm
    {
        public string iniFilePath = "";
        public static FileIniDataParser fileIniData = new FileIniDataParser();
        IniData parsedData = null;
        public string LoginInfo = "";
        public string userName;
        public string password;
        public FrmUserLogin()
        {
            InitializeComponent();
        }

        private void FrmUserLogin_Load(object sender, EventArgs e)
        {
           
            iniFilePath = Application.StartupPath + "\\TIEVision.ini";
            parsedData = fileIniData.ReadFile(iniFilePath);
            LoginInfo = parsedData["GeneralConfiguration"]["LoginInfo"];

            if(!ValidationService.CheckNull(LoginInfo))
            {
                string[] UserPass = EncodeHelper.DesDecrypt(LoginInfo).Split('|');
                if(UserPass.Length ==2)
                {
                    textBox_UserName.Text = UserPass[0];
                    textBox_Password.Text = UserPass[1];
                }
            }
            else
            {
                checkBoxRemember.Checked = false;
            }


            string nIndex = IniInfoHelper.GetInstance().GetValueInfo("GeneralConfiguration", "Index");
            radioGroup1.SelectedIndex = Convert.ToInt32(nIndex);
            radioGroup1_SelectedIndexChanged(null, null);
            //Bitmap img = new Bitmap("D:\\images\\body.jpg");
            //FileStream fs = new FileStream("D:\\cie1931_500x500.rgb", FileMode.Open, FileAccess.Read);
            //int nBytes = (int)fs.Length;
            //byte[] byteArray = new byte[nBytes];
            //int nBytesRead = fs.Read(byteArray, 0, nBytes);
            //using (MemoryStream br = new MemoryStream(byteArray))
            //{
            //    Image image = System.Drawing.Image.FromStream(br);
            //    btn_login.Image = image;
            //}
            //Console.WriteLine(img.Size);
            //SplashScreenManager.ShowForm(typeof(WaitForm1));

        }

        private void btn_login_Click(object sender, EventArgs e)
        {

            userName = textBox_UserName.Text.Trim();
            password = textBox_Password.Text.Trim();
            if (ValidationService.CheckNull(userName) || ValidationService.CheckNull(password))
            {
                XtraMessageBox.Show("用户名或密码不能为空!");
                return;
            }
            //CheckAdminUser();
            UserInfo userInfo = new UserInfo();
            userInfo.UserName = userName;
            userInfo.Password = EncodeHelper.DesEncrypt(password);
            if (userName=="admin"&& password =="admin")
            //if (MongoHelper.GetInstance().CheckUserLogin(userInfo))
            {
                
                this.Hide();
                FrmStartup spScreen = new FrmStartup();
                
                Program.SetMainForm(spScreen);
                Program.ShowMainForm();
                //Thread.Sleep(5000);
                //Program.SetMainForm(new FrmMain());
                //Program.ShowMainForm();

                if (checkBoxRemember.Checked == true)
                {
                    string string64 = EncodeHelper.DesEncrypt(userName + "|" + password);
                    parsedData["GeneralConfiguration"]["LoginInfo"] = string64;
                    fileIniData.WriteFile(iniFilePath, parsedData);
                }
                else
                {
                    parsedData["GeneralConfiguration"]["LoginInfo"] = "";
                    fileIniData.WriteFile(iniFilePath, parsedData);
                }
                if (radioGroup1.SelectedIndex == 0)
                {
                    parsedData["GeneralConfiguration"]["Index"] = "0";
                    fileIniData.WriteFile(iniFilePath, parsedData);
                }
                else if (radioGroup1.SelectedIndex == 1)
                {
                    parsedData["GeneralConfiguration"]["Index"] = "1";
                    fileIniData.WriteFile(iniFilePath, parsedData);
                }
                else if (radioGroup1.SelectedIndex == 2)
                {
                    parsedData["GeneralConfiguration"]["Index"] = "2";
                    fileIniData.WriteFile(iniFilePath, parsedData);
                }
                this.Close();
            }
            else
            {
                XtraMessageBox.Show("用户名或密码错误!");
                return;

            }
            
          
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private bool CheckAdminUser()
        {
            //默认创建admin用户
            UserInfo userInfo = new UserInfo();
            userInfo.CreateTime = new BsonDateTime(DateTime.Now);
            userInfo.UserName = "admin";
            userInfo.Password = EncodeHelper.DesEncrypt("admin");
            userInfo.AccountName = "管理员";
            userInfo.UserRole = "0";
            userInfo.EmailAddress = "admin@ferretview.com";
            userInfo.PhoneNumber = "1356897895";
            if(!MongoHelper.GetInstance().CheckUserInfo(userInfo))
            {
                MongoHelper.GetInstance().AddUser(userInfo);
            }
            return true;
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(radioGroup1.SelectedIndex == 0)
            {
               
                pictureEdit3.Visible = true;
            
                //IniInfoHelper.GetInstance().SetValueInfo("GeneralConfiguration", "Index", "0");
            }

            if (radioGroup1.SelectedIndex == 1)
            {
               
                pictureEdit3.Visible = false;
              
                //IniInfoHelper.GetInstance().SetValueInfo("GeneralConfiguration", "Index", "1");
            }
            if(radioGroup1.SelectedIndex ==2)
            {
              
                pictureEdit3.Visible = true;
            }
        }
       
    }
}
