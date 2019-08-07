using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TIEVision.Model;
using MongoDB.Driver.GridFS;
using System.Diagnostics;

namespace TIEVision.COMMON
{
    public class FaceMongoDAL
    {

        public static IMongoClient mMongoClient;
        public static IMongoDatabase mMongoDataBase;
        public static IMongoCollection<FaceObject> mFaceObjectCollection;
        public static IMongoCollection<FaceAnalysisObject> mFaceAnaObjectCollection;
        public static IMongoCollection<FaceTask> mFaceTaskCollection;
        public static IMongoCollection<FaceLibraryResource> mFaceLibraryCollection;
        public static IMongoCollection<FaceHitInfo> mFaceHitInfoCollection;
        public static IMongoCollection<FaceAlarmInfo> mFaceAlarmInfoCollection;
        public static IMongoCollection<FaceHitStaticLib> mFaceHitStaticLibCollection;
        public static IMongoCollection<FaceCompareResTask> mFaceCompareResTaskCollection;
        public static GridFSBucket mGridFSBucket;
        public static string GoHttpGridFs;
        private static FaceMongoDAL mContext = null;
        public static FaceMongoDAL GetInstance()
        {
            if (mContext == null)
            {
                mContext = new FaceMongoDAL();
            }
            return mContext;
        }

        public FaceMongoDAL()
        {

            string mongo_connect = System.Configuration.ConfigurationManager.AppSettings["MongoConn"].ToString();
            mMongoClient = new MongoClient(mongo_connect);

            mMongoDataBase = mMongoClient.GetDatabase("ferret");

            mFaceTaskCollection = mMongoDataBase.GetCollection<FaceTask>("faces_task");
            mFaceLibraryCollection = mMongoDataBase.GetCollection<FaceLibraryResource>("faces_libres");
            mFaceObjectCollection = mMongoDataBase.GetCollection<FaceObject>("faces_static");
            mFaceAnaObjectCollection = mMongoDataBase.GetCollection<FaceAnalysisObject>("faces_analyze");
            mFaceHitInfoCollection = mMongoDataBase.GetCollection<FaceHitInfo>("faces_hitinfo");
            mFaceAlarmInfoCollection = mMongoDataBase.GetCollection<FaceAlarmInfo>("faces_alarminfo");
            mFaceHitStaticLibCollection = mMongoDataBase.GetCollection<FaceHitStaticLib>("faces_hitstaticlib");
            mFaceCompareResTaskCollection = mMongoDataBase.GetCollection<FaceCompareResTask>("faces_comparerestask");
            mGridFSBucket = new GridFSBucket(mMongoDataBase);

            GoHttpGridFs = System.Configuration.ConfigurationManager.AppSettings["GoHttp"].ToString();
            CreateIndex();
        }
        private async Task CreateIndex()
        {
            //createIndex 
            await mFaceHitInfoCollection.Indexes.CreateOneAsync(Builders<FaceHitInfo>.IndexKeys.Ascending(_ => _.CreateTime));
            await mFaceAnaObjectCollection.Indexes.CreateOneAsync(Builders<FaceAnalysisObject>.IndexKeys.Ascending(_ => _.CreateTime));
            await mFaceAnaObjectCollection.Indexes.CreateOneAsync(Builders<FaceAnalysisObject>.IndexKeys.Ascending(_ => _.CameraID));
            await mFaceAlarmInfoCollection.Indexes.CreateOneAsync(Builders<FaceAlarmInfo>.IndexKeys.Ascending(_ => _.PassTime));
            await mFaceObjectCollection.Indexes.CreateOneAsync(Builders<FaceObject>.IndexKeys.Ascending(_ => _.LibResId));
            await mFaceHitStaticLibCollection.Indexes.CreateOneAsync(Builders<FaceHitStaticLib>.IndexKeys.Ascending(_ => _.LibCompareId)); 
        }


        public List<FaceTask> GetAllFaceTask()
        {
            var coursor = mFaceTaskCollection.Find(new BsonDocument()).ToList();
            List<FaceTask> resultFaceTasks = (List<FaceTask>)coursor;
            return resultFaceTasks;
        }

