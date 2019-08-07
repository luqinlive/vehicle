using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIEVision.COMMON
{
    public class TargetInfoReslt
    {
        public string Id { get; set; }
        //face & target
        public string CalcType { get; set; }
        public string PassTime { get; set; }
        //public string PassTime { get; set; }
        public string CameraID { get; set; }
        public string Location { get; set; }
        public string TargetId { get; set; }
        public int TargetType { get; set; }
        public int BeginFrameIndex { get; set; }
        public int EndFrameIndex { get; set; }
        public int KeyNum { get; set; }
        public List<KeyInfo> KeyInfo { get; set; }

        public TargetFea TargetFea { get; set; }
        public VehicleInfo VehicleInfo { get; set; }
        public HumanInfo HumanInfo { get; set; }
        public OtherInfo OtherInfo { get; set; }
        public string BackGround { get; set; }

        public Image KeyImage { get; set; }
        public List<Image> FaceImages { get; set; }
        public Image BackgroundImage { get; set; }
    }

    public class KeyRect
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class KeyGraph
    {
        public int nChannels { get; set; }
        public string DateTime { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int widthStep { get; set; }
        public string ImageId { get; set; }
        public string imageData { get; set; }
    }

    public class KeyInfo
    {
        public int KeyIndex { get; set; }
        
        public KeyRect KeyRect { get; set; }
        public KeyGraph KeyGraph { get; set; }
    }

    public class TargetFea
    {
        public List<double> AppFeatData { get; set; }
        public List<double> ColorFeatData { get; set; }
    }

    public class VehicleInfo
    {
        public string PlateLicence { get; set; }
        public string PlateColor { get; set; }
        public string MainColorName { get; set; }
        public int MainColorCode { get; set; }
        public string SubColorName { get; set; }
        public int SubColorCode { get; set; }
        public int ColorConfidence { get; set; }
    }

    public class UpperBodyColor
    {
        public string MainColorName { get; set; }
        public int MainColorCode { get; set; }
        public string SubColorName { get; set; }
        public int SubColorCode { get; set; }
        public int ColorConfidence { get; set; }
    }


    public class LowerBodyColor
    {
        public string MainColorName { get; set; }
        public int MainColorCode { get; set; }
        public string SubColorName { get; set; }
        public int SubColorCode { get; set; }
        public int ColorConfidence { get; set; }
    }
    public class HumanInfo
    {
        public UpperBodyColor UpperBodyColor { get; set; }
        public LowerBodyColor LowerBodyColor { get; set; }
    }

    public class OtherInfo
    {
        public UpperBodyColor UpperBodyColor { get; set; }
        public LowerBodyColor LowerBodyColor { get; set; }
    }

}
