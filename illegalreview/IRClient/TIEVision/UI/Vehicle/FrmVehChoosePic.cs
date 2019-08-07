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

namespace TIEVision.UI.Vehicle
{
    public partial class FrmVehChoosePic : XtraForm
    {
        private Thread thLoadData = null;
        public string mTargetFileName { get; set; }
        private double mPictureWidth;
        private double mPictureHeight;
        public Bitmap cropBmp;
        private List<Rectangle> mListCustomBodyRec = new List<Rectangle>();
        public List<TIEVision.COMMON.HOBJ.HOBJInfo> mResultInfoList = new List<TIEVision.COMMON.HOBJ.HOBJInfo>();
        public VehicleRecogResult mVehResult = new VehicleRecogResult();
        //当前选中目标
        public int nSelectedTarget = 0;
        public FrmVehChoosePic()
        {
            InitializeComponent();
        }

        private void FrmVehChoosePic_Load(object sender, EventArgs e)
        {
            pictureEdit1.Properties.AllowFocused = false;
        }

        public void SetImagePath(string fileName)
        {
            mTargetFileName = fileName;
            pictureEdit1.Image = Image.FromFile(fileName);
            try
            {
                if (thLoadData != null)
                {
                    thLoadData.Abort();
                }
                thLoadData = new Thread(new ThreadStart(GetResultData));
                thLoadData.Start();
            }
            catch
            {

            }

        }

        public void GetResultData()
        {
             VehicleRecogResult result =  ZmqVehicleSink.GetInstance().GetVehicleByPic(mTargetFileName);
             if(null != result )
             {
                 mVehResult = result;
                 if (null != result.Veh)
                 {
                     foreach (var item in result.Veh)
                     {
                         string[] vehicleLocation = item.Clwz.Split(',');
                         if (vehicleLocation.Length == 4)
                         {
                             int _x = Convert.ToInt32(vehicleLocation[0]);
                             int _y = Convert.ToInt32(vehicleLocation[1]);
                             int _w = Convert.ToInt32(vehicleLocation[2]);
                             int _h = Convert.ToInt32(vehicleLocation[3]);

                             mListCustomBodyRec.Add(new Rectangle(new Point(_x, _y), new Size(_w, _h)));
                             TIEVision.COMMON.HOBJ.HOBJInfo info = new TIEVision.COMMON.HOBJ.HOBJInfo();
                             info.info.rect.x = _x;
                             info.info.rect.y = _y;
                             info.info.rect.width = _w;
                             info.info.rect.height = _h;
                             mResultInfoList.Add(info);
                         }


                     }
                 }
                 
             }

             pictureEdit1.Invalidate();
        }


