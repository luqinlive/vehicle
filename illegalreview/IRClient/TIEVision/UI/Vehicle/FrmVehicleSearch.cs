using DevExpress.XtraEditors;
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
using TIEVision.DAL;
using TIEVision.Model;

namespace TIEVision.UI.Vehicle
{
    public partial class FrmVehicleSearch : XtraForm
    {
        private Thread thLoadVehicleData = null;
        private Thread thLoadControlData = null;
        public delegate void ShowDataEventHandler();
        public delegate void DelegateBindHandler();
        private List<VehicleObject> mVehicleObjList = new List<VehicleObject>();
        public List<CardTargetItem> mCardTargetList = new List<CardTargetItem>();
        private long mVehicleCount = 0;
        PagerControl pagerVehicle = null;
        private int nRecordPerPage = 30;
        List<HSysDictInfo> dictListMblx = new List<HSysDictInfo>();
        List<HSysDictInfo> dictListMbys = new List<HSysDictInfo>();
        List<HSysDictInfo> dictListClpp = new List<HSysDictInfo>();
        List<HSysDictInfo> dictListXwtz = new List<HSysDictInfo>();
        DataTable dtCasesList = new DataTable();
        /// <summary>
        /// 开始时间
        /// </summary>
        public string queryStrartTime = null;
        /// <summary>
        /// 结束时间
        /// </summary>
        public string queryEndTime = null;
        /// <summary>
        /// 车牌号码
        /// </summary>
        string queryCphm = null;
        /// <summary>
        /// 车辆类型
        /// </summary>
        string queryCllx = null;
        /// <summary>
        /// 车辆颜色
        /// </summary>
        string queryClys = null;
        /// <summary>
        /// 车辆品牌
        /// </summary>
        string queryClpp = null;
        /// <summary>
        /// 行为特征
        /// </summary>
        string queryXwtz = null;
        public HVehicleQuery mTargetQuery = new HVehicleQuery();

        public FrmVehicleSearch()
        {
            InitializeComponent();
        }

