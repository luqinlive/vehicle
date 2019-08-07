using DevExpress.XtraEditors;
using MongoDB.Bson;
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
using TIEVision.DAL;
using TIEVision.Model;
using TIEVison.COMMON;

namespace TIEVision.UI.Vehicle
{
    public partial class FrmVehSearchByPic : XtraForm
    {
        private string mFileName = "";
        public PagerControl pager = null;
        public const int nRecordPerPage = 30;
        Thread thLoadData = null;
        public delegate void DelegateProgress(int progress);
        public delegate void DelegateBindHandler();
        //以图搜图
        public string mSearchFaceFileName = "";
        public string mSearchFaceBase64 = "";
        public Thread mThrdVehicleSearch = null;
        public string mQueryClpp = "";
        public SortedSet<VehicleCompareResult> mSortedVehResult = new SortedSet<VehicleCompareResult>(new VehicleCompareSort());
        public List<VehicleCompareResult> mSortedVehResultList = new List<VehicleCompareResult>();
        public List<CardTargetItem> mCardVehResultList = new List<CardTargetItem>();

        public FrmVehSearchByPic()
        {
            InitializeComponent();
        }

        private void FrmVehSearchByPic_Load(object sender, EventArgs e)
        {
            SetDateEditProp(this.dtpStrartTime, Convert.ToDateTime(DateTime.Now.AddDays(0).ToString("yyyy-MM-dd") + " 00:00:00"));
            SetDateEditProp(this.dtpEndTime, Convert.ToDateTime(DateTime.Now.AddDays(0).ToString("yyyy-MM-dd") + " 23:59:59"));

            this.layoutView1.OptionsView.ShowViewCaption = false;
            this.layoutView1.OptionsView.ViewMode = DevExpress.XtraGrid.Views.Layout.LayoutViewMode.MultiRow;
            this.layoutView1.OptionsCustomization.AllowFilter = false;
            this.layoutView1.OptionsCustomization.AllowSort = false;

            pager = new PagerControl(1, nRecordPerPage, 1, "前往");
            pager.currentPageChanged += new EventHandler(pager_currentPageChanged);//页码变化 触发的事件
            pager.Dock = DockStyle.Fill;
            this.panelControl3.Controls.Add(pager);//在Panel容器中加入这个控件

            progressPanel1.Location = new Point(gridControl1.Location.X + gridControl1.Width / 2 - progressPanel1.Width / 2,
                gridControl1.Location.Y + gridControl1.Height / 2 - progressPanel1.Height / 2);
        }

        private void pager_currentPageChanged(object sender, EventArgs e)
        {
            PagerControl pager = sender as PagerControl;
            if (pager == null)
            {
                return;
            }
            //Console.WriteLine(pager.CurrentPage);
            try
            {
                this.Invoke(new DelegateBindHandler(BindDataSource));
            }
            catch
            { }
            //BindResults(pager.CurrentPage);//查询数据并分页绑定
        }



        private void SetDateEditProp(DateEdit dtpEdit, DateTime dtTime)
        {
            dtpEdit.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            dtpEdit.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dtpEdit.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            dtpEdit.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dtpEdit.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";
            dtpEdit.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            dtpEdit.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;
            dtpEdit.EditValue = dtTime;


        }
        private string mSearchBase64 = "";
        int nSelectedIndex = 0;
        VehicleRecogResult mVehSearchResult = new VehicleRecogResult();
        private void simpleButtonChoosePic_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "请选择要图片";
                ofd.Filter = "JPG图片|*.jpg|PNG图片|*.png|BMP图片|*.bmp|Gif图片|*.gif";
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    mFileName = ofd.FileName;

