using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraTab;
using TIEVision.COMMON;
using TIEVision.UI;
using TIEVision.UI.Vehicle;
using TIEVison.COMMON;
using TIEVision.Model;
using ZeroMQ;
using System.Media;
using System.IO;
using DevExpress.XtraBars.Alerter;
using System.Drawing.Drawing2D;
using TIEVision.DAL;
using IRVision.UI.Vehicle;


namespace TIEVision
{
    public partial class FrmMain : XtraForm
    {
        //public static ZContext mContext = new ZContext();

        public FrmMain()
        {
            InitializeComponent();
            Skin skin = TabSkins.GetSkin(DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveLookAndFeel);
            SkinElement element = skin[TabSkins.SkinTabPane];
            element.Color.BackColor = SystemColors.Control;
            element.Image.ImageCount = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

           //Type t = typeof(Image);
           //Console.Write( t.FullName);

            this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            this.WindowState = FormWindowState.Maximized;
            string nIndex = IniInfoHelper.GetInstance().GetValueInfo("GeneralConfiguration", "Index");
            if(nIndex == "0")
            {
                SetTargetStatues(false);
                SetFaceStatus(false);
                //FrmVehicleTask frmVehicleTask = new FrmVehicleTask();
                //frmVehicleTask.FormBorderStyle = FormBorderStyle.None;
                //frmVehicleTask.Dock = DockStyle.Fill;
                //frmVehicleTask.TopLevel = false;
                //this.xtraTabPage6.Controls.Add(frmVehicleTask);
                //frmVehicleTask.Show();


                FrmMarking frmMarking = new FrmMarking();
                frmMarking.FormBorderStyle = FormBorderStyle.None;
                frmMarking.Dock = DockStyle.Fill;
                frmMarking.TopLevel = false;
                this.xtraTabPage6.Controls.Add(frmMarking);
                frmMarking.Show();

                //FrmVehicleSearch frmVehicleSearch = new FrmVehicleSearch();
                //frmVehicleSearch.FormBorderStyle = FormBorderStyle.None;
                //frmVehicleSearch.Dock = DockStyle.Fill;
                //frmVehicleSearch.TopLevel = false;
                //this.xtraTabPage7.Controls.Add(frmVehicleSearch);
                //frmVehicleSearch.Show();
                FrmCrossing frmCrossing = new FrmCrossing();
                frmCrossing.FormBorderStyle = FormBorderStyle.None;
                frmCrossing.Dock = DockStyle.Fill;
                frmCrossing.TopLevel = false;
                this.xtraTabPage7.Controls.Add(frmCrossing);
                frmCrossing.Show();

                FrmVehSearchByPic frmVehSearchByPic = new FrmVehSearchByPic();
                frmVehSearchByPic.FormBorderStyle = FormBorderStyle.None;
                frmVehSearchByPic.Dock = DockStyle.Fill;
                frmVehSearchByPic.TopLevel = false;
                this.xtraTabPage8.Controls.Add(frmVehSearchByPic);
                frmVehSearchByPic.Show();



                this.xtraTabControl1.SelectedTabPage = xtraTabPage6;

                this.xtraTabPage8.PageVisible = false;

                /*
                try
                {
                    bool isExist = ServiceHelper.IsServiceExisted("HCarRegWorker");
                    if (isExist)
                    {
                        ServiceHelper.StartService("HCarRegWorker");
                    }
                    isExist = ServiceHelper.IsServiceExisted("HCarRegProxy");
                    if (isExist)
                    {
                        ServiceHelper.StartService("HCarRegProxy");
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(typeof(FrmMain), ex.Message);
                }   
                 * */
            }
            else if( nIndex =="1")
            {             

              
            }
            else if(nIndex =="2")
            {
                
                
            }
            
            //系统设置
            FrmSystemSet frmSystemSet = new FrmSystemSet();
            frmSystemSet.FormBorderStyle = FormBorderStyle.None;
            frmSystemSet.Dock = DockStyle.Fill;
            frmSystemSet.TopLevel = false;
            this.xtraTabPage4.Controls.Add(frmSystemSet);
            frmSystemSet.Show();
            /*
            TestObject obj = new TestObject();
            Bitmap bmp = (Bitmap)Image.FromFile(@"D:\images\33.jpg");
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                bmp.Save(ms, bmp.RawFormat);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                //return byteImage;
                obj.BinaryData = byteImage;
                TestMongoDAL.GetInstance().Add(obj);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }  
           */

           
        }

