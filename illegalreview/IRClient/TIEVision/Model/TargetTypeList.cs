using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIEVision.Model
{
    enum TIE_CLASS
    {
        HUMAN_PERSON = 0X1,					//PERSON

        VEHC_CAR = 0X100,								//CAR
        VEHC_CART = 0X200,							//CART
        VEHC_LORRY = 0X300,						//LORRY
        VEHC_TRACTOR = 0X400,					//TRACTOR
        VEHC_MIDBUS = 0X500,					//MIDBUS

        OTHER_BIKE = 0X10000,								//BIKE
        OTHER_OILMOTOR = 0X20000,					//OILMOTOR
        OTHER_ELECTRICMOTOR = 0X30000,	//ELECTRICMOTOR
        OTHER_TRIYCLE = 0X40000						//TRIYCLE
    };

    public class TargetTypeList
    {
        public static List<HSysDictInfo> dictListMblx = new List<HSysDictInfo>();
        public static List<HSysDictInfo> mMbysList = new List<HSysDictInfo>();
        public List<HSysDictInfo> mScreenModeList = new List<HSysDictInfo>();
        public List<HSysDictInfo> mLaneNumberList = new List<HSysDictInfo>();
        public List<HSysDictInfo> mLaneTypeList = new List<HSysDictInfo>();
        private static TargetTypeList mContext = null;
        TargetTypeList()
        {
            HSysDictInfo item = new HSysDictInfo();
            item.SYSDICT_CODE = "";
            item.SYSDICT_NAME = "";
            dictListMblx.Add(item);
            item = new HSysDictInfo();
            item.SYSDICT_CODE = ((int)TIE_CLASS.HUMAN_PERSON).ToString();// "1";
            item.SYSDICT_NAME = "行人";
            dictListMblx.Add(item);
            item = new HSysDictInfo();
            item.SYSDICT_CODE = ((int)TIE_CLASS.OTHER_BIKE).ToString();// "65536";
            item.SYSDICT_NAME = "自行车";
            dictListMblx.Add(item);
            item = new HSysDictInfo();
            item.SYSDICT_CODE = ((int)TIE_CLASS.OTHER_OILMOTOR).ToString();// "512";
            item.SYSDICT_NAME = "摩托车";
            dictListMblx.Add(item);
            item = new HSysDictInfo();
            item.SYSDICT_CODE = ((int)TIE_CLASS.OTHER_ELECTRICMOTOR).ToString();// "768";
            item.SYSDICT_NAME = "电动摩托车";
            dictListMblx.Add(item);
            item = new HSysDictInfo();
            item.SYSDICT_CODE = ((int)TIE_CLASS.OTHER_TRIYCLE).ToString();// "1024";
            item.SYSDICT_NAME = "三轮车";
            dictListMblx.Add(item);
            item = new HSysDictInfo();
            item.SYSDICT_CODE = ((int)TIE_CLASS.VEHC_CAR).ToString(); //"256";
            item.SYSDICT_NAME = "小型车";
            dictListMblx.Add(item);
            item = new HSysDictInfo();
            item.SYSDICT_CODE = ((int)TIE_CLASS.VEHC_CART).ToString();//"65536";
            item.SYSDICT_NAME = "大巴";
            dictListMblx.Add(item);
            item = new HSysDictInfo();
            item.SYSDICT_CODE = ((int)TIE_CLASS.VEHC_LORRY).ToString();//"131072";
            item.SYSDICT_NAME = "卡车";
            dictListMblx.Add(item);
            item = new HSysDictInfo();
            item.SYSDICT_CODE = ((int)TIE_CLASS.VEHC_TRACTOR).ToString();//"196608";
            item.SYSDICT_NAME = "拖拉机";
            dictListMblx.Add(item);
            item = new HSysDictInfo();
            item.SYSDICT_CODE = ((int)TIE_CLASS.VEHC_MIDBUS).ToString();// "262144";
            item.SYSDICT_NAME = "中小巴";
            dictListMblx.Add(item);



            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "531"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "2"; model.SYSDICT_NAME = "黑"; model.FLAG = 0;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "532"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "4"; model.SYSDICT_NAME = "灰"; model.FLAG = 0;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "533"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "6"; model.SYSDICT_NAME = "白"; model.FLAG = 0;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "534"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "10"; model.SYSDICT_NAME = "红"; model.FLAG = 0;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "535"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "12"; model.SYSDICT_NAME = "棕"; model.FLAG = 0;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "536"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "14"; model.SYSDICT_NAME = "黄"; model.FLAG = 0;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "537"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "16"; model.SYSDICT_NAME = "绿"; model.FLAG = 0;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "538"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "18"; model.SYSDICT_NAME = "蓝"; model.FLAG = 0;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "539"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "20"; model.SYSDICT_NAME = "粉"; model.FLAG = 0;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "540"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "11"; model.SYSDICT_NAME = "红棕"; model.FLAG = 1;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "541"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "13"; model.SYSDICT_NAME = "棕黄"; model.FLAG = 1;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "542"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "15"; model.SYSDICT_NAME = "黄绿"; model.FLAG = 1;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "543"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "17"; model.SYSDICT_NAME = "蓝绿"; model.FLAG = 1;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "544"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "19"; model.SYSDICT_NAME = "蓝粉"; model.FLAG = 1;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "545"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "3"; model.SYSDICT_NAME = "黑灰"; model.FLAG = 1;
                mMbysList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_ID = "546"; model.SYSDICT_TYPE = "1106"; model.SYSDICT_CODE = "5"; model.SYSDICT_NAME = "灰白"; model.FLAG = 1;
                mMbysList.Add(model);
            }
            //ScreenMode
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_CODE = "1"; model.SYSDICT_NAME = "一分屏";
                mScreenModeList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_CODE = "2"; model.SYSDICT_NAME = "二分屏";
                mScreenModeList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_CODE = "3"; model.SYSDICT_NAME = "三分屏";
                mScreenModeList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_CODE = "4"; model.SYSDICT_NAME = "四分屏";
                mScreenModeList.Add(model);
            }

            //LaneNumber
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_CODE = "1"; model.SYSDICT_NAME = "一车道";
                mLaneNumberList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_CODE = "2"; model.SYSDICT_NAME = "二车道";
                mLaneNumberList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_CODE = "3"; model.SYSDICT_NAME = "三车道";
                mLaneNumberList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_CODE = "4"; model.SYSDICT_NAME = "四车道";
                mLaneNumberList.Add(model);
            }

            //LaneType
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_CODE = "1"; model.SYSDICT_NAME = "直行车道";
                mLaneTypeList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_CODE = "2"; model.SYSDICT_NAME = "右转车道";
                mLaneTypeList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_CODE = "3"; model.SYSDICT_NAME = "左转车道";
                mLaneTypeList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_CODE = "4"; model.SYSDICT_NAME = "直行和右转";
                mLaneTypeList.Add(model);
            }
            {
                HSysDictInfo model = new HSysDictInfo();
                model.SYSDICT_CODE = "5"; model.SYSDICT_NAME = "直行和左转";
                mLaneTypeList.Add(model);
            }


        }

        ~TargetTypeList()
        {

        }
        public static TargetTypeList GetInstance()
        {
            if (mContext == null)
            {
                mContext = new TargetTypeList();

                
            }
            return mContext;
        }

        public List<HSysDictInfo> GetTargetTypeList()
        {
            return dictListMblx;
        }
        public List<HSysDictInfo> GetTargetColorList()
        {
            return mMbysList;
        }


        
    }
}
