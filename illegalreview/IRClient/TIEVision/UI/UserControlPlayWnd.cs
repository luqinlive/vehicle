using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TIEVison.COMMON;
using TIEVision.COMMON;

namespace TIEVision.UI
{
    public partial class UserControlPlayWnd : UserControl
    {
        private bool bPlayStatus = false;
        private string mSelectedVideoId = "";
        public UserControlPlayWnd()
        {
            InitializeComponent();
        }

        private void UserControlPlayWnd_Load(object sender, EventArgs e)
        {
            //panelControl2.Visible = false;
            SetButtonsImage();
        }


        public IntPtr GetPlayWndHandle()
        {
            return panelControl1.Handle;
        }

        private void panelControl1_MouseEnter(object sender, EventArgs e)
        {
           // panelControl2.Visible = true;
        }

        private void panelControl1_MouseLeave(object sender, EventArgs e)
        {
            //panelControl2.Visible = false;
        }

        public void SetPlayStatus(bool PlayStatus)
        {
            this.bPlayStatus = PlayStatus;
            if(bPlayStatus)
            {
                var type = FontAwesome.ParseType("PauseCircle");
                simpleButtonPlay.Image = FontAwesome.Instance.GetImage(
                    new FontAwesome.Properties(type)
                    {
                        ForeColor = Color.White,
                        Size = 20,
                        BackColor = Color.Transparent,
                        BorderColor = Color.Transparent,
                        ShowBorder = false
                    });
            }
        }

        public void SetPlayId(string VideoId)
        {
            mSelectedVideoId = VideoId;
        }

        public string GetPlayId()
        {
            return mSelectedVideoId;
        }

        public void SetTrackBarDuration(int Duration)
        {
            trackBarControl1.Properties.Maximum = Duration;
        }

        public void SetTrackBarValue(int TrackValue)
        {
            trackBarControl1.Value = TrackValue;
        }
        
        private void SetButtonsImage()
        {
            {
                var type = FontAwesome.ParseType("PlayCircle");
                simpleButtonPlay.Image = FontAwesome.Instance.GetImage(
                    new FontAwesome.Properties(type)
                    {
                        ForeColor = Color.White,
                        Size = 20,
                        BackColor = Color.Transparent,
                        BorderColor = Color.Transparent,
                        ShowBorder = false
                    });
            }


            {
                var type = FontAwesome.ParseType("StopCircle");
                simpleButtonStop.Image = FontAwesome.Instance.GetImage(
                    new FontAwesome.Properties(type)
                    {
                        ForeColor = Color.White,
                        Size = 20,
                        BackColor = Color.Transparent,
                        BorderColor = Color.Transparent,
                        ShowBorder = false
                    });
            }

            {
                var type = FontAwesome.ParseType("StepBackward");
                simpleBtnPrevsFrame.Image = FontAwesome.Instance.GetImage(
                    new FontAwesome.Properties(type)
                    {
                        ForeColor = Color.White,
                        Size = 20,
                        BackColor = Color.Transparent,
                        BorderColor = Color.Transparent,
                        ShowBorder = false
                    });
            }
            {
                var type = FontAwesome.ParseType("StepForward");
                simpleBtnNextFrame.Image = FontAwesome.Instance.GetImage(
                    new FontAwesome.Properties(type)
                    {
                        ForeColor = Color.White,
                        Size = 20,
                        BackColor = Color.Transparent,
                        BorderColor = Color.Transparent,
                        ShowBorder = false
                    });
            }


        }

        private void simpleButtonStop_Click(object sender, EventArgs e)
        {
            HNodeLib.VideoPlayControlStop(mSelectedVideoId);
        }

        private void simpleButtonPlay_Click(object sender, EventArgs e)
        {
            if (bPlayStatus)
            {
                {
                    var type = FontAwesome.ParseType("PlayCircle");
                    simpleButtonPlay.Image = FontAwesome.Instance.GetImage(
                        new FontAwesome.Properties(type)
                        {
                            ForeColor = Color.White,
                            Size = 20,
                            BackColor = Color.Transparent,
                            BorderColor = Color.Transparent,
                            ShowBorder = false
                        });
                }
                LogHelper.WriteLog(typeof(UserControlPlayWnd), "Pause Control:"+  mSelectedVideoId);
                HNodeLib.VideoPlayControlPause(mSelectedVideoId);
                bPlayStatus = false;
            }
            else
            {
                {
                    var type = FontAwesome.ParseType("PauseCircle");
                    simpleButtonPlay.Image = FontAwesome.Instance.GetImage(
                        new FontAwesome.Properties(type)
                        {
                            ForeColor = Color.White,
                            Size = 20,
                            BackColor = Color.Transparent,
                            BorderColor = Color.Transparent,
                            ShowBorder = false
                        });
                }

                LogHelper.WriteLog(typeof(UserControlPlayWnd), "Play Control:" + mSelectedVideoId);
                HNodeLib.VideoPlayControl(mSelectedVideoId, 1);
                bPlayStatus = true;
            }
        }

        private void simpleBtnPrevsFrame_Click(object sender, EventArgs e)
        {
            HNodeLib.VideoPlayControlPrevsFrame(mSelectedVideoId);
        }

        private void simpleBtnNextFrame_Click(object sender, EventArgs e)
        {
            HNodeLib.VideoPlayControlNextFrame(mSelectedVideoId);
        }
    }
}
