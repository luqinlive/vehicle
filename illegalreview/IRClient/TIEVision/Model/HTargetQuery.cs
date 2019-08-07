using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIEVision.Model
{
    public class HTargetQuery
    {

        public static long  HUMAN	= 	0XFF;						//行人
        public static long VEHC = 0XFF00;			//车辆
        public static long OTHER = 0XFF0000;		//其他
        public string timeStart{get;set;} 
        public string timeEnd{get;set;}
        public string queryCameraId { get; set; }
        public string queryMblx {get;set;}
        public string queryCphm { get; set; }
        public string queryClys { get; set; }
        public string queryUpperColor { get; set; }
        public string queryLowerColor { get; set; }
        public int PageNumber {get;set;}
        public int PageSize { get; set; }
        public bool bQueryVehicle { get; set; }
    }

    public class HVehicleQuery
    {
        public string timeStart { get; set; }
        public string timeEnd { get; set; }
        public string queryCphm { get; set; }
        public string queryClys { get; set; }
        public string queryCllx { get; set; }
        public string queryClpp { get; set; }
        public string queryXwtz { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