        private void simpleButtonConform_Click(object sender, EventArgs e)
        {
            if (nSelectedTarget < 0)
            {
                MessageBox.Show("请先框选一个目标!");
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Rectangle ScreenToPic(Rectangle rect)
        {
            double scaleX = mPictureWidth / (double)pictureEdit1.Width;
            double scaleY = mPictureHeight / (double)pictureEdit1.Height;
            int _width = (int)(Math.Round(rect.Width * scaleX));
            int _height = (int)(Math.Round(rect.Height * scaleY));
            int _xs = (int)(Math.Round(rect.X * scaleX));
            int _ys = (int)(Math.Round(rect.Y * scaleY));
            return new Rectangle(new Point(_xs, _ys), new Size(_width, _height));
        }

        private Rectangle PicToScreen(Rectangle rect)
        {
            double scaleX = mPictureWidth / (double)pictureEdit1.Width;
            double scaleY = mPictureHeight / (double)pictureEdit1.Height;
            int _width = (int)(Math.Round(rect.Width / scaleX));
            int _height = (int)(Math.Round(rect.Height / scaleY));
            int _xp = (int)(Math.Round(rect.X / scaleX));
            int _yp = (int)(Math.Round(rect.Y / scaleY));
            return new Rectangle(new Point(_xp, _yp), new Size(_width, _height));
        }

        bool _selecting = false;
        private bool drawing;
        private Point startPos;
        private Point currentPos;
        Rectangle _selection;


        private void pictureEdit1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Pen pen = new Pen(Color.Red, 1);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                if (drawing)
                {
                    e.Graphics.DrawRectangle(pen, GetRectangle());
                }
                using (Bitmap bitmap = (Bitmap)Bitmap.FromFile(mTargetFileName))
                {
                    mPictureWidth = (double)bitmap.Width;
                    mPictureHeight = (double)bitmap.Height;
                    int nIndex = nSelectedTarget;
                    //if (nIndex >= 0)
                    {
                        //Paint Vehicle Body Area
                        if (mResultInfoList.Count > 0)
                        {
                            for (int i = 0; i < mResultInfoList.Count; ++i)
                            {
                                TIEVision.COMMON.HOBJ.CvRect_t body = mResultInfoList[i].info.rect;
                                int _x = body.x;
                                int _y = body.y;
                                int _h = body.height;
                                int _w = body.width;
                                Rectangle bodyRect = new Rectangle(new Point(_x, _y), new Size(_w, _h));
                                Pen penBlue = new Pen(Color.FromArgb(0xff, Color.LimeGreen), 2);
                                if (i == nIndex)
                                {
                                    penBlue = new Pen(Color.Red, 2);
                                    penBlue.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                                    //pick rectangle average color
                                    //bodyRect.Inflate(-bodyRect.Width / 4, -bodyRect.Height / 4);
                                    cropBmp = bitmap.Clone(bodyRect, bitmap.PixelFormat);

                                }
                                else
                                {
                                    penBlue.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                                }
                                e.Graphics.DrawRectangle(penBlue, PicToScreen(new Rectangle(new Point(_x, _y), new Size(_w, _h))));
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void pictureEdit1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _selecting = true;
                _selection = new Rectangle(new Point(e.X, e.Y), new Size());

                currentPos = startPos = e.Location;
                drawing = true;
            }
        }

        private void pictureEdit1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selecting)
            {
                _selection.Width = (e.X - _selection.X);
                _selection.Height = (e.Y - _selection.Y);

                // Redraw the picturebox:
                pictureEdit1.Refresh();
            }

            currentPos = e.Location;
            if (drawing)
            {
                pictureEdit1.Invalidate();
            }
        }

        private Rectangle GetRectangle()
        {
            return new Rectangle(
                Math.Min(startPos.X, currentPos.X),
                Math.Min(startPos.Y, currentPos.Y),
                Math.Abs(startPos.X - currentPos.X),
                Math.Abs(startPos.Y - currentPos.Y)
                );
        }

        private void pictureEdit1_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                drawing = false;
                this.pictureEdit1.Cursor = Cursors.Default;
                int nSelectIndex = -1;
                var rect = GetRectangle();
                for (int nIndex = 0; nIndex < mResultInfoList.Count; nIndex++)
                {
                    TIEVision.COMMON.HOBJ.CvRect_t body = mResultInfoList[nIndex].info.rect;
                    int _x = body.x;
                    int _y = body.y;
                    int _h = body.height;
                    int _w = body.width;
                    mListCustomBodyRec.Add(new Rectangle(new Point(_x, _y), new Size(_w, _h)));


                    Rectangle bodyRect = new Rectangle(new Point(_x, _y), new Size(_w, _h));
                    Rectangle overlappedRect = Rectangle.Empty;

                    Rectangle custRect = ScreenToPic(rect);
                    overlappedRect = Rectangle.Intersect(bodyRect, custRect);
                    if (!overlappedRect.IsEmpty)
                    {
                        double interSectPercent = Math.Round((double)(overlappedRect.Width * overlappedRect.Height) / (bodyRect.Width * bodyRect.Height), 2);
                        int interSect = Convert.ToInt32(interSectPercent * 100);
                        if (interSect > 60)
                        {
                            nSelectIndex = nIndex;
                        }
                        Console.WriteLine("重合度:" + interSect + " nIndex :" + nIndex);
                    }
                }
                nSelectedTarget = nSelectIndex;
            }
        }
    }
}
