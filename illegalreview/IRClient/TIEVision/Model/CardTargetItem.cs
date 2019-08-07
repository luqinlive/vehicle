using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using System.ComponentModel;
using System.Net;
using System.IO;

namespace TIEVision.Model
{
    public class CardTargetItem
    {

        public string FileName { get; set; }
        public Image ShowImage { get; set; }
        public string PassTime { get; set; }
        public string PlateNo { get; set; }
        public string PlateColor { get; set; }
        public string CrossName { get; set; }
        public string TargetRect { get; set; }
        public string ImageName { get; set; }
        public string BackGroundImage { get; set; }

        public string toString()
        {
            return PassTime + " " + PlateNo + " " + PlateColor + " " + CrossName;
        }
      
    }

    public class CardFaceHistory
    {
        public Image ShowImage { get; set; }
        public string PassTime { get; set; }
        public string CrossName { get; set; }
    }

    public class CardTargetBindItem
    {

        public string FileName { get; set; }
        public Image ShowImage { get; set; }
        public string PassTime { get; set; }
        public string PlateNo { get; set; }
        public string PlateColor { get; set; }
        public string CrossName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetImageUrl(string url )
        {
            //Name = name;
            //ShowImage = ResourceImageHelper.CreateImageFromResources("DevExpress.XtraEditors.Images.loading.gif" ,typeof(CardTargetBindItem).Assembly);
            //BackgroundImageLoader bg = new BackgroundImageLoader();
           // bg.Load(url);
            //bg.Loaded += (s, e) =>
            using(WebClient wc = new WebClient())
            {
                ShowImage = BytesToImage(wc.DownloadData(url));
                PropertyChangedEventArgs args = new PropertyChangedEventArgs("ShowImage");
                
                PropertyChanged(this, args);
                
            }
        }

        public static Image BytesToImage(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            Image image = System.Drawing.Image.FromStream(ms);
            return image;
        }

    }
}
