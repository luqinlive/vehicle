using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRVision.UI.Vehicle
{
    public partial class FrmMarkingSet : XtraForm
    {
        public int nHaveLaneLine { get; set; }
        public int nHaveStopLine { get; set; }
        public int nHaveTrafficLights { get; set; }
        public int nHaveZebra { get; set; }

        public FrmMarkingSet()
        {
            InitializeComponent();
        }

        private void FrmMarkingSet_Load(object sender, EventArgs e)
        {
            nHaveLaneLine = 1;
            nHaveStopLine = 1;
            nHaveTrafficLights = 1;
            nHaveZebra = 1;
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
