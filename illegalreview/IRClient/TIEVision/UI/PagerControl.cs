using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNet.Common.Util;
using DevExpress.XtraEditors;

namespace TIEVision.UI
{
    public partial class PagerControl : XtraUserControl
    {
        private bool isNeedNotify = true;
        public PagerControl()
        {
            InitializeComponent();
        }

        public PagerControl(int currentPage, int recordsPerPage, int totalCount)
        {
            InitializeComponent();
            this.totalCount = totalCount;
            this.recordsPerPage = recordsPerPage;
            this.currentPage = currentPage;
            DrawControl();
        }

        public PagerControl(int currentPage, int recordsPerPage, int totalCount, string jumpText)
        {
            InitializeComponent();
            this.totalCount = totalCount;
            this.recordsPerPage = recordsPerPage;
            this.currentPage = currentPage;
            this.JumpText = jumpText;
            DrawControl();
        }

        private int currentPage = 1;
        /// <summary>
        /// 当前页面
        /// </summary>
        public virtual int CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; }
        }

        private int recordsPerPage = 15;
        /// <summary>
        /// 每页记录数
        /// </summary>
        public virtual int RecordsPerPage
        {
            get { return recordsPerPage; }
            set { recordsPerPage = value; }
        }

        private int totalCount = 0;
        /// <summary>
        /// 总记录数
        /// </summary>
        public virtual int TotalCount
        {
            get { return totalCount; }
            set { 
                totalCount = value; 
                //this.lblTotalCount.Text = TotalCount.ToString();
                //this.lblPageCount.Text = GetPageCount().ToString();
                isNeedNotify = false;
                DrawControl();
                isNeedNotify = true;
            }
        }

        private int pageCount = 0;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (recordsPerPage != 0)
                {
                    pageCount = GetPageCount();
                }
                return pageCount;
            }
        }

        private string jumpText;
        /// <summary>
        /// 跳转按钮文本
        /// </summary>
        public string JumpText
        {
            get
            {
                if (string.IsNullOrEmpty(jumpText))
                {
                    jumpText = "Go";
                }
                return jumpText;
            }
            set { jumpText = value; }
        }

        public event EventHandler currentPageChanged;
        private void SetFormCtrEnabled()
        {
            this.lnkFirst.Enabled = true;
            this.simpBtnFirst.Enabled = true;

            this.lnkPrev.Enabled = true;
            this.simpBtnPrev.Enabled = true;

            this.lnkNext.Enabled = true;
            this.simpBtnNext.Enabled = true;

            this.lnkLast.Enabled = true;
            this.simpBtnLast.Enabled = true;

            this.btnGo.Enabled = true;
        }

        /// <summary>
        /// 计算总页数
        /// </summary>
        /// <returns></returns>
        private int GetPageCount()
        {
            if (RecordsPerPage == 0)
            {
                return 0;
                //throw new DivideByZeroException("每页记录数为0");
            }
            int pageCount = TotalCount / RecordsPerPage;
            if (TotalCount % RecordsPerPage == 0)
            {
                pageCount = TotalCount / RecordsPerPage;
            }
            else
            {
                pageCount = TotalCount / RecordsPerPage + 1;
            }
            return pageCount;
        }

        /// <summary>
        /// 页面控件呈现
        /// </summary>
        private void DrawControl()
        {
            this.btnGo.Text = this.JumpText;
            //lblCurrentPage.Text = string.Format("{0}/{1}  共 {2} 条记录，每页 {3} 条", CurrentPage.ToString(),
            //    this.PageCount.ToString(), TotalCount.ToString(), RecordsPerPage.ToString());
            this.lblCurrentPage.Text = CurrentPage.ToString();
            this.lblPageCount.Text = PageCount.ToString();
            this.lblTotalCount.Text = TotalCount.ToString();
            this.lblRecPerPg.Text = RecordsPerPage.ToString();

            if (currentPageChanged != null && isNeedNotify)
            {
                currentPageChanged(this, null);//当前分页数字改变时，触发委托事件
            }
            SetFormCtrEnabled();
            if (PageCount == 1)//有且仅有一页
            {
                this.lnkFirst.Enabled = false;
                this.simpBtnFirst.Enabled = false; 

                this.lnkPrev.Enabled = false;
                this.simpBtnPrev.Enabled = false;

                this.lnkNext.Enabled = false;
                this.simpBtnNext.Enabled = false;

                this.lnkLast.Enabled = false;
                this.simpBtnLast.Enabled = false;
                this.btnGo.Enabled = false;
            }
            else if (CurrentPage == 1)//第一页
            {
                this.lnkFirst.Enabled = false;
                this.simpBtnFirst.Enabled = false; 
                this.lnkPrev.Enabled = false;
                this.simpBtnPrev.Enabled = false;
                //this.lnkFirst.ForeColor = Color.Gray;
                //this.lnkPrev.ForeColor = Color.Gray;
            }
            else if (CurrentPage == this.PageCount)//最后一页
            {
                this.lnkNext.Enabled = false;
                this.simpBtnNext.Enabled = false;
                this.lnkLast.Enabled = false;
                this.simpBtnLast.Enabled = false;
            }
        }

        private void lnkFirst_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CurrentPage = 1;
            DrawControl();
        }

        public void lnkPrev_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CurrentPage = Math.Max(1, CurrentPage - 1);
            DrawControl();
        }

        public void lnkNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CurrentPage = Math.Min(PageCount, CurrentPage + 1);
            DrawControl();
        }

        private void lnkLast_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CurrentPage = PageCount;
            DrawControl();
        }

        /// <summary>
        /// enter键功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPageNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strNum = this.txtPageNum.Text.Trim();
            if (e.KeyChar == (char)Keys.Enter && RegUtil.IsPositiveNumber(strNum))
            {
                CurrentPage = int.Parse(strNum);
                DrawControl();
            }
        }

        /// <summary>
        /// 页数限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            string strNum = this.txtPageNum.Text.Trim();
            if (strNum.Length > 0 && RegUtil.IsPositiveNumber(strNum) && int.Parse(strNum) > PageCount)
            {
                txtPageNum.Text = PageCount.ToString();
            }
        }

        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            string strNum = this.txtPageNum.Text.Trim();
            if (RegUtil.IsPositiveNumber(strNum) == false)
            {
                return;
            }
            CurrentPage = int.Parse(strNum);
            DrawControl();
        }

        public void txtPageNumReset()
        {
            txtPageNum.Text = "";
        }

        private void simpBtnFirst_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            DrawControl();
        }

        private void simpBtnPrev_Click(object sender, EventArgs e)
        {
            CurrentPage = Math.Max(1, CurrentPage - 1);
            DrawControl();
        }

        private void simpBtnNext_Click(object sender, EventArgs e)
        {
            CurrentPage = Math.Min(PageCount, CurrentPage + 1);
            DrawControl();
        }

        private void simpBtnLast_Click(object sender, EventArgs e)
        {
            CurrentPage = PageCount;
            DrawControl();
        }

    }
}
