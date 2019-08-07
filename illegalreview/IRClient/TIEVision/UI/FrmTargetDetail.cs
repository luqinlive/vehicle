using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TIEVision.COMMON;
using TIEVision.Model;

namespace TIEVision.UI
{
    public partial class FrmTargetDetail : XtraForm
    {
        //切换Tab标签事件
        public delegate void ShowTabPageEventHandler(string tabPageName);
        public static event ShowTabPageEventHandler ShowTabPage;

        public delegate void ShowVideoPosEventHandler(TargetInfo targetInfo);
        public static event ShowVideoPosEventHandler ShowVideoPos;

        public delegate void OnShowTargetEventHandler();
        public string strImage = null;
        public string mDetailInfo = "";
        public Image mTargetBackImage = null;
        public Image mHitImage = null;
        public Image mAlarmImage = null;
        public TargetInfo mTargetInfo = null;
        public Rectangle mTargetRect = new Rectangle();
        public FaceAlarmInfo mFaceAlarmInfo = null;
        public bool bAlarmInfo = false;
        public Caseinfo mAcqCaseInfoList = new Caseinfo();

        public DevExpress.XtraEditors.Controls.PictureSizeMode PicSizeMode { get; set; }
        
        public FrmTargetDetail()
        {
            InitializeComponent();
        }

        public void SetImagePath(string imagePath)
        {
            this.strImage = imagePath;
            new Thread(new ThreadStart(StartDownload)).Start();
        }

        public void SetDetailInfo(string detailInfo)
        {
            this.mDetailInfo = detailInfo;
        }
        public void SetTargetRect(Rectangle rect)
        {
            this.mTargetRect = rect;
        }

        public void SetTargetModel(TargetInfo targetInfo)
        {
            simpleBtnKeyIndex.Visible = true;
            this.mTargetInfo = targetInfo;
        }

        public void SetAlarmInfo(FaceAlarmInfo faceAlarmInfo)
        {
            this.bAlarmInfo = true;
            this.mFaceAlarmInfo = faceAlarmInfo;
        }

        private void FrmTargetDetail_Load(object sender, EventArgs e)
        {
            if (null != PicSizeMode)
            {
                pictureEdit1.Properties.SizeMode = PicSizeMode;// DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            }
            
        }


        private void StartDownload()
        {
            try
            {
                if (File.Exists(strImage))
                {
                    using (FileStream fs = new FileStream(strImage, FileMode.Open, FileAccess.Read))
                    {
                        mTargetBackImage = new Bitmap(Image.FromStream(fs));
                    }
                }
                else if (!string.IsNullOrEmpty(strImage))
                {
                    mTargetBackImage = MongoHelper.GetInstance().GetImageFileByName(strImage);
                    //mTargetBackImage = Image.FromStream(WebRequest.Create(strImage).GetResponse().GetResponseStream());
                   if(null != mFaceAlarmInfo)
                   {
                       mHitImage = MongoHelper.GetInstance().GetImageFileByName(mFaceAlarmInfo.AlarmImageName);
                       mAlarmImage = MongoHelper.GetInstance().GetImageFileByName(mFaceAlarmInfo.HitImageName);
                   }
                    
                }
                else
                {
                    //using image.fromfile can't delete file 

                }
            }
            catch (Exception ex)
            { }
            try
            {
                this.Invoke(new OnShowTargetEventHandler(ShowTargetInfo));
            }
            catch (Exception ex)
            { }
        }

        private void ShowTargetInfo()
        {
            if(mTargetBackImage.Height > pictureEdit1.Height)
            {
                pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            }else
            {
                pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;

            }
            pictureEdit1.Image = mTargetBackImage;
            this.labelControl1.Text = mDetailInfo;
            if (bAlarmInfo)
            {
                pictureEdit1.Location = new Point(pictureEdit1.Location.X + pictureEdit3.Width, pictureEdit1.Location.Y);
                pictureEdit3.Image = mHitImage;
                pictureEdit4.Image = mAlarmImage;
                pictureEdit3.Visible = true;
                pictureEdit4.Visible = true;
                string CrossName="";
                if(null != mFaceAlarmInfo)
                {
                    foreach (var caseItem in mAcqCaseInfoList.Case)
                    {
                        if (null != caseItem.Avfiles)
                        {
                            foreach (var avItem in caseItem.Avfiles)
                            {
                                if (avItem.Id == mFaceAlarmInfo.CrossName)
                                {
                                    CrossName = "  文件:" + avItem.Name;
                                    break;
                                }
                            }
                        }
                        if (null != caseItem.Devices)
                        {
                            foreach (var devices in caseItem.Devices)
                            {
                                if (devices.Id == mFaceAlarmInfo.CrossName)
                                {
                                    CrossName = "  摄像头:" + devices.Name;
                                    break;
                                }
                            }
                        }

                    }
                    this.labelControl1.Text = "报警时间:" + mFaceAlarmInfo.PassTime + "  名称:" + mFaceAlarmInfo.ShowName +CrossName;
                }
               
            }
           
        }

        private void pictureEdit1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Red, 2);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            if (null != mTargetBackImage)
            {
                double scaleX = (double)mTargetBackImage.Width / (double)pictureEdit1.Width;
                double scaleY = (double)mTargetBackImage.Height / (double)pictureEdit1.Height;
                int _width = 0;
                int _height = 0;
                int _xp = 0;
                int _yp = 0;
                _width = (int)(Math.Round(mTargetRect.Width / scaleX));
                _height = (int)(Math.Round(mTargetRect.Height / scaleY));
                _xp = (int)(Math.Round(mTargetRect.X / scaleX));
                _yp = (int)(Math.Round(mTargetRect.Y / scaleY));
                e.Graphics.DrawRectangle(pen, new Rectangle(_xp, _yp, _width, _height));
            }
        }

