using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIEVision.Model
{
    public class FaceTask
    {
         public ObjectId Id { get; set; }
         public string TaskId { get; set; }
         public string TaskPath { get; set; }
         public string TaskCount { get; set; }
         public string TaskState { get; set; }
         public BsonDateTime CreateTime { get; set; }
    }

    public class FaceLibraryResource
    {
        public ObjectId Id { get; set; }
        public string LibResId { get; set; }
        public string LibResName { get; set; }
        public string LibResCount { get; set; }
        public BsonDateTime CreateTime { get; set; }
    }

    public class FaceObject
    {
        public ObjectId Id { get; set; }
        public string TaskId { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Sex { get; set; }
        public string IdCard { get; set; }
        public string Age { get; set; }
        public string Nation { get; set; }
        public string Description { get; set; }
        public string ServiceType { get; set; }
        public string LibResId { get; set; }
        public BsonDateTime CreateTime { get; set; }
        public string ImagePath { get; set; }
        public string BackImagePath { get; set; }
        public string FaceFeature { get; set; }

        public string toString()
        {
            string retStr = "";
            if(!string.IsNullOrEmpty(Name))
            {
                retStr += " 姓名:" + Name;
            }
            if (!string.IsNullOrEmpty(IdCard))
            {
                retStr += " 身份证:" + IdCard;
            }
            if (!string.IsNullOrEmpty(Description))
            {
                retStr += " 描述:" + Description;
            }
            return retStr;
        }
    }

    public class FaceAnalysisObject
    {
        public ObjectId Id { get; set; }
        public BsonDateTime CreateTime { get; set; }
        public string CameraID { get; set; }
        public string KeyIndex { get; set; }
        public string ImageName { get; set; }
        public string BackGroundImage { get; set; }
        public int IsFeatured { get; set; }
        public string FaceFeature { get; set; }
    }

    public class FaceAnaHitObject
    {
        public string CreateTime { get; set; }
        public string ImageName { get; set; }
        public string BackGroundImage { get; set; }
        public string FaceFeature { get; set; }
        public string CrossName { get; set; }
    }


    public class FaceCompareResult
    {
        public string ImagePath { get; set; }
        public string CreateTime { get; set; }
        public string IdCard { get; set; }
        public string LibResId { get; set; }
        public float Score { get; set; }
        public string BackGroundImage { get; set; }
    }

    public class FaceCompareSort : IComparer<FaceCompareResult>
    {

        public int Compare(FaceCompareResult x, FaceCompareResult y)
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

    //黑名单
    public class FaceHitInfo
    {
        public ObjectId Id { get; set; }
        public BsonDateTime CreateTime { get; set; }
        public string ImageName { get; set; }
        public string ShowName { get; set; }
        public string FaceFeature { get; set; }

        public string ThresholdCorrections { get; set; }

    }

    public class FaceAlarmInfo
    {
        public ObjectId Id { get; set; }
        public BsonDateTime CreateTime { get; set; }
        public BsonDateTime PassTime { get; set; }
        public string AlarmImageName { get; set; }
        public string HitImageName { get; set; }
        public string BackGroundImage { get; set; }
        public string ShowName { get; set; }
        public string CrossName { get; set; }

        public string ThresholdResult { get; set; }
    }


    public class CardFaceAlarmItem
    {

        public string FileName { get; set; }
        public Image ShowImage { get; set; }
        public Image ShowImage2 { get; set; }
        public string PassTime { get; set; }
        public string PlateNo { get; set; }
        public string PlateColor { get; set; }
        public string CrossName { get; set; }
        public string TargetRect { get; set; }

        public string toString()
        {
            return PassTime + " " + PlateNo + " " + PlateColor + " " + CrossName;
        }

    }

    public class FaceQueryParam
    {
        public string timeStart { get; set; }
        public string timeEnd { get; set; }
        public string CrossingName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }

    public class FaceHitStaticLib
    {
        public ObjectId Id { get; set; }
        public string LibCompareId { get; set; }
        public BsonDateTime CreateTime { get; set; }
        public string LibResId { get; set; }
        public string ImageName { get; set; }
        public FaceObject SrcFaceObject { get; set; }
        public string BackGroundImage { get; set; }
        public List<FaceObject> faceObject { get; set; }
        public List<double> Socre { get; set; }
    }

    public class FaceHitStaticLibCompareResult
    {
        public float Score { get; set; }
        public FaceObject faceObj { get; set; }
    }

    public class FaceHitStaticLibSort : IComparer<FaceHitStaticLibCompareResult>
    {

        public int Compare(FaceHitStaticLibCompareResult x, FaceHitStaticLibCompareResult y)
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


    //库比库比对任务
    public class FaceCompareResTask
    {
        public ObjectId Id { get; set; }
        public string LibCompareId { get; set; }
        public string LibResType1 { get; set; }
        public string LibResId1 { get; set; }
        public string LibResName1 { get; set; }
        public string LibResType2 { get; set; }
        public string LibResId2 { get; set; }
        public string LibResName2 { get; set; }
        public string CompareTaskName { get; set; }
        public string LibCompareState { get; set; }
        public BsonDateTime CreateTime { get; set; }
    }

    public class FaceCompareResTaskGrid
    {
        public string LibCompareId { get; set; }
        public string LibResId1 { get; set; }
        public string LibResName1 { get; set; }
        public string LibResId2 { get; set; }
        public string LibResName2 { get; set; }
        public string CompareTaskName { get; set; }
        public string LibCompareState { get; set; }
        public string CreateTime { get; set; }
    }


}
