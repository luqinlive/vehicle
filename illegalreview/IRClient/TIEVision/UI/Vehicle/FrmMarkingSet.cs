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

        public int nTrafficNumber1 { get; set; }
        public int nTrafficNumber2 { get; set; }
        public int nTrafficNumber3 { get; set; }
        public int nTrafficNumber4 { get; set; }

        //LaneNumber
        public int nLaneNumber1 { get; set; }
        public int nLaneNumber2 { get; set; }
        public int nLaneNumber3 { get; set; }
        public int nLaneNumber4 { get; set; }
        public string mLaneType1 { get; set; }
        public string mLaneType2 { get; set; }
        public string mLaneType3 { get; set; }
        public string mLaneType4 { get; set; }



        public List<HSysDictInfo> mScreenModeList = new List<HSysDictInfo>();
        public List<HSysDictInfo> mLaneNumberList = new List<HSysDictInfo>();
        public List<HSysDictInfo> mLaneTypeList = new List<HSysDictInfo>();

        public FrmMarkingSet()
        {
            InitializeComponent();
        }

        private void FrmMarkingSet_Load(object sender, EventArgs e)
        {
            

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

            mLaneTypeList = TargetTypeList.GetInstance().mLaneTypeList;
            comboBoxEdit_LaneType1.Properties.Items.Clear();
            comboBoxEdit_LaneType2.Properties.Items.Clear();
            comboBoxEdit_LaneType3.Properties.Items.Clear();
            comboBoxEdit_LaneType4.Properties.Items.Clear();

            comboBoxEdit_TrafficLane1.Properties.Items.Clear();
            comboBoxEdit_TrafficLane2.Properties.Items.Clear();
            comboBoxEdit_TrafficLane3.Properties.Items.Clear();
            comboBoxEdit_TrafficLane4.Properties.Items.Clear();

            foreach (var laneType in mLaneTypeList)
            {
                comboBoxEdit_LaneType1.Properties.Items.Add(laneType.SYSDICT_NAME);
                comboBoxEdit_LaneType2.Properties.Items.Add(laneType.SYSDICT_NAME);
                comboBoxEdit_LaneType3.Properties.Items.Add(laneType.SYSDICT_NAME);
                comboBoxEdit_LaneType4.Properties.Items.Add(laneType.SYSDICT_NAME);

                comboBoxEdit_TrafficLane1.Properties.Items.Add(laneType.SYSDICT_NAME);
                comboBoxEdit_TrafficLane2.Properties.Items.Add(laneType.SYSDICT_NAME);
                comboBoxEdit_TrafficLane3.Properties.Items.Add(laneType.SYSDICT_NAME);
                comboBoxEdit_TrafficLane4.Properties.Items.Add(laneType.SYSDICT_NAME);

            }


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

            //TrafficLights
            if(nTrafficNumber1 == 1)
            {
                checkEdit_TrafficLights1.Checked = true;
            }
            else
            {
                checkEdit_TrafficLights1.Checked = false;

            }
            if (nTrafficNumber2 == 1)
            {
                checkEdit_TrafficLights2.Checked = true;
            }
            else
            {
                checkEdit_TrafficLights2.Checked = false;

            }
            if (nTrafficNumber3 == 1)
            {
                checkEdit_TrafficLights3.Checked = true;
            }
            else
            {
                checkEdit_TrafficLights3.Checked = false;

            }
            if (nTrafficNumber4 == 1)
            {
                checkEdit_TrafficLights4.Checked = true;
            }
            else
            {
                checkEdit_TrafficLights4.Checked = false;

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


            //LaneNumber 
            if(nLaneNumber1 ==1 )
            {
                checkEdit_Lane1.Checked = true;
            }
            else
            {
                checkEdit_Lane1.Checked = false;
            }
            if (nLaneNumber2 == 1)
            {
                checkEdit_Lane2.Checked = true;
            }
            else
            {
                checkEdit_Lane2.Checked = false;
            }
            if (nLaneNumber3 == 1)
            {
                checkEdit_Lane3.Checked = true;
            }
            else
            {
                checkEdit_Lane3.Checked = false;
            }
            if (nLaneNumber4 == 1)
            {
                checkEdit_Lane4.Checked = true;
            }
            else
            {
                checkEdit_Lane4.Checked = false;
            }

            //LaneType 
            if(!string.IsNullOrEmpty(mLaneType1))
            {
                string[] laneCodeType = mLaneType1.Split(',');
                comboBoxEdit_LaneType1.SelectedIndex = Convert.ToInt32(laneCodeType[0]) -1;
            }
            else
            {
                comboBoxEdit_LaneType1.SelectedIndex = -1;
            }
            if (!string.IsNullOrEmpty(mLaneType2))
            {
                string[] laneCodeType = mLaneType2.Split(',');
                comboBoxEdit_LaneType2.SelectedIndex = Convert.ToInt32(laneCodeType[0]) - 1;
               
            }
            else
            {
                comboBoxEdit_LaneType2.SelectedIndex = -1;
            }
           
            if (!string.IsNullOrEmpty(mLaneType3))
            {
                string[] laneCodeType = mLaneType3.Split(',');
                comboBoxEdit_LaneType3.SelectedIndex = Convert.ToInt32(laneCodeType[0]) - 1;
                 
            }
            else
            {
                comboBoxEdit_LaneType3.SelectedIndex = -1;
            }
            if (!string.IsNullOrEmpty(mLaneType4))
            {
                string[] laneCodeType = mLaneType4.Split(',');
                comboBoxEdit_LaneType4.SelectedIndex = Convert.ToInt32(laneCodeType[0]) - 1;


            }
            else
            {
                comboBoxEdit_LaneType4.SelectedIndex = -1;
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

            if (comboBoxEdit_LaneType1.SelectedIndex>=0)
            {
                int nIndex = comboBoxEdit_LaneType1.SelectedIndex;
                mLaneType1 = mLaneTypeList[nIndex].SYSDICT_CODE + "," + mLaneTypeList[nIndex].SYSDICT_NAME;
            }
            else
            {
                mLaneType1 = "-1,null";
            }
            if (comboBoxEdit_LaneType2.SelectedIndex >= 0)
            {
                int nIndex = comboBoxEdit_LaneType2.SelectedIndex;
                mLaneType2 = mLaneTypeList[nIndex].SYSDICT_CODE + "," + mLaneTypeList[nIndex].SYSDICT_NAME;
            }
            else
            {
                mLaneType2 = "-1,null";
            }
            if (comboBoxEdit_LaneType3.SelectedIndex >= 0)
            {
                int nIndex = comboBoxEdit_LaneType3.SelectedIndex;
                mLaneType3 = mLaneTypeList[nIndex].SYSDICT_CODE + "," + mLaneTypeList[nIndex].SYSDICT_NAME;
            }
            else
            {
                mLaneType3 = "-1,null";
            }
            if (comboBoxEdit_LaneType4.SelectedIndex >= 0)
            {
                int nIndex = comboBoxEdit_LaneType4.SelectedIndex;
                mLaneType4 = mLaneTypeList[nIndex].SYSDICT_CODE + "," + mLaneTypeList[nIndex].SYSDICT_NAME;
            }
            else
            {
                mLaneType4 = "-1,null";
            }

            if (checkEdit_Lane1.Checked == true) { this.nLaneNumber1 = 1; } else { this.nLaneNumber1 = 0; mLaneType1 = "-1,null"; }
            if (checkEdit_Lane2.Checked == true) { this.nLaneNumber2 = 1; } else { this.nLaneNumber2 = 0; mLaneType2 = "-1,null"; }
            if (checkEdit_Lane3.Checked == true) { this.nLaneNumber3 = 1; } else { this.nLaneNumber3 = 0; mLaneType3 = "-1,null"; }
            if (checkEdit_Lane4.Checked == true) { this.nLaneNumber4 = 1; } else { this.nLaneNumber4 = 0; mLaneType4 = "-1,null"; }

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
            if (checkEdit_TrafficLights1.Checked == true) { this.nTrafficNumber1 = 1; } else { this.nTrafficNumber1 = 0; }
            if (checkEdit_TrafficLights2.Checked == true) { this.nTrafficNumber2 = 1; } else { this.nTrafficNumber2 = 0; }
            if (checkEdit_TrafficLights3.Checked == true) { this.nTrafficNumber3 = 1; } else { this.nTrafficNumber3 = 0; }
            if (checkEdit_TrafficLights4.Checked == true) { this.nTrafficNumber4 = 1; } else { this.nTrafficNumber4 = 0; }


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