        private void FrmTargetDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void simpleBtnKeyIndex_Click(object sender, EventArgs e)
        {
            if(null != ShowTabPage)
            {
                ShowTabPage("FrmVideoAnalysis");
            }
            if(null != ShowVideoPos)
            {
                ShowVideoPos(mTargetInfo);
            }

            this.Close();
        }

        private void pictureEdit1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mTargetBackImage == null)
            {
                return;
            }
            if (pictureEdit1.Bounds.Contains(e.X, e.Y))
            {
                pictureEdit2.Visible = true;
                int _picSrcWidth = pictureEdit1.Bounds.Width;
                if (e.X < _picSrcWidth / 2)
                {
                    pictureEdit2.Location = new Point(pictureEdit1.Location.X + pictureEdit1.Bounds.Width - pictureEdit2.Width, pictureEdit1.Location.Y);
                }
                else
                {
                    pictureEdit2.Location = new Point(pictureEdit1.Location.X, pictureEdit1.Location.Y);
                }
            }
            Rectangle rect = new Rectangle();
            //if (pictureEdit1.SizeMode == PictureBoxSizeMode.Zoom)
            {
               // rect = GetPictureBoxZoomSize(pictureEdit1);
            }
            UpdateZoomedImage(e, rect);
        }

        private Color _BackColor = Color.Black;
        private void UpdateZoomedImage(MouseEventArgs e, Rectangle rect)
        {
            if (pictureEdit1.Image == null)
                return;

            int zoomWidth = 160; // pictureBox2.Width / 1;
            int zoomHeight = 110; // pictureBox2.Height / 1;

            Bitmap tempBitmap = new Bitmap(zoomWidth, zoomHeight, PixelFormat.Format24bppRgb);
            Graphics bmGraphics = Graphics.FromImage(tempBitmap);
            bmGraphics.Clear(_BackColor);
            bmGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            double scaleX = (double)pictureEdit1.Image.Width / (double)pictureEdit1.Width;
            double scaleY = (double)pictureEdit1.Image.Height / (double)pictureEdit1.Height;

            //if (pictureEdit1.SizeMode == PictureBoxSizeMode.Zoom)
            //{
            //    scaleX = (double)pictureEdit1.Image.Width / (double)rect.Width;
            //    scaleY = (double)pictureEdit1.Image.Height / (double)rect.Height;
            //}

            int _xp = (int)((double)(e.X) * scaleX);
            int _yp = (int)((double)(e.Y) * scaleY);
            //if (pictureEdit1.SizeMode == PictureBoxSizeMode.Zoom)
            //{
            //    _xp = (int)((double)(e.X - rect.X) * scaleX);
            //    _yp = (int)((double)(e.Y - rect.Y) * scaleY);
            //}
            Rectangle rectDst = new Rectangle(0, 0, zoomWidth, zoomHeight);
            //Console.WriteLine(rectDst.ToString());
            Rectangle rectSrc = new Rectangle(_xp - zoomWidth / 2, _yp - zoomHeight / 2, zoomWidth, zoomHeight);
            //Console.WriteLine(rectSrc);
            bmGraphics.DrawImage(pictureEdit1.Image, rectDst, rectSrc, GraphicsUnit.Pixel);
            pictureEdit2.Image = tempBitmap;
            pictureEdit2.Refresh();
            bmGraphics.Dispose();
        }

        private void pictureEdit1_MouseLeave(object sender, EventArgs e)
        {
            pictureEdit2.Visible = false;
        }

        private void pictureEdit4_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void FrmTargetDetail_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

 
    }
}
