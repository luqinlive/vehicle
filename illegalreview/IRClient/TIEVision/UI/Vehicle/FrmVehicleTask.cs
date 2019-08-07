using CodeProject;
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

namespace TIEVision.UI.Vehicle
{
    public partial class FrmVehicleTask : XtraForm
    {
        private Thread thLoadFaceTaskData = null;
        private Thread m_thrd = null;
        public delegate void DelegateBindHandler();
        public delegate void DelegateRestartTime();
        private List<VehicleTask> mVehicleTaskList = new List<VehicleTask>();
        int currentIndex = -1;
        public int mTaskCount = 0;
        private string mTaskPath = "";
        public List<string> m_listFilePath = new List<string>();
        public FrmVehicleTask()
        {
            InitializeComponent();
        }

        private void FrmVehicleTask_Load(object sender, EventArgs e)
        {
            loadTaskData();
        }

        private void loadTaskData()
        {
            try
            {
                if (thLoadFaceTaskData != null)
                {
                    thLoadFaceTaskData.Abort();
                }
                thLoadFaceTaskData = new Thread(new ThreadStart(GetFaceTaskData));
                thLoadFaceTaskData.Start();
            }
            catch
            {

            }
        }

        public void GetFaceTaskData()
        {
            mVehicleTaskList = VehicleMongoDAL.GetInstance().GetAllVehicleTask();
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
                if (null != mVehicleTaskList)
                    this.gridControl1.DataSource = mVehicleTaskList;
            }
            catch { }
        }
        private void simpleButtonChoseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.Description = "请选择文件路径";
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                mTaskPath = folderDlg.SelectedPath;
                textEdit1.Text = folderDlg.SelectedPath;
            }
        }

        private void simpleButtonAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textEdit1.Text))
            {
                mTaskPath = textEdit1.Text;
                VehicleTask vehicleTask = new VehicleTask();
                vehicleTask.CreateTime = new BsonDateTime(DateTime.Now);
                vehicleTask.TaskId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                vehicleTask.TaskPath = mTaskPath;
                vehicleTask.TaskState = "0";
                vehicleTask.TaskCount = "0";
                bool bResult = VehicleMongoDAL.GetInstance().AddVehicleTask(vehicleTask);
                if (bResult)
                {
                    XtraMessageBox.Show("添加成功!");
                    loadTaskData();
                }
            }
            else
            {
                XtraMessageBox.Show("先选择路径!");
            }
        }

        private void simpleButtonStart_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_thrd != null)
            {
                m_thrd.Abort();
            }
            timer1.Enabled = false;
            m_thrd = new Thread(new ThreadStart(SavePicturesToDB), 0);
            m_thrd.IsBackground = true;
            m_thrd.Start();
        }

        public void RestartTime()
        {
            timer1.Enabled = true;
        }

        public void SavePicturesToDB()
        {
            mVehicleTaskList = VehicleMongoDAL.GetInstance().GetAllVehicleTask();
            if (null != mVehicleTaskList)
            {
                if (mVehicleTaskList.Count > 0)
                {
                    for (int i = 0; i < mVehicleTaskList.Count; i++)
                    {
                        if (mVehicleTaskList[i].TaskState == "0")
                        {
                            currentIndex = i;
                            break;
                        }
                        else
                        {
                            currentIndex = -1;
                        }
                    }

                    if (currentIndex >= 0)
                    {
                        foreach (FileData f in FastDirectoryEnumerator.EnumerateFiles(mVehicleTaskList[currentIndex].TaskPath, "*", System.IO.SearchOption.AllDirectories))
                        {

                            string filename = f.Path.ToLower();
                            string FileFullName = f.Path;
                            if ((filename.Contains(".jpg") || filename.Contains(".bmp") || filename.Contains(".jpeg")))
                            {
                                m_listFilePath.Add(f.Path);
                                //InsertIntoMongo(filename);

                                mVehicleTaskList[currentIndex].TaskCount = mTaskCount++.ToString();
                            }
                            if (m_listFilePath.Count >= 100)
                            {
                                ZmqVehicleSink.GetInstance().SendVehicles(m_listFilePath);
                                try
                                {
                                    mVehicleTaskList[currentIndex].TaskCount = (Convert.ToInt32(mVehicleTaskList[currentIndex].TaskCount) + m_listFilePath.Count).ToString();
                                    VehicleMongoDAL.GetInstance().UpdateVehicleTask(mVehicleTaskList[currentIndex]);
                                    this.Invoke(new DelegateBindHandler(BindDataSource));
                                }
                                catch
                                { }
                                m_listFilePath.Clear();
                            }

                        }
                        if (m_listFilePath.Count > 0)
                        {
                            ZmqVehicleSink.GetInstance().SendVehicles(m_listFilePath);
                            try
                            {
                                mVehicleTaskList[currentIndex].TaskCount = (Convert.ToInt32(mVehicleTaskList[currentIndex].TaskCount) + m_listFilePath.Count).ToString();
                                VehicleMongoDAL.GetInstance().UpdateVehicleTask(mVehicleTaskList[currentIndex]);
                                this.Invoke(new DelegateBindHandler(BindDataSource));
                            }
                            catch
                            { }
                            m_listFilePath.Clear();
                        }
                        mVehicleTaskList[currentIndex].TaskState = "1";
                        VehicleMongoDAL.GetInstance().UpdateVehicleTask(mVehicleTaskList[currentIndex]);
                        loadTaskData();

                    }
                }
            }

            try
            {
                Thread.Sleep(1000);
                this.Invoke(new DelegateRestartTime(RestartTime));
            }
            catch { }
        }

        private void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "TaskState")
            {
                switch (e.Value.ToString().Trim())
                {
                    case "1":
                        e.DisplayText = "已完成";
                        break;
                    case "0":
                        e.DisplayText = "未完成";
                        break;
                    default:
                        e.DisplayText = "";
                        break;
                }
            }
            if (e.Column.FieldName == "CreateTime")
            {
                DateTime dt = Convert.ToDateTime(e.Value.ToString());
                e.DisplayText = dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}