        public bool DeleteFaceTask(FaceTask mFaceTask)
        {
            bool ret = false;
            var builder = Builders<FaceTask>.Filter;
            var filter = builder.Eq("_id", mFaceTask.Id);
            mFaceTaskCollection.DeleteOne(filter);
            return true;
        }

        //资源库管理
        public List<FaceLibraryResource> GetAllFaceLibRes()
        {
            var coursor = mFaceLibraryCollection.Find(new BsonDocument()).ToList();
            List<FaceLibraryResource> resultFaceLibRes = (List<FaceLibraryResource>)coursor;
            return resultFaceLibRes;
        }

        public bool AddFaceLibRes(FaceLibraryResource faceLibRes)
        {
            mFaceLibraryCollection.InsertOne(faceLibRes);
            return true;
        }

        public bool  CheckFaceLibResName(string faceLibResName)
        {
            bool ret = false;
            var builder = Builders<FaceLibraryResource>.Filter;
            var filter = builder.Eq("LibResName",faceLibResName);

            var cursor = mFaceLibraryCollection.Find(filter).ToList();
            if (cursor.Count > 0)
            {
                ret = true;
            }
            return ret;
        }

        public bool DeleteFaceLibRes(FaceLibraryResource faceLibRes)
        {
            bool ret = false;
            var builder = Builders<FaceLibraryResource>.Filter;
            var filter = builder.Eq("_id", faceLibRes.Id);
            mFaceLibraryCollection.DeleteOne(filter);
            return true;
        }

        public bool UpdateFaceLibRes(FaceLibraryResource faceLibRes)
        {
            var filter = Builders<FaceLibraryResource>.Filter.Eq(s => s.Id, faceLibRes.Id);
            mFaceLibraryCollection.ReplaceOne(filter, faceLibRes);
            return true;
        }

        public bool AddFaceTask(FaceTask mFaceTask)
        {
            mFaceTaskCollection.InsertOne(mFaceTask);
            return true;
        }

        public bool UpdateFaceTask(FaceTask mFaceTask)
        {
            var filter = Builders<FaceTask>.Filter.Eq(s => s.Id, mFaceTask.Id);

            mFaceTaskCollection.ReplaceOne(filter, mFaceTask);
            return true;
        }

        public bool AddFaceObject(FaceObject mFaceObj)
        {
            mFaceObjectCollection.InsertOne(mFaceObj);
            return true;
        }

        public bool DeleteFaceObject(FaceObject mFaceObj)
        {
            bool ret = false;
            var builder = Builders<FaceObject>.Filter;
            var filter = builder.Eq("_id", mFaceObj.Id);
            var result = mFaceObjectCollection.DeleteOne(filter);
            if (result.DeletedCount >= 1)
            {
                ret = true;
            }
            return ret;
        }

        public bool DeleteFaceObjByLibResId(string faceLibResId)
        {
            bool ret = false;
            var builder = Builders<FaceObject>.Filter;
            var filter = builder.Eq("LibResId", faceLibResId);
            var result = mFaceObjectCollection.DeleteMany(filter);
            if (result.DeletedCount >= 1)
            {
                ret = true;
            }
            return ret;
        }
        public bool AddFaceObject(List<FaceObject> mFaceObj)
        {
            //mFaceObjectCollection.InsertOne(mFaceObj);
            mFaceObjectCollection.InsertMany(mFaceObj);
            return true;
        }

        public bool AddFaceAnaObject(List<FaceAnalysisObject> mFaceAnaObj)
        {
            mFaceAnaObjectCollection.InsertMany(mFaceAnaObj);
            return true;
        }
        public bool AddFaceAnaObject(FaceAnalysisObject mFaceAnaObj)
        {
            mFaceAnaObjectCollection.InsertOne(mFaceAnaObj);
            return true;
        }