        SoundPlayer soundPlayer = new SoundPlayer();
        public delegate void ShowAlarmEventHandler(CardFaceAlarmItem cardItem);
        private void ShowFaceAlarmInfo(FaceAlarmInfo faceObj)
        {
            CardFaceAlarmItem itemInfo = new CardFaceAlarmItem();

            itemInfo.ShowImage = MongoHelper.GetInstance().GetImageFileByName(faceObj.HitImageName);
            itemInfo.ShowImage2 = MongoHelper.GetInstance().GetImageFileByName(faceObj.AlarmImageName);
            itemInfo.PassTime = "时间:" + Convert.ToDateTime(faceObj.CreateTime.AsDateTime.AddHours(8)).ToString("yyyy-MM-dd HH:mm:ss");
            itemInfo.CrossName = "比分:" + faceObj.ThresholdResult;
            try
            {
                this.Invoke(new ShowAlarmEventHandler(ShowAlertDialog), itemInfo);
            }
            catch { }

        }

        private void ShowAlertDialog(CardFaceAlarmItem cardItem)
        {
            AlertInfo info = new AlertInfo("提示", cardItem.PassTime + "     " + cardItem.CrossName);
            //AlertInfo info = new AlertInfo("", "");
            info.Image = cardItem.ShowImage;
            Image frame = (Image)cardItem.ShowImage.Clone();
            Image playbutton = (Image)cardItem.ShowImage2.Clone();
            using (frame)
            {
                int width = 0; int height = 0;
                if(frame.Width> playbutton.Width)
                {
                    width = frame.Width * 2;
                    height = frame.Height > playbutton.Height ? frame.Height : playbutton.Height;
                    var bitmap = new Bitmap(width, height);
                    {
                        using (var canvas = Graphics.FromImage(bitmap))
                        {
                            canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            canvas.DrawImage(frame,
                                             new Rectangle(0,
                                                           0,
                                                           frame.Width,
                                                           frame.Height),
                                             new Rectangle(0,
                                                           0,
                                                           frame.Width,
                                                           frame.Height),
                                             GraphicsUnit.Pixel);
                            canvas.DrawImage(playbutton, new Rectangle(bitmap.Width / 2+(bitmap.Width / 2 - playbutton.Width) / 2,
                                                           (bitmap.Height-playbutton.Height)/2,
                                                           playbutton.Width,
                                                           playbutton.Height), new Rectangle(0,
                                                           0,
                                                           playbutton.Width,
                                                           playbutton.Height), GraphicsUnit.Pixel);
                            canvas.Save();
                        }
                        try
                        {
                            info.Image = ((Image)bitmap).GetThumbnailImage(300, 200, () => false, IntPtr.Zero); ;
                            //bitmap.Save(/*somekindofpath*/,
                            //            System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        catch (Exception ex) { }
                    }
                }
                else
                {
                    width = playbutton.Width * 2;
                    height = frame.Height > playbutton.Height ? frame.Height : playbutton.Height;
                    var bitmap = new Bitmap(width, height);
                    {
                        using (var canvas = Graphics.FromImage(bitmap))
                        {
                            canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            canvas.DrawImage(frame,
                                             new Rectangle((bitmap.Width/2 -frame.Width)/2,
                                                           (bitmap.Height- frame.Height)/2,
                                                           frame.Width,
                                                           frame.Height),
                                             new Rectangle(0,
                                                           0,
                                                           frame.Width,
                                                           frame.Height),
                                             GraphicsUnit.Pixel);
                            canvas.DrawImage(playbutton, new Rectangle(playbutton.Width,
                                                           0,
                                                           playbutton.Width,
                                                           playbutton.Height), new Rectangle(0,
                                                           0,
                                                           playbutton.Width,
                                                           playbutton.Height), GraphicsUnit.Pixel);
                            canvas.Save();
                        }
                        try
                        {
                            //bitmap.Save(@"C:\1.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            info.Image = ((Image)bitmap).GetThumbnailImage(300, 200, () => false, IntPtr.Zero); 
                            
                        }
                        catch (Exception ex) { }
                    }
                }
                
            }

            //info.Image = CombineBitmap(m_ListImage);// cardItem.ShowImage;
            
            alertControl1.Show(this.FindForm(), info);
            try
            {
                string alarmFile = Application.StartupPath + "\\BUZZ.WAV";
                if (File.Exists(alarmFile))
                {
                    soundPlayer.Dispose();
                    soundPlayer = new SoundPlayer();
                    soundPlayer.SoundLocation = alarmFile;
                    soundPlayer.Load();
                    soundPlayer.Play();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void alertControl1_AlertClick(object sender, AlertClickEventArgs e)
        {
            
        }

        private void alertControl1_BeforeFormShow(object sender, AlertFormEventArgs e)
        {
            e.AlertForm.Size = new System.Drawing.Size(450, 200);
        }

        private void alertControl1_FormLoad(object sender, AlertFormLoadEventArgs e)
        {
            e.AlertForm.Size = new System.Drawing.Size(450, 200);
        }

        public void SetTargetStatues(bool status)
        {
          
           
        }

        public void SetVhielcStatus(bool status)
        {
            this.xtraTabPage6.PageVisible = status;
            this.xtraTabPage7.PageVisible = status;
            this.xtraTabPage8.PageVisible = status;
        }

        public void SetFaceStatus(bool status)
        {
           
        }

        private void ShowTabPageFunc(string tabPageName)
        {
            string nIndex = IniInfoHelper.GetInstance().GetValueInfo("GeneralConfiguration", "Index");
            if (nIndex == "0")
            {
               
            }
            else if (nIndex == "1")
            {
              
            }
        }

        private void simpleButtonMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void simpleButtonMax_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("是否退出程序!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                ZmqComparison.GetInstance().RleaseZmqComparison();
               
                ZmqVehicleSink.GetInstance().RleaseZmqVehicleSink();
               
             
                //ZmqFaceHitResLib.GetInstance().RleaseZmqFaceHitResLib();
                //frmVideoAnalysis.Close();
                try
                {
                    bool isExist = ServiceHelper.IsServiceExisted("TFeatureComparison");
                    if (isExist)
                    {
                        ServiceHelper.StopService("TFeatureComparison");
                    }
                    isExist = ServiceHelper.IsServiceExisted("HCarRegWorker");
                    if (isExist)
                    {
                        ServiceHelper.StopService("HCarRegWorker");
                    }
                    isExist = ServiceHelper.IsServiceExisted("HCarRegProxy");
                    if (isExist)
                    {
                        ServiceHelper.StopService("HCarRegProxy");
                    }
                }
                catch(Exception ex)
                {
                    LogHelper.WriteLog(typeof(FrmMain), ex.Message);
                }
                this.Close();
                this.Dispose();
                Application.Exit();
            }
            else
            {
               // e.Cancel = true;

            }
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            
        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        private void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            XtraTabPage selectedPage = xtraTabControl1.SelectedTabPage;
            if(null != selectedPage)
            {
               // selectedPage.ForeColor = Color.Red;
                //if(selectedPage == xtraTabPage11)
                {
                    //xtraTabPage11.Appearance.Header.BorderColor = Color.Red;
                    //xtraTabPage11.Image =Image.FromFile(Application.StartupPath + "\\人员分析-黄.png");
                }
            }
        }

        
    }
}