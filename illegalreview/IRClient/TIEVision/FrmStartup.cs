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
using TIEVision.UI.Vehicle;

namespace TIEVision
{
    public partial class FrmStartup : XtraForm
    {
        private bool _Exit;

        public bool Exit
        {
            get { return _Exit; }
        }
        public FrmStartup()
        {
            InitializeComponent();
        }

        private void frmstartup_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            this.WindowState = FormWindowState.Maximized;
            Program.SetMainForm(new FrmMain());
            //Program.SetMainForm(new FrmMarking());
            ThreadPool.QueueUserWorkItem(new WaitCallback(Program.InitApp), this);
        }

        //显示文字信息
        public void PrintMsg(Object msg)
        {
            lblMsg.Text = msg.ToString();
        }

        //关闭启动窗体，如果需要中止程序，传参数false
        public void CloseForm(Object o)
        {
            this._Exit = Convert.ToBoolean(o);
            
            Program.ShowMainForm();

            this.Close();
        }

        private void FrmStartup_SizeChanged(object sender, EventArgs e)
        {
            lblMsg.Left = Screen.PrimaryScreen.WorkingArea.Width / 2 - lblMsg.Width;
        }
    }
}
