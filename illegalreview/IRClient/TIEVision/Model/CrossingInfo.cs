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
        public long CropHeight { get; set; }
        public LaneLine LaneLine { get; set; }
        public List<string> StopLine { get; set; }
        public List<string> ZebraCrossing { get; set; }
        public List<string> TrafficLight { get; set; }
    }

    public class LaneLine
    {
        public long LineNumber { get; set; }
        public List<LinePosition> LinePosition { get; set; }
    }

    public class LinePosition
    {
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
    }

     
}
