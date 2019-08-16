using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRVision.Model
{
    public class CrossingInfo
    {
        public string ID { get; set; }
        public string CROSSING_ID { get; set; }
        public string CREATE_TIME { get; set; }
        public string CROSSING_NAME { get; set; }
        public string IMAGE_DATA { get; set; }
        public string CROSSING_CONFIG { get; set; }
        public string DESC { get; set; }
    }

    public class CrossConfig
    {
        public long ScreenMode { get; set; }
        public long LaneNumber { get; set; }
        public long CropHeight { get; set; }
        public LaneLine LaneLine { get; set; }
        public StopLine StopLine { get; set; }
        public ZebraCrossing ZebraCrossing { get; set; }
        public TrafficLight TrafficLight { get; set; }
    }

    public class LaneLine
    {
        public long HaveLine { get; set; }
        public long LineNumber { get; set; }
        public List<LinePosition> LinePosition { get; set; }
    }

    public class LinePosition
    {
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
    }

    public partial class StopLine
    {
        public long HaveLine { get; set; }
        public List<string> Points { get; set; }
    }

    public partial class ZebraCrossing
    {
        public long HaveLine { get; set; }
        public List<string> TrafficPoints { get; set; }
    }

    public partial class TrafficLight
    {
        public long HaveLine { get; set; }
        public List<TrafficLine> TrafficLine { get; set; }
    }

    public partial class TrafficLine
    {
        public List<string> Points { get; set; }
    }

     
}