        public List<FaceObject> GetFaceList(int PageNumber, int PageSize)
        {
            var sort = Builders<FaceObject>.Sort.Descending("CreateTime");
            var cursor = mFaceObjectCollection.Find(new BsonDocument()).Skip((PageNumber - 1) * PageSize).Limit(PageSize).ToList();
            return cursor;
        }

        public List<FaceObject> GetAllFaceList()
        {
            var cursor = mFaceObjectCollection.Find(new BsonDocument()).ToList();
            return cursor;
        }
        public List<FaceObject> GetAllFaceList(string queryLibRes)
        {
            if (!string.IsNullOrEmpty(queryLibRes))
            {
                var builder = Builders<FaceObject>.Filter;
                string[] LibResArray = queryLibRes.Replace(" ", "").Split(',');
                var filter = builder.In("LibResId", LibResArray);
                var cursor = mFaceObjectCollection.Find(filter).ToList();
                return cursor;
            }
            else
            {
                var cursor = mFaceObjectCollection.Find(new BsonDocument()).ToList();
                return cursor;
            }
        }

        public List<FaceObject> GetFaceListGtId(string queryLibRes, ObjectId id, int PageSize)
        {
            var builder = Builders<FaceObject>.Filter;
            var filter = builder.Gte("_id", id);
            if (!string.IsNullOrEmpty(queryLibRes))
            {
                string[] LibResArray = queryLibRes.Replace(" ", "").Split(',');
                filter = filter & builder.In("LibResId", LibResArray);
                var sort = Builders<FaceObject>.Sort.Ascending("_id");
                return  mFaceObjectCollection.Find(filter).Sort(sort).Limit(PageSize).ToList();
            }
            else
            {
                return mFaceObjectCollection.Find(filter).Limit(PageSize).ToList();
                
            }
           
        }