        private void FrmVehicleSearch_Load(object sender, EventArgs e)
        {
            this.layoutView1.OptionsView.ShowViewCaption = false;
            this.layoutView1.OptionsView.ViewMode = DevExpress.XtraGrid.Views.Layout.LayoutViewMode.MultiRow;
            this.layoutView1.OptionsCustomization.AllowFilter = false;
            this.layoutView1.OptionsCustomization.AllowSort = false;

            pagerVehicle = new PagerControl(1, nRecordPerPage, 1, "前往");
            pagerVehicle.currentPageChanged += new EventHandler(pager_currentPageChanged);//页码变化 触发的事件
            pagerVehicle.Dock = DockStyle.Fill;
            this.panelControl3.Controls.Add(pagerVehicle);//在Panel容器中加入这个控件

            SetDateEditProp(this.dtpStrartTime, Convert.ToDateTime(DateTime.Now.AddDays(0).ToString("yyyy-MM-dd") + " 00:00:00"));
            SetDateEditProp(this.dtpEndTime, Convert.ToDateTime(DateTime.Now.AddDays(0).ToString("yyyy-MM-dd") + " 23:59:59"));
            //loadData();
            dtCasesList.Columns.Add("Id", typeof(string));
            dtCasesList.Columns.Add("ParentId", typeof(string));
            dtCasesList.Columns.Add("Name", typeof(string));

            lookUpEditType.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("SYSDICT_NAME", 100, "Full Name"));
            lookUpEditClzpp.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("SYSDICT_NAME", 100, "Full Name"));
            lookUpEditClnk.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("SYSDICT_NAME", 100, "Full Name"));
            if (thLoadControlData != null)
            {
                thLoadControlData.Abort();
            }
            thLoadControlData = new Thread(new ThreadStart(GetControlData));
            thLoadControlData.IsBackground = true;
            thLoadControlData.Start();

        }

        private void GetControlData()
        {
            //List<HSysDictInfo> dictinfo = (List<HSysDictInfo>)GlobalContext.GetInstance()["MBYSLIST"];
            //dictListClys = dictinfo;
            //0-行人;1-自行车;2-摩托车;3-电动摩托车;4-三轮车;5-小型车;6-大车;7-开车;8-拖拉机;9-中巴
            dictListMblx = VehicleDictList.GetInstance().GetVehicleTypeList();
            dictListMbys = VehicleDictList.GetInstance().GetVehicleColorList();
            //dictListClpp = VehicleDictList.GetInstance().GetVehiclePpList();
            dictListClpp = VehicleDictList.GetInstance().GetVehicleClpp();
            dictListXwtz = VehicleDictList.GetInstance().GetVehicleXwtz();
            //mAcqCaseInfoList = AcqCaseManager.LoadAcqCaseInfos();
            try
            {
                this.Invoke(new ShowDataEventHandler(ShowControlData));
            }
            catch
            { }


        }

        //子品牌
        private void lookUpEditClpp_EditValueChanged(object sender, EventArgs e)
        {
            string Clpp = lookUpEditClpp.EditValue.ToString();
            List<HSysDictInfo> m_Clzpp = new List<HSysDictInfo>();
            m_Clzpp = VehicleDictList.GetInstance().GetVehicleClzpp(Clpp);

            if(null != m_Clzpp)
            {
                
                lookUpEditClzpp.Properties.DataSource = m_Clzpp;
                //lookUpEditType.EditValue = "";
                lookUpEditClzpp.Properties.ShowHeader = false;
                lookUpEditClzpp.ItemIndex = 0;
            }
        }

        //年款
        private void lookUpEditClzpp_EditValueChanged(object sender, EventArgs e)
        {
            string Clzpp = lookUpEditClzpp.EditValue.ToString();
            List<HSysDictInfo> m_Clnk = new List<HSysDictInfo>();
            m_Clnk = VehicleDictList.GetInstance().GetVehicleClnk(Clzpp);

            if (null != m_Clnk)
            {
                lookUpEditClnk.Properties.DataSource = m_Clnk;
                lookUpEditClnk.Properties.ShowHeader = false;
                lookUpEditClnk.ItemIndex = 0;
            }
        }

         private void ShowControlData()
        {
            if (null != dictListMblx)
            {
                
                lookUpEditType.Properties.DataSource = dictListMblx;
                //lookUpEditType.EditValue = "";
                lookUpEditType.Properties.ShowHeader = false;
                lookUpEditType.ItemIndex = 0;
                // lookUpEditType.Properties.DisplayMember = "SYSDICT_NAME";
                // lookUpEditType.Properties.ValueMember = "SYSDICT_CODE";
            }

            if (null != dictListMbys)
            {
                //车身颜色
                cbbox_vehiclecolor.Properties.DataSource = dictListMbys;
                cbbox_vehiclecolor.Properties.SelectAllItemVisible = false;
                cbbox_vehiclecolor.Properties.DisplayMember = "SYSDICT_NAME";
                cbbox_vehiclecolor.Properties.ValueMember = "SYSDICT_CODE";
            }

            if(null  != dictListClpp)
            {
                lookUpEditClpp.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("SYSDICT_NAME", 100, "Full Name"));
                lookUpEditClpp.Properties.DataSource = dictListClpp;
                //lookUpEditType.EditValue = "";
                lookUpEditClpp.Properties.ShowHeader = false;
                lookUpEditClpp.ItemIndex = 0;
               // DataRow dr = null;
               // for (int i = 0; i < dictListClpp.Count; i++ )
               // {
               //     dr = dtCasesList.NewRow();
               //     dr["Id"] = dictListClpp[i].SYSDICT_CODE.ToString()+Guid.NewGuid();
               //     dr["ParentId"] ="1111";
               //     dr["Name"] = dictListClpp[i].SYSDICT_NAME.ToString();
               //     dtCasesList.Rows.Add(dr);
               // }
               //// treeListLookUpEditClpp.DataSource = dtCasesList;
               // treeList1.DataSource = dtCasesList;
            }

             if(null != dictListXwtz)
             {
                 checkedComboBoxEditXwtz.Properties.DataSource = dictListXwtz;
                 checkedComboBoxEditXwtz.Properties.SelectAllItemVisible = false;
                 checkedComboBoxEditXwtz.Properties.DisplayMember = "SYSDICT_NAME";
                 checkedComboBoxEditXwtz.Properties.ValueMember = "SYSDICT_CODE";
             }
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

        private void pager_currentPageChanged(object sender, EventArgs e)
        {
            PagerControl pager = sender as PagerControl;
            if (pager == null)
            {
                return;
            }
            //Console.WriteLine(pager.CurrentPage);
            loadData();
            //BindResults(pager.CurrentPage);//查询数据并分页绑定
        }

        private void loadData()
        {
            try
            {
                if (thLoadVehicleData != null)
                {
                    thLoadVehicleData.Abort();
                }
                thLoadVehicleData = new Thread(new ThreadStart(GetVehicleListData));
                thLoadVehicleData.Start();
            }
            catch
            {

            }
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
           

            //string x= shellFile.Properties.System.Image.Dimensions.Value;
            // Image.FromFile(cell.FileName); 
            //Stream resultStream = new ThumbnailCreator().CreateThumbnailStream(thumbnailSize: 256, imageStream: new FileStream(cell.FileName, FileMode.Open, FileAccess.Read), imageFormat: Format.Jpeg);
            Image img = Image.FromFile(cell.FileName);//ThumbnailExtractor.FromFile(cell.FileName, new Size(256, 256),UseEmbeddedThumbnails.Auto,true,true); 
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
            //img.Dispose();
        }

         public void GetVehicleListData()
        {
            mVehicleObjList.Clear();
            mCardTargetList.Clear();
            mTargetQuery.PageNumber = 1;
            mTargetQuery.PageSize = 30;
            mTargetQuery.PageNumber = pagerVehicle.CurrentPage;
            mTargetQuery.PageSize = pagerVehicle.RecordsPerPage;
            //mVehicleCount = VehicleMongoDAL.GetInstance().GetVehicleCount();
            mVehicleCount = VehicleMongoDAL.GetInstance().GetVehicleCount(mTargetQuery);
            //mVehicleObjList = VehicleMongoDAL.GetInstance().GetVehicleList(pagerVehicle.CurrentPage, pagerVehicle.RecordsPerPage);
            mVehicleObjList = VehicleMongoDAL.GetInstance().GetVehicleList(mTargetQuery);
            int i = 0;
            foreach (var vehicleObj in mVehicleObjList)
            {
                CardTargetItem itemInfo = new CardTargetItem();
                //Image vehImage =Image.FromFile(vehicleObj.ImagePath);
                
                //itemInfo.ShowImage = vehImage;
                Cell cell = new Cell { FieldName = "ShowImage", RowIndex = i++, FileName = vehicleObj.ImagePath ,RectBody = vehicleObj.vehicle.Clwz};
                LoadImageAsync(cell);
                itemInfo.PassTime = "时间:" + Convert.ToDateTime(vehicleObj.CreateTime.AsDateTime.AddHours(8)).ToString("yyyy-MM-dd HH:mm:ss");
                itemInfo.PlateNo = "车牌:" + vehicleObj.vehicle.Hphm;
                itemInfo.CrossName = "车型:" + vehicleObj.vehicle.Clpp;
                foreach (var item in VehicleDictList.GetInstance().GetVehicleTypeList())
                {
                    if (item.SYSDICT_CODE == vehicleObj.vehicle.Cllx)
                    {
                        itemInfo.PlateNo += "    类型:" + item.SYSDICT_NAME + "  ";
                        break;
                    }
                }
                mCardTargetList.Add(itemInfo);
            }
            try
            {
                this.Invoke(new DelegateBindHandler(BindDataSource));
            }
            catch
            { }
        }

         private void layoutView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            
        }

         private void layoutView1_DataSourceChanged(object sender, EventArgs e)
         {
             
             
         }

         private void BindDataSource()
         {
             try
             {
                 this.gridControl1.DataSource = null;
                 if (null != mCardTargetList)
                     this.gridControl1.DataSource = mCardTargetList;
                 pagerVehicle.TotalCount = Convert.ToInt32(mVehicleCount);
             }
             catch
             {

             }
         }

        private void simpBtnSearch_Click(object sender, EventArgs e)
        {
            InitializeQuery();
            loadData();
        }

        private void InitializeQuery()
        {
            if (!ValidationService.CheckNull(dtpStrartTime.Text))
            {
                queryStrartTime = Convert.ToDateTime(dtpStrartTime.EditValue.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                mTargetQuery.timeStart = queryStrartTime;
            }
            else
            {
                queryStrartTime = null;
            }
            if (!ValidationService.CheckNull(dtpEndTime.Text))
            {
                queryEndTime = Convert.ToDateTime(dtpEndTime.EditValue.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                mTargetQuery.timeEnd = queryEndTime;
            }
            else
            {
                queryEndTime = null;
            }

            //车牌号码
            if (!ValidationService.CheckNull(textBoxCphm.Text))
            {
                queryCphm = textBoxCphm.Text.ToString();
                mTargetQuery.queryCphm = queryCphm;
            }
            else
            {
                queryCphm = null;
                mTargetQuery.queryCphm = null;
            }
            if (!ValidationService.CheckNull(cbbox_vehiclecolor.Text))
            {
                queryClys = (string)cbbox_vehiclecolor.EditValue;
                mTargetQuery.queryClys = queryClys;
            }
            else
            {
                queryClys = null;
                mTargetQuery.queryClys = null;
            }
            //行为特征
            if(!ValidationService.CheckNull(checkedComboBoxEditXwtz.Text))
            {
                queryXwtz = (string)checkedComboBoxEditXwtz.EditValue;
                queryXwtz = queryXwtz.Replace(",", "");
                mTargetQuery.queryXwtz=queryXwtz;
            }else{
                queryXwtz=null ;
                mTargetQuery.queryXwtz = null;
            }

            if (!ValidationService.CheckNull(lookUpEditType.Text))
            {
                queryCllx = lookUpEditType.EditValue.ToString();
                mTargetQuery.queryCllx = queryCllx;
            }
            else
            {
                queryCllx = null;
                mTargetQuery.queryCllx = null;
            }

            if (!ValidationService.CheckNull(lookUpEditClpp.Text))
            {
                queryClpp = lookUpEditClpp.Text.ToString();
                mTargetQuery.queryClpp = queryClpp;
            }
            else
            {
                queryClpp = null;
                mTargetQuery.queryClpp = null;
            }

        }

        private FrmTargetDetail frmTargetDetial = null;
        private void layoutView1_DoubleClick(object sender, EventArgs e)
        {
            int nIndex = layoutView1.FocusedRowHandle;
            if (nIndex < 0)
                return;
            CardTargetItem itemInfo = mCardTargetList[nIndex];
            VehicleObject vehicleObj = mVehicleObjList[nIndex];
            Rectangle rect = new Rectangle();
            string[] vehicleLocation = vehicleObj.vehicle.Clwz.Split(',');
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
            frmTargetDetial.SetImagePath(vehicleObj.ImagePath);
            //frmTargetDetial.SetTargetModel(targetInfoList[nIndex]);
            frmTargetDetial.SetDetailInfo(vehicleObj.ToString());
            frmTargetDetial.SetTargetRect(rect);
            frmTargetDetial.Show();
            frmTargetDetial.BringToFront();
        }


       
    }
}