                    FrmVehChoosePic frmChoosePic = new FrmVehChoosePic();
                    frmChoosePic.SetImagePath(mFileName);
                    frmChoosePic.ShowDialog();
                    if (frmChoosePic.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        this.pictureEdit1.Image = frmChoosePic.cropBmp;
                        //objectInfo = frmChoosePic.mObjectInfo;
                        mVehSearchResult = frmChoosePic.mVehResult;
                        nSelectedIndex = frmChoosePic.nSelectedTarget;
                        mSearchFaceBase64 = mVehSearchResult.Veh[nSelectedIndex].Cltzxx;
                        Veh vehicle = mVehSearchResult.Veh[nSelectedIndex];
                        if (null != vehicle)
                        {
                            mQueryClpp = vehicle.Clpp.Split('-')[0];
                            string retStr = "车型：" + vehicle.Clpp + "  \n";
                            retStr += "车牌：" + vehicle.Hphm + "  \n";
                            foreach (var item in VehicleDictList.GetInstance().GetVehicleTypeList())
                            {
                                if (item.SYSDICT_CODE == vehicle.Cllx)
                                {
                                    retStr += "车辆类型：" + item.SYSDICT_NAME + "  \n";
                                    break;
                                }
                            }
                            retStr += "行为特征：";
                            int i = 0;
                            foreach (var item in VehicleDictList.GetInstance().GetVehicleXwtz())
                            {
                                if (vehicle.Xwtz.Contains(item.SYSDICT_CODE))
                                {
                                    if (i++ == 0)
                                    {
                                        retStr += "" + item.SYSDICT_NAME + "\n";
                                    }
                                    else
                                    {
                                        retStr += "                  " + item.SYSDICT_NAME + "\n";
                                    }

                                }
                            }
                            labelControlResult.Text = retStr;
                        }

                    }

                }
            }
        }

        private void simpleButtonQuery_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mSearchFaceBase64))
            {
                XtraMessageBox.Show("未选择图片", "提示");
                return;
            }
            this.gridControl1.DataSource = null;
            if (mThrdVehicleSearch != null)
            {
                mThrdVehicleSearch.Abort();
            }
            simpleButtonQuery.Enabled = false;
            progressPanel1.Visible = true;
            mThrdVehicleSearch = new Thread(new ThreadStart(SearchFaceMongo));
            mThrdVehicleSearch.IsBackground = true;
            mThrdVehicleSearch.Start();
        }

        private void SearchFaceMongo()
        {
            mCardVehResultList.Clear();
            mSortedVehResult.Clear();
            mSortedVehResultList.Clear();
            HVehicleQuery vehicleQuery = new HVehicleQuery();
            vehicleQuery.timeStart = Convert.ToDateTime(dtpStrartTime.EditValue.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            vehicleQuery.timeEnd = Convert.ToDateTime(dtpEndTime.EditValue.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            vehicleQuery.queryClpp = mQueryClpp;

            LogHelper.WriteLog(typeof(FrmVehSearchByPic), "count start");
            //int mTotalVehCount = Convert.ToInt32(VehicleMongoDAL.GetInstance().GetVehicleCount());
            int mTotalVehCount = Convert.ToInt32(VehicleMongoDAL.GetInstance().GetVehicleCount(vehicleQuery));
            LogHelper.WriteLog(typeof(FrmVehSearchByPic), "count end " + mTotalVehCount);
            int RecordsPerPage = 1000;
            int pageCount = mTotalVehCount / RecordsPerPage;
            if (mTotalVehCount % RecordsPerPage == 0)
            {
                pageCount = mTotalVehCount / RecordsPerPage;
            }
            else
            {
                pageCount = mTotalVehCount / RecordsPerPage + 1;
            }
            vehicleQuery.PageSize = RecordsPerPage;

            LogHelper.WriteLog(typeof(FrmVehSearchByPic), "query start");
            //ObjectId fistId = VehicleMongoDAL.GetInstance().GetVehicleFirstId(vehicleQuery);
            for (int i = 1; i <= pageCount; i++)
            {
                LogHelper.WriteLog(typeof(FrmVehSearchByPic), "mongo start" + i);
                //List<FaceObject> TempFaceObjList = FaceMongoDAL.GetInstance().GetFaceList(i, RecordsPerPage);
                vehicleQuery.PageNumber = i;
                //List<VehicleObject> TempFaceObjList = VehicleMongoDAL.GetInstance().GetVehicleListGtId(fistId, vehicleQuery);
                List<VehicleObject> TempFaceObjList = VehicleMongoDAL.GetInstance().GetVehicleList(vehicleQuery);
                /*
                if (null != TempFaceObjList)
                {
                    if (TempFaceObjList.Count > 0)
                    {
                        fistId = TempFaceObjList[TempFaceObjList.Count - 1].Id;
                    }
                }*/
                LogHelper.WriteLog(typeof(FrmVehSearchByPic), "mongo end");

                double progress = (double)i / (double)pageCount * 100;
                try
                {
                    this.Invoke(new DelegateProgress(ShowProgress), Convert.ToInt32(progress));
                }
                catch
                {

                }

                foreach (var faceObj in TempFaceObjList)
                {
                    if (mVehSearchResult.Veh[nSelectedIndex].Cbdm != faceObj.vehicle.Cbdm)
                        continue;
                    VehicleCompareResult faceResult = new VehicleCompareResult();
                    float score = 0;// (float)VehSimCos.HCARREGSimCos(mSearchFaceBase64, faceObj.vehicle.Cltzxx);//Feature.Compare(mSearchFaceBase64, faceObj.FaceFeature);
                    faceResult.Score = score;
                    faceResult.CreateTime = faceObj.CreateTime.AsDateTime.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
                    faceResult.ImagePath = faceObj.ImagePath;
                    faceResult.Clpp = faceObj.vehicle.Clpp;
                    faceResult.Clwz = faceObj.vehicle.Clwz;
                    faceResult.Cphm = faceObj.vehicle.Hphm;
                    mSortedVehResult.Add(faceResult);
                    if (mSortedVehResult.Count > 100)
                    {
                        mSortedVehResult.Remove(mSortedVehResult.Last());
                    }
                }

            }
            mSortedVehResultList = mSortedVehResult.ToList();
            LogHelper.WriteLog(typeof(FrmVehSearchByPic), "query end");

            try
            {
                this.Invoke(new DelegateBindHandler(BindDataSource));
            }
            catch
            { }

        }

        private void ShowProgress(int progress)
        {
            progressPanel1.Description = "Loading... " + progress + "%";
        }

        struct Cell
        {
            public string FieldName { get; set; }
            public int RowIndex { get; set; }
            public string FileName { get; set; }
            public string RectBody { get; set; }
        }
        private void LoadImageAsync(Cell cell)
        {
            Thread thread2 = new Thread(new ParameterizedThreadStart(DoWork));
            thread2.Start(cell);
        }

        void DoWork(object parameter)
        {
            // emulate image loading
            Cell cell = (Cell)parameter;
            //Thread.Sleep(1000);
            //Image img = Image.FromFile(cell.FileName);
            //ShellFile shellFile = ShellFile.FromFilePath(cell.FileName);
            //Bitmap shellThumb = shellFile.Thumbnail.ExtraLargeBitmap;
            //Bitmap shellThumb = shellFile.Thumbnail.LargeBitmap;
            Image img = Image.FromFile(cell.FileName);
            Size thumbnailSize = WindowsThumbnailProvider.GetThumbnailSize(img);
            if (!string.IsNullOrEmpty(cell.RectBody))
            {
                using (Graphics g = Graphics.FromImage(img))
                {
                    Color customColor = Color.FromArgb(100, Color.Red);
                    SolidBrush shadowBrush = new SolidBrush(customColor);
                    Pen pen = new Pen(Color.Red, 8);
                    Rectangle rectFToFill = new Rectangle();
                    string[] vehicleLocation = cell.RectBody.Split(',');
                    if (vehicleLocation.Length == 4)
                    {
                        rectFToFill.X = Convert.ToInt32(vehicleLocation[0]);
                        rectFToFill.Y = Convert.ToInt32(vehicleLocation[1]);
                        rectFToFill.Width = Convert.ToInt32(vehicleLocation[2]);
                        rectFToFill.Height = Convert.ToInt32(vehicleLocation[3]);

                    }
                    //rectFToFill.Inflate(new Size(50, 50));
                    //g.FillRectangles(shadowBrush, new RectangleF[] { rectFToFill });
                    g.DrawRectangles(pen, new Rectangle[] { rectFToFill });
                }
            }

            Image thumbnail = img.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
            gridControl1.Invoke(new MethodInvoker(() =>
            {
                layoutView1.SetRowCellValue(layoutView1.GetRowHandle(cell.RowIndex), cell.FieldName, thumbnail);
            }));
        }

        int nStart = 0; int nEnd = 0;
        private void BindDataSource()
        {
            try
            {
                this.gridControl1.DataSource = null;
                if (null != mSortedVehResultList)
                {
                    if (mSortedVehResultList.Count > 0)
                    {
                        int nStartNum = (pager.CurrentPage - 1) * pager.RecordsPerPage;
                        int nPerNum = pager.RecordsPerPage;

                        nStart = nStartNum;
                        nEnd = nPerNum;
                        if (nStart < mSortedVehResultList.Count())
                        {
                            int remain = mSortedVehResultList.Count - nStart;
                            if (remain < nEnd)
                            {
                                nEnd = remain;
                            }
                        }
                        mCardVehResultList.Clear();
                        LogHelper.WriteLog(typeof(FrmVehSearchByPic), "show start");
                        int i = 0;
                        foreach (var faceResultObj in mSortedVehResultList.GetRange(nStart, nEnd))
                        {
                            CardTargetItem itemInfo = new CardTargetItem();
                            //itemInfo.ShowImage = Image.FromFile(faceResultObj.ImagePath);
                            Cell cell = new Cell { FieldName = "ShowImage", RowIndex = i++, FileName = faceResultObj.ImagePath, RectBody = faceResultObj.Clwz };
                            LoadImageAsync(cell);
                            itemInfo.PassTime = "时间:" + faceResultObj.CreateTime;
                            itemInfo.CrossName = "分数:" + faceResultObj.Score.ToString("F2");
                            itemInfo.PlateColor = "车型:" + faceResultObj.Clpp;
                            itemInfo.PlateNo = "车牌:" + faceResultObj.Cphm;
                            mCardVehResultList.Add(itemInfo);
                        }
                        LogHelper.WriteLog(typeof(FrmVehSearchByPic), "show end");
                        this.gridControl1.DataSource = mCardVehResultList;

                    }
                    pager.TotalCount = mSortedVehResultList.Count();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                simpleButtonQuery.Enabled = true;
                progressPanel1.Visible = false;
            }
        }

        FrmTargetDetail frmTargetDetial = null;
        private void layoutView1_DoubleClick(object sender, EventArgs e)
        {
            int nIndex = layoutView1.FocusedRowHandle;
            CardTargetItem itemInfo = mCardVehResultList[nIndex];
            //VehicleCompareResult vehicleRstObj = mSortedVehResult.ElementAt(nIndex);//.[nIndex];
            VehicleCompareResult vehicleRstObj = mSortedVehResultList.GetRange(nStart, nEnd)[nIndex];
            Rectangle rect = new Rectangle();
            string[] vehicleLocation = vehicleRstObj.Clwz.Split(',');
            if (vehicleLocation.Length == 4)
            {
                rect.X = Convert.ToInt32(vehicleLocation[0]);
                rect.Y = Convert.ToInt32(vehicleLocation[1]);
                rect.Width = Convert.ToInt32(vehicleLocation[2]);
                rect.Height = Convert.ToInt32(vehicleLocation[3]);

            }
            if (null == frmTargetDetial)
            {
                frmTargetDetial = new FrmTargetDetail();
            }
            else if (null != frmTargetDetial)
            {
                if (frmTargetDetial.IsDisposed)
                {
                    frmTargetDetial = new FrmTargetDetail();
                }
            }
            frmTargetDetial.SetImagePath(vehicleRstObj.ImagePath);
            //frmTargetDetial.SetTargetModel(targetInfoList[nIndex]);
            //frmTargetDetial.SetDetailInfo(itemInfo.toString());
            frmTargetDetial.SetTargetRect(rect);
            frmTargetDetial.Show();
            frmTargetDetial.BringToFront();
        }


    }
}