        public ObjectId GetFaceFirstId(string queryLibRes)
        {
            ObjectId object_id = new ObjectId();
            try
            {
                if (!string.IsNullOrEmpty(queryLibRes))
                {
                    var builder = Builders<FaceObject>.Filter;
                    string[] LibResArray = queryLibRes.Replace(" ", "").Split(',');
                    var filter = builder.In("LibResId", LibResArray);
                    var sort = Builders<FaceObject>.Sort.Ascending("_id");
                    var cursor = mFaceObjectCollection.Find(filter).Sort(sort).Limit(1).ToList();
                    if (cursor.Count > 0)
                        object_id = cursor[cursor.Count - 1].Id;
                }
                else
                {
                    var cursor = mFaceObjectCollection.Find(new BsonDocument()).Limit(1).ToList();
                    if (cursor.Count > 0)
                        object_id = cursor[cursor.Count - 1].Id;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return object_id;
        }

        public long GetFaceCount()
        {
            return mFaceObjectCollection.Count(new BsonDocument());
        }

        public List<FaceObject> GetFaceList(string queryLibRes, int PageNumber, int PageSize)
        {
            //var sort = Builders<FaceObject>.Sort.Descending("CreateTime");
            var builder = Builders<FaceObject>.Filter;
            if (!string.IsNullOrEmpty(queryLibRes))
            {
                string[] LibResArray = queryLibRes.Replace(" ", "").Split(',');
                var filter = builder.In("LibResId", LibResArray);
                var cursor = mFaceObjectCollection.Find(filter).Skip((PageNumber - 1) * PageSize).Limit(PageSize).ToList();
                return cursor;
            }
            else
            {
                var cursor = mFaceObjectCollection.Find(new BsonDocument()).Skip((PageNumber - 1) * PageSize).Limit(PageSize).ToList();
                return cursor;
            }

        }

        public long GetFaceCount(string queryLibRes)
        {
            var builder = Builders<FaceObject>.Filter;
            if (!string.IsNullOrEmpty(queryLibRes))
            {
                string[] LibResArray = queryLibRes.Replace(" ", "").Split(',');
                var filter = builder.In("LibResId", LibResArray);
                return mFaceObjectCollection.Count(filter);
            }
            else
            {
                return mFaceObjectCollection.Count(new BsonDocument());
            }

        }


        //历史分析记录查询
        public long GetFaceAnaCount()
        {
            return mFaceAnaObjectCollection.Count(new BsonDocument());
        }

        public List<FaceAnalysisObject> GetFaceAnaList(int PageNumber, int PageSize)
        {
            var sort = Builders<FaceAnalysisObject>.Sort.Descending("CreateTime");
            var cursor = mFaceAnaObjectCollection.Find(new BsonDocument()).Sort(sort).Skip((PageNumber - 1) * PageSize).Limit(PageSize).ToList();
            return cursor;
        }

        public long GetFaceAnaCount(FaceQueryParam faceQueryParam)
        {
            var builder = Builders<FaceAnalysisObject>.Filter;

            if (!string.IsNullOrEmpty(faceQueryParam.timeStart) && !string.IsNullOrEmpty(faceQueryParam.CrossingName))
            {
                var filter = builder.Gt("CreateTime", new BsonDateTime(Convert.ToDateTime(faceQueryParam.timeStart))) &
                                   builder.Lt("CreateTime", new BsonDateTime(Convert.ToDateTime(faceQueryParam.timeEnd)));
                string[] cameraIdArray = faceQueryParam.CrossingName.Replace("'", "").Split(',');
                filter = filter & builder.In("CameraID", cameraIdArray);
                return mFaceAnaObjectCollection.Count(filter);
            }
            if(!string.IsNullOrEmpty(faceQueryParam.timeStart))
            {
                   var filter = builder.Gt("CreateTime", new BsonDateTime(Convert.ToDateTime(faceQueryParam.timeStart))) &
                   builder.Lt("CreateTime", new BsonDateTime(Convert.ToDateTime(faceQueryParam.timeEnd)));
                   return mFaceAnaObjectCollection.Count(filter);
            }
            if (!string.IsNullOrEmpty(faceQueryParam.CrossingName))
            {
                string[] cameraIdArray = faceQueryParam.CrossingName.Replace("'", "").Split(',');
                var filter = builder.In("CameraID", cameraIdArray);
                return mFaceAnaObjectCollection.Count(filter);
            }
           return 0;
        }

        public List<FaceAnalysisObject> GetFaceAnaList(FaceQueryParam faceQueryParam)
        {
            var builder = Builders<FaceAnalysisObject>.Filter;
            if (!string.IsNullOrEmpty(faceQueryParam.timeStart) && !string.IsNullOrEmpty(faceQueryParam.CrossingName))
            {
                var filter = builder.Gt("CreateTime", new BsonDateTime(Convert.ToDateTime(faceQueryParam.timeStart))) &
                    builder.Lt("CreateTime", new BsonDateTime(Convert.ToDateTime(faceQueryParam.timeEnd)));
                string[] cameraIdArray = faceQueryParam.CrossingName.Replace("'", "").Split(',');
                filter = filter & builder.In("CameraID", cameraIdArray);
                var sort = Builders<FaceAnalysisObject>.Sort.Descending("CreateTime");
                var cursor = mFaceAnaObjectCollection.Find(filter).Sort(sort).Skip((faceQueryParam.PageNumber - 1) * faceQueryParam.PageSize).Limit(faceQueryParam.PageSize).ToList();
                return cursor;
            }
            if (!string.IsNullOrEmpty(faceQueryParam.timeStart))
            {
                var filter = builder.Gt("CreateTime", new BsonDateTime(Convert.ToDateTime(faceQueryParam.timeStart))) &
                    builder.Lt("CreateTime", new BsonDateTime(Convert.ToDateTime(faceQueryParam.timeEnd)));
                var sort = Builders<FaceAnalysisObject>.Sort.Descending("CreateTime");
                var cursor = mFaceAnaObjectCollection.Find(filter).Sort(sort).Skip((faceQueryParam.PageNumber - 1) * faceQueryParam.PageSize).Limit(faceQueryParam.PageSize).ToList();
                return cursor;
            }
            if (!string.IsNullOrEmpty(faceQueryParam.CrossingName))
            {
                string[] cameraIdArray = faceQueryParam.CrossingName.Replace("'", "").Split(',');
                var filter = builder.In("CameraID", cameraIdArray);
                var sort = Builders<FaceAnalysisObject>.Sort.Descending("CreateTime");
                var cursor = mFaceAnaObjectCollection.Find(filter).Sort(sort).Skip((faceQueryParam.PageNumber - 1) * faceQueryParam.PageSize).Limit(faceQueryParam.PageSize).ToList();
                return cursor;
            }
            return null;
        }

        //黑名单
        public bool AddFaceHitInfo(FaceHitInfo faceHitInfo)
        {
            mFaceHitInfoCollection.InsertOne(faceHitInfo);
            return true;
        }

        public bool AddFaceHitInfo(List<FaceHitInfo> faceHitInfoList)
        {
            mFaceHitInfoCollection.InsertMany(faceHitInfoList);
            return true;
        }

        public bool DeleteFaceHitInfo(FaceHitInfo mFaceObj)
        {
            bool ret = false;
            var builder = Builders<FaceHitInfo>.Filter;
            var filter = builder.Eq("_id", mFaceObj.Id);
            var result = mFaceHitInfoCollection.DeleteOne(filter);
            if (result.DeletedCount >= 1)
            {
                ret = true;
            }
            return ret;
        }

        public long GetFaceHitCount()
        {
            return mFaceHitInfoCollection.Count(new BsonDocument());
        }

        public List<FaceHitInfo> GetFaceHitInfoList(int PageNumber, int PageSize)
        {
            var sort = Builders<FaceHitInfo>.Sort.Descending("CreateTime");
            var cursor = mFaceHitInfoCollection.Find(new BsonDocument()).Sort(sort).Skip((PageNumber - 1) * PageSize).Limit(PageSize).ToList();
            return cursor;
        }

        public List<FaceHitInfo> GetFaceHitInfoList()
        {
            var cursor = mFaceHitInfoCollection.Find(new BsonDocument()).ToList();
            return cursor;
        }

        //历史报警记录
        public bool AddFaceAlarmInfo(FaceAlarmInfo faceAlarmInfo)
        {
            mFaceAlarmInfoCollection.InsertOne(faceAlarmInfo);
            return true;
        }

        public bool DeleteFaceAlarmInfo(FaceAlarmInfo mFaceObj)
        {
            bool ret = false;
            var builder = Builders<FaceAlarmInfo>.Filter;
            var filter = builder.Eq("_id", mFaceObj.Id);
            var result = mFaceAlarmInfoCollection.DeleteOne(filter);
            if (result.DeletedCount >= 1)
            {
                ret = true;
            }
            return ret;
        }

        public long GetFaceAlarmCount()
        {
            return mFaceAlarmInfoCollection.Count(new BsonDocument());
        }

        public long GetFaceAlarmCount(FaceQueryParam faceQueryParam)
        {
            var builder = Builders<FaceAlarmInfo>.Filter;
            var filter = builder.Gt("PassTime", new BsonDateTime(Convert.ToDateTime(faceQueryParam.timeStart))) &
                    builder.Lt("PassTime", new BsonDateTime(Convert.ToDateTime(faceQueryParam.timeEnd)));
            return mFaceAlarmInfoCollection.Count(filter);
        }

        public List<FaceAlarmInfo> GetFaceAlarmInfoList(int PageNumber, int PageSize)
        {
            var sort = Builders<FaceAlarmInfo>.Sort.Descending("PassTime");
            var cursor = mFaceAlarmInfoCollection.Find(new BsonDocument()).Sort(sort).Skip((PageNumber - 1) * PageSize).Limit(PageSize).ToList();
            return cursor;
        }

        public List<FaceAlarmInfo> GetFaceAlarmInfoList(FaceQueryParam faceQueryParam)
        {
            var builder = Builders<FaceAlarmInfo>.Filter;
            var filter = builder.Gt("PassTime", new BsonDateTime(Convert.ToDateTime(faceQueryParam.timeStart))) &
                    builder.Lt("PassTime", new BsonDateTime(Convert.ToDateTime(faceQueryParam.timeEnd)));
            var sort = Builders<FaceAlarmInfo>.Sort.Descending("PassTime");
            var cursor = mFaceAlarmInfoCollection.Find(new BsonDocument()).Sort(sort).Skip((faceQueryParam.PageNumber - 1) * faceQueryParam.PageSize).Limit(faceQueryParam.PageSize).ToList();
            return cursor;
        }

        //撞库
        public bool AddFaceHitStaticLib(FaceHitStaticLib faceHitStaticLib)
        {
            mFaceHitStaticLibCollection.InsertOne(faceHitStaticLib);
            return true;
        }

        public long GetFaceHitStaticLibCount()
        {
            return mFaceHitStaticLibCollection.Count(new BsonDocument());
        }
        public long GetFaceHitStaticLibCount(string queryHitLibResId)
        {
            if (!string.IsNullOrEmpty(queryHitLibResId))
            {
                var builder = Builders<FaceHitStaticLib>.Filter;
                string[] LibResArray = queryHitLibResId.Replace(" ", "").Split(',');
                var filter = builder.In("LibCompareId", LibResArray);
                return mFaceHitStaticLibCollection.Find(filter).Count();
            }else
            {
                return mFaceHitStaticLibCollection.Count(new BsonDocument());
            }
        }

        public bool  DeleteFaceHitStaticLibById(string compareId)
        {
            bool ret = false;
            var builder = Builders<FaceHitStaticLib>.Filter;
            var filter = builder.Eq("LibCompareId", compareId);
            var result = mFaceHitStaticLibCollection.DeleteMany(filter);
            if (result.DeletedCount >= 1)
            {
                ret = true;
            }
            return ret;
        }

        public List<FaceHitStaticLib> GetFaceHitStaticLibList(int PageNumber, int PageSize)
        {
            var cursor = mFaceHitStaticLibCollection.Find(new BsonDocument()).Skip((PageNumber - 1) * PageSize).Limit(PageSize).ToList();
            return cursor;
        }


        public List<FaceHitStaticLib> GetFaceHitStaticLibList(string queryHitLibResId , int PageNumber, int PageSize)
        {
            if (!string.IsNullOrEmpty(queryHitLibResId))
            {
                var builder = Builders<FaceHitStaticLib>.Filter;
                string[] LibResArray = queryHitLibResId.Replace(" ", "").Split(',');
                var filter = builder.In("LibCompareId", LibResArray);
                return mFaceHitStaticLibCollection.Find(filter).Skip((PageNumber - 1) * PageSize).Limit(PageSize).ToList(); ;
            }
            else
            {
                var cursor = mFaceHitStaticLibCollection.Find(new BsonDocument()).Skip((PageNumber - 1) * PageSize).Limit(PageSize).ToList();
                return cursor;
            }
        }


        //库比对任务
        public bool DeleteFaceCompareResTask(FaceCompareResTask faceCompareResTask)
        {
            bool ret = false;
            var builder = Builders<FaceCompareResTask>.Filter;
            var filter = builder.Eq("_id", faceCompareResTask.Id);
            mFaceCompareResTaskCollection.DeleteOne(filter);
            return true;
        }

        public bool AddFaceCompareResTask(FaceCompareResTask faceCompareResTask)
        {
            mFaceCompareResTaskCollection.InsertOne(faceCompareResTask);
            return true;
        }

        public long GetFaceCompareResTaskCount()
        {
            return mFaceCompareResTaskCollection.Count(new BsonDocument());
        }

        public List<FaceCompareResTask> GetFaceCompareResTaskList()
        {
            var cursor = mFaceCompareResTaskCollection.Find(new BsonDocument()).ToList();
            return cursor;
        }

        public bool UpdateFaceCompareTask(FaceCompareResTask faceCompareResTask)
        {
            var filter = Builders<FaceCompareResTask>.Filter.Eq(s => s.Id, faceCompareResTask.Id);

            mFaceCompareResTaskCollection.ReplaceOne(filter, faceCompareResTask);
            return true;
        }
    }
}
