using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIEVision.Model
{
    public class VehicleTask
    {
        public ObjectId Id { get; set; }
        public string TaskId { get; set; }
        public string TaskPath { get; set; }
        public string TaskCount { get; set; }
        public string TaskState { get; set; }
        public BsonDateTime CreateTime { get; set; }
    }

    public partial class VehicleRecogResult
    {
        public long Code { get; set; }
        public string Gcxh { get; set; }
        public long Jls { get; set; }
        public string Msg { get; set; }
        public long Tpqxd { get; set; }
        public List<Veh> Veh { get; set; }
    }

    public partial class Veh
    {
        public string Cbdm { get; set; }
        public string Cllx { get; set; }
        public string Clpp { get; set; }
        public string Cltzxx { get; set; }
        public string Clwz { get; set; }
        public string Csys { get; set; }
        public string Fjswz { get; set; }
        public string Hphm { get; set; }
        public long Hpkxd { get; set; }
        public string Hpzl { get; set; }
        public string Jsrwz { get; set; }
        public string Njbwz { get; set; }
        public long Ppkxd { get; set; }
        public string Sbwz { get; set; }
        public long Sxh { get; set; }
        public long Tzkxd { get; set; }
        public string Xwtz { get; set; }
        public long Ywhp { get; set; }
        public long Ywnjb { get; set; }
    }

    public class VehicleObject
    {
        public ObjectId Id { get; set; }
        public string TaskId { get; set; }
       
        public BsonDateTime CreateTime { get; set; }
        public string ImagePath { get; set; }

        public Veh vehicle { get; set; }
        public string ToString()
        {
            string retStr = "";
            retStr += "时间：" + CreateTime.AsDateTime.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss") + "  ";
            retStr += "车型：" + vehicle.Clpp + "  ";
            retStr += "车牌：" + vehicle.Hphm + "  ";
            foreach(var item in VehicleDictList.GetInstance().GetVehicleTypeList())
            {
                if(item.SYSDICT_CODE == vehicle.Cllx)
                {
                    retStr += "车辆类型：" + item.SYSDICT_NAME + "  ";
                    break;
                }
            }
            retStr += "行为特征：";
            foreach(var item in VehicleDictList.GetInstance().GetVehicleXwtz())
            {
                if(vehicle.Xwtz.Contains(item.SYSDICT_CODE) )
                {
                    retStr+=item.SYSDICT_NAME+" ";
                }
            }
            return retStr;
        }
    }

    public class VehicleCompareResult
    {
        public string ImagePath { get; set; }
        public string CreateTime { get; set; }
        public string Cphm { get; set; }
        public string Clwz { get; set; }
        public string Clpp { get; set; }

        public float Score { get; set; }
    }

    public class VehicleCompareSort : IComparer<VehicleCompareResult>
    {

        public int Compare(VehicleCompareResult x, VehicleCompareResult y)
        {
            if (x.Score > y.Score)
            {
                return -1;
            }
            else
            {
                if (x.Score == y.Score)
                {
                    return 0;
                }
                return 1;
            }
        }
    }

}
