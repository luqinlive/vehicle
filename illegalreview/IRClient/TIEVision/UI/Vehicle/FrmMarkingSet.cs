using DevExpress.XtraEditors;
using IRVision.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TIEVision.Model;

namespace IRVision.UI.Vehicle
{
    public partial class FrmMarkingSet : XtraForm
    {
        public int nHaveLaneLine { get; set; }
        public int nHaveStopLine { get; set; }
        public int nHaveTrafficLights { get; set; }
        public int nHaveZebra { get; set; }
        public CrossConfig mCrossConfig { get; set; }
        public CrossingInfo mCrossInfo { get; set; }
        public int nCropHeight { get; set; }
        public int nScreenMode { get; set; }
        public int nLaneNumber { get; set; }
        public List<HSysDictInfo> mScreenModeList = new List<HSysDictInfo>();
        public List<HSysDictInfo> mLaneNumberList = new List<HSysDictInfo>();

        public FrmMarkingSet()
        {
            InitializeComponent();
        }

        private void FrmMarkingSet_Load(object sender, EventArgs e)
        {
            nScreenMode = 4;
            nLaneNumber = 3;
            nHaveLaneLine = 1;
            nHaveStopLine = 1;
            nHaveTrafficLights = 1;
            nHaveZebra = 1;
            nCropHeight = 0;

            mScreenModeList = TargetTypeList.GetInstance().mScreenModeList;
            comboBoxEditScreenMode.Properties.Items.Clear();
            foreach(var screenMode in mScreenModeList)
            {
                comboBoxEditScreenMode.Properties.Items.Add(screenMode.SYSDICT_NAME);
            }
            //comboBoxEditScreenMode.SelectedIndex = 3;

            mLaneNumberList = TargetTypeList.GetInstance().mLaneNumberList;
            comboBoxEditLaneNumber.Properties.Items.Clear();
            foreach (var laneNumber in mLaneNumberList)
            {
                comboBoxEditLaneNumber.Properties.Items.Add(laneNumber.SYSDICT_NAME);
            }
            //comboBoxEditLaneNumber.SelectedIndex = 2;

        }

        public void SetControlStatus()
        {
            if(nHaveLaneLine ==1)
            {
                checkEditLaneLine.Checked = true;
            }else{
                checkEditLaneLine.Checked = false;
            }

            if(nHaveStopLine ==1)
            {
                checkEditStopLine.Checked = true;
            }
            else
            {
                checkEditStopLine.Checked = false;
            }

            if(nHaveTrafficLights == 1)
            {
                checkEditTrafficLights.Checked = true;
            }
            else
            {
                checkEditTrafficLights.Checked = false;
            }

            if(nHaveZebra ==1)
            {
                checkEditZebra.Checked = true;
            }
            else
            {
                checkEditZebra.Checked = false;
            }

            if(null!= mCrossConfig)
            {
                textEdit_CropHeight.Text = mCrossConfig.CropHeight + "" ;
            }
            if(null != mCrossInfo)
            {
                textEditCrossName.Text = mCrossInfo.CROSSING_NAME;
                textEditCrossID.Text = mCrossInfo.CROSSING_ID;
            }

            for(int i = 0 ;i< mScreenModeList.Count(); i++)
            {
                if(mScreenModeList[i].SYSDICT_CODE == nScreenMode+"")
                {
                    comboBoxEditScreenMode.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < mLaneNumberList.Count(); i++)
            {
                if (mLaneNumberList[i].SYSDICT_CODE == nLaneNumber+"")
                {
                    comboBoxEditLaneNumber.SelectedIndex = i;
                    break;
                }
            }
        }

        private void simpleBtnSave_Click(object sender, EventArgs e)
        {
            //LaneLine
            if(checkEditLaneLine.Checked == true)
            {
                this.nHaveLaneLine = 1;
            }
            else
            {
                this.nHaveLaneLine = 0;
            }

            //StopLine
            if (checkEditStopLine.Checked == true)
            {
                this.nHaveStopLine = 1;
            }
            else
            {
                this.nHaveStopLine = 0;
            }

            //TrafficLights
            if (checkEditTrafficLights.Checked == true)
            {
                this.nHaveTrafficLights = 1;
            }
            else
            {
                this.nHaveTrafficLights = 0;
            }

            //Zebra
            if (checkEditZebra.Checked == true)
            {
                this.nHaveZebra = 1;
            }
            else
            {
                this.nHaveZebra = 0;
            }

            if (comboBoxEditScreenMode.SelectedIndex >=0)
            {
                int nIndex = comboBoxEditScreenMode.SelectedIndex;
                nScreenMode = Convert.ToInt32(mScreenModeList[nIndex].SYSDICT_CODE);
            }


            if (comboBoxEditLaneNumber.SelectedIndex >= 0)
            {
                int nIndex = comboBoxEditLaneNumber.SelectedIndex;
                nLaneNumber = Convert.ToInt32(mLaneNumberList[nIndex].SYSDICT_CODE);
            }


            nCropHeight = Convert.ToInt32(textEdit_CropHeight.Text.ToString());

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void simpleBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
