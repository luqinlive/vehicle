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
using System.IO;
using System.Drawing;
using DevExpress.XtraEditors;

namespace TIEVision.COMMON
{
    public class MongoHelper
    {
        
        public static IMongoClient mMongoClient;
        public static IMongoDatabase mMongoDataBase;
        public static IMongoCollection<TargetInfo> mTargetCollection;
        public static IMongoCollection<OverViewInfo> mOverViewCollection;
        public static IMongoCollection<avCase> mCaseCollection;
        public static IMongoCollection<UserInfo> mUserInfoCollection;
        //public static IMongoCollection<FaceTask> mFaceTaskCollection;
        public static GridFSBucket mGridFSBucket;
        public static string GoHttpGridFs;
        private static MongoHelper mContext = null;
        public static MongoHelper GetInstance()
        {
            if (mContext == null)
            {
                mContext = new MongoHelper();
            }
            return mContext;
        }

        public MongoHelper()
        {
            try
            {
                string mongo_connect = System.Configuration.ConfigurationManager.AppSettings["MongoConn"].ToString();
                mMongoClient = new MongoClient(mongo_connect);
                //mMongoClient.Settings.SocketTimeout = new TimeSpan(5 * 1000);
                mMongoDataBase = mMongoClient.GetDatabase("ferret");
                
                mTargetCollection = mMongoDataBase.GetCollection<TargetInfo>("target");
                mOverViewCollection = mMongoDataBase.GetCollection<OverViewInfo>("overview");
                mCaseCollection = mMongoDataBase.GetCollection<avCase>("avcase");
                mUserInfoCollection = mMongoDataBase.GetCollection<UserInfo>("userinfo");
                //mFaceTaskCollection = mMongoDataBase.GetCollection<FaceTask>("facetask");
                mGridFSBucket = new GridFSBucket(mMongoDataBase);

                GoHttpGridFs = System.Configuration.ConfigurationManager.AppSettings["GoHttp"].ToString();

                CreateIndex();
            }catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
           
        }

        private async Task CreateIndex()
        {
            await mTargetCollection.Indexes.CreateOneAsync(Builders<TargetInfo>.IndexKeys.Ascending(_ => _.PassTime));
            await mTargetCollection.Indexes.CreateOneAsync(Builders<TargetInfo>.IndexKeys.Ascending(_ => _.TargetType));
            await mTargetCollection.Indexes.CreateOneAsync(Builders<TargetInfo>.IndexKeys.Ascending(_ => _.CameraID));
        }

        public string GetCaseXmlInfo()
        {
            try
            {
                var filter = Builders<avCase>.Filter.Empty;
                var result = mCaseCollection.Find(filter).ToList();
                if(null != result)
                {
                    List<avCase> mlist = ((List<avCase>)result);
                    if(mlist.Count >0)
                    {
                        return mlist[0].avcasecfg;
                    }
                }
            }
            catch
            {
               
            }
            return null;
        }

        public List<TargetInfo> GetTargetByTime(string timeStart, string timeEnd, string queryMblx,int PageNumber, int PageSize)
        {
            try
            {
                Stopwatch sw1 = new Stopwatch();
                var builder = Builders<TargetInfo>.Filter;

                var filter = builder.Gt("PassTime", new BsonDateTime(Convert.ToDateTime(timeStart))) &
                    builder.Lt("PassTime", new BsonDateTime(Convert.ToDateTime(timeEnd))) ;
                if(!string.IsNullOrEmpty(queryMblx))
                {
                    //filter = builder.Gt("PassTime", new BsonDateTime(Convert.ToDateTime(timeStart))) &
                    //builder.Lt("PassTime", new BsonDateTime(Convert.ToDateTime(timeEnd)))& ;
                    filter = filter &  builder.Eq("TargetType", queryMblx);
                }
                //& builder.Regex("VehicleInfo.PlateLicence", new  BsonRegularExpression("粤H"))
                
                var sort = Builders<TargetInfo>.Sort.Descending("PassTime");
                sw1.Start();
               
                var cursor = mTargetCollection.Find(filter).Sort(sort).Skip((PageNumber - 1) * PageSize).Limit(PageSize).ToList();
                sw1.Stop();
                Console.WriteLine("方式分页耗时：{0},{1}", sw1.ElapsedMilliseconds, cursor.Count);
                return cursor;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return null;
        }

        public List<TargetInfo> GetTargetByTime(HTargetQuery mTargetQuery)
        {
            try
            {
                Stopwatch sw1 = new Stopwatch();
                var builder = Builders<TargetInfo>.Filter;

                var filter = builder.Gt("PassTime", new BsonDateTime(Convert.ToDateTime(mTargetQuery.timeStart))) &
                    builder.Lt("PassTime", new BsonDateTime(Convert.ToDateTime(mTargetQuery.timeEnd)));
                if(!string.IsNullOrEmpty(mTargetQuery.queryCameraId))
                {
                    string[] cameraIdArray = mTargetQuery.queryCameraId.Replace("'","").Split(',');
                    filter = filter & builder.In("CameraID", cameraIdArray);
                }

                if (!string.IsNullOrEmpty(mTargetQuery.queryMblx))
                {
                    filter = filter & builder.Eq("TargetType", mTargetQuery.queryMblx);
                    if ((HTargetQuery.HUMAN & Convert.ToInt64(mTargetQuery.queryMblx))>0)
                    {
                        if (!string.IsNullOrEmpty(mTargetQuery.queryUpperColor))
                        {
                            string[] upper_color = mTargetQuery.queryUpperColor.Split(',');
                            filter = filter & (builder.In("HumanInfo.UpperBodyColor.MainColorCode", upper_color));
                        }
                        if (!string.IsNullOrEmpty(mTargetQuery.queryLowerColor))
                        {
                            string[] lower_color = mTargetQuery.queryLowerColor.Split(',');
                            filter = filter & (builder.In("HumanInfo.LowerBodyColor.MainColorCode", lower_color));
                        }
                       
                    }
                    if ((HTargetQuery.VEHC & Convert.ToInt64(mTargetQuery.queryMblx))>0)
                    {
                        if (!string.IsNullOrEmpty(mTargetQuery.queryCphm))
                        {
                           
                            filter = filter & builder.Regex("VehicleInfo.PlateLicence", new BsonRegularExpression(mTargetQuery.queryCphm));
                        }
                        if (!string.IsNullOrEmpty(mTargetQuery.queryClys))
                        {
                            string[] vehicle_color = mTargetQuery.queryClys.Split(',');
                            filter = filter & builder.In("VehicleInfo.MainColorCode", vehicle_color);
                        }
                    }
                    if ((HTargetQuery.OTHER & Convert.ToInt64(mTargetQuery.queryMblx))>0)
                    {
                        if (!string.IsNullOrEmpty(mTargetQuery.queryUpperColor))
                        {
                            string[] upper_color = mTargetQuery.queryUpperColor.Split(',');
                            filter = filter & (builder.In("OtherInfo.UpperBodyColor.MainColorCode", upper_color) );
                        }
                        if (!string.IsNullOrEmpty(mTargetQuery.queryLowerColor))
                        {
                            string[] lower_color = mTargetQuery.queryLowerColor.Split(',');
                            filter = filter & (builder.In("OtherInfo.LowerBodyColor.MainColorCode", lower_color) );
                        }
                    }
                    
                }
                else
                {
                    if (mTargetQuery.bQueryVehicle)
                    {
                        if (!string.IsNullOrEmpty(mTargetQuery.queryCphm))
                        {
                            filter = filter & builder.Regex("VehicleInfo.PlateLicence", new BsonRegularExpression(mTargetQuery.queryCphm));
                        }
                        if (!string.IsNullOrEmpty(mTargetQuery.queryClys))
                        {
                            string[] vehicle_color = mTargetQuery.queryClys.Split(',');
                            filter = filter & builder.In("VehicleInfo.MainColorCode", vehicle_color);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(mTargetQuery.queryUpperColor))
                        {
                            string[] upper_color = mTargetQuery.queryUpperColor.Split(',');
                            filter = filter & (builder.In("OtherInfo.UpperBodyColor.MainColorCode", upper_color) | builder.In("HumanInfo.UpperBodyColor.MainColorCode", upper_color));
                        }
                        if (!string.IsNullOrEmpty(mTargetQuery.queryLowerColor))
                        {
                            string[] lower_color = mTargetQuery.queryLowerColor.Split(',');
                            filter = filter & (builder.In("OtherInfo.LowerBodyColor.MainColorCode", lower_color) | builder.In("HumanInfo.LowerBodyColor.MainColorCode", lower_color));
                        }
                    }
                }
                
                var sort = Builders<TargetInfo>.Sort.Descending("PassTime");
                sw1.Start();

                var cursor = mTargetCollection.Find(filter).Sort(sort).Skip((mTargetQuery.PageNumber - 1) * mTargetQuery.PageSize).Limit(mTargetQuery.PageSize).ToList();
                sw1.Stop();
                Console.WriteLine("方式分页耗时：{0},{1}", sw1.ElapsedMilliseconds, cursor.Count);
                return cursor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return null;
        }

        public long GetCountByTime(HTargetQuery mTargetQuery)
        {
            var builder = Builders<TargetInfo>.Filter;

            var filter = builder.Gt("PassTime", new BsonDateTime(Convert.ToDateTime(mTargetQuery.timeStart))) &
                builder.Lt("PassTime", new BsonDateTime(Convert.ToDateTime(mTargetQuery.timeEnd)));
            if (!string.IsNullOrEmpty(mTargetQuery.queryCameraId))
            {
                string[] cameraIdArray = mTargetQuery.queryCameraId.Replace("'", "").Split(',');
                filter = filter & builder.In("CameraID", cameraIdArray);
            }
            if (!string.IsNullOrEmpty(mTargetQuery.queryMblx))
            {
                filter = filter & builder.Eq("TargetType", mTargetQuery.queryMblx);
                if ((HTargetQuery.HUMAN & Convert.ToInt64(mTargetQuery.queryMblx)) > 0)
                {
                    if (!string.IsNullOrEmpty(mTargetQuery.queryUpperColor))
                    {
                        string[] upper_color = mTargetQuery.queryUpperColor.Split(',');
                        filter = filter & (builder.In("HumanInfo.UpperBodyColor.MainColorCode", upper_color));
                    }
                    if (!string.IsNullOrEmpty(mTargetQuery.queryLowerColor))
                    {
                        string[] lower_color = mTargetQuery.queryLowerColor.Split(',');
                        filter = filter & (builder.In("HumanInfo.UpperBodyColor.MainColorCode", lower_color));
                    }

                }
                if ((HTargetQuery.VEHC & Convert.ToInt64(mTargetQuery.queryMblx)) > 0)
                {
                    if (!string.IsNullOrEmpty(mTargetQuery.queryCphm))
                    {
                       
                        filter = filter & builder.Regex("VehicleInfo.PlateLicence", new BsonRegularExpression(mTargetQuery.queryCphm));
                    }
                    if (!string.IsNullOrEmpty(mTargetQuery.queryClys))
                    {
                        string[] vehicle_color = mTargetQuery.queryClys.Split(',');
                        filter = filter & builder.In("VehicleInfo.MainColorCode", vehicle_color);
                    }
                }
                if ((HTargetQuery.OTHER & Convert.ToInt64(mTargetQuery.queryMblx)) > 0)
                {
                    if (!string.IsNullOrEmpty(mTargetQuery.queryUpperColor))
                    {
                        string[] upper_color = mTargetQuery.queryUpperColor.Split(',');
                        filter = filter & (builder.In("OtherInfo.UpperBodyColor.MainColorCode", upper_color));
                    }
                    if (!string.IsNullOrEmpty(mTargetQuery.queryLowerColor))
                    {
                        string[] lower_color = mTargetQuery.queryLowerColor.Split(',');
                        filter = filter & (builder.In("OtherInfo.LowerBodyColor.MainColorCode", lower_color));
                    }
                }

            }
            else
            {
                if (mTargetQuery.bQueryVehicle)
                {
                    if (!string.IsNullOrEmpty(mTargetQuery.queryCphm))
                    {
                        filter = filter & builder.Regex("VehicleInfo.PlateLicence", new BsonRegularExpression(mTargetQuery.queryCphm));
                    }
                    if (!string.IsNullOrEmpty(mTargetQuery.queryClys))
                    {
                        string[] vehicle_color = mTargetQuery.queryClys.Split(',');
                        filter = filter & builder.In("VehicleInfo.MainColorCode", vehicle_color);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(mTargetQuery.queryUpperColor))
                    {
                        string[] upper_color = mTargetQuery.queryUpperColor.Split(',');
                        filter = filter & (builder.In("OtherInfo.UpperBodyColor.MainColorCode", upper_color) | builder.In("HumanInfo.UpperBodyColor.MainColorCode", upper_color));
                    }
                    if (!string.IsNullOrEmpty(mTargetQuery.queryLowerColor))
                    {
                        string[] lower_color = mTargetQuery.queryLowerColor.Split(',');
                        filter = filter & (builder.In("OtherInfo.LowerBodyColor.MainColorCode", lower_color) | builder.In("HumanInfo.UpperBodyColor.MainColorCode", lower_color));
                    }
                }
            }

            return mTargetCollection.Count(filter);
        }


        public List<TargetInfo> GetTargetListByTimeId(ObjectId lastId,string timeStart, string timeEnd, int PageNumber, int PageSize,bool next)
        {
            try
            {
                Stopwatch sw1 = new Stopwatch();
                var builder = Builders<TargetInfo>.Filter;

                var filter = builder.Gt("PassTime", new BsonDateTime(Convert.ToDateTime(timeStart))) &
                    builder.Lt("PassTime", new BsonDateTime(Convert.ToDateTime(timeEnd))) 
                    &builder.Gte("_id",lastId);
                var sort = Builders<TargetInfo>.Sort.Ascending("_id");
                if(!next)
                {
                    filter = builder.Gt("PassTime", new BsonDateTime(Convert.ToDateTime(timeStart))) &
                    builder.Lt("PassTime", new BsonDateTime(Convert.ToDateTime(timeEnd)))
                    & builder.Lte("_id", lastId);
                    sort = Builders<TargetInfo>.Sort.Descending("_id");
                }
                sw1.Start();

                var cursor = mTargetCollection.Find(filter).Sort(sort).Limit(PageSize).ToList();
                sw1.Stop();
                Console.WriteLine("方式分页耗时：{0},{1}", sw1.ElapsedMilliseconds, cursor.Count);
                return cursor;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return null;
        }


        public ObjectId GetTargetLastId(string timeStart, string timeEnd, int PageSize)
        {
            ObjectId object_id = new ObjectId();
            try
            {
                var builder = Builders<TargetInfo>.Filter;
                var filter = builder.Gt("PassTime", new BsonDateTime(Convert.ToDateTime(timeStart))) &
                        builder.Lt("PassTime", new BsonDateTime(Convert.ToDateTime(timeEnd)));
                var cursor = mTargetCollection.Find(filter).Limit(PageSize).ToList();
                if(cursor.Count>0)
                    object_id = cursor[cursor.Count - 1].Id;

            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return object_id;
        }

        public long GetCountByTime(string timeStart, string timeEnd, string queryMblx)
        {
            var builder = Builders<TargetInfo>.Filter;

            var filter = builder.Gt("PassTime", new BsonDateTime(Convert.ToDateTime(timeStart))) &
                builder.Lt("PassTime", new BsonDateTime(Convert.ToDateTime(timeEnd)));

            if (!string.IsNullOrEmpty(queryMblx))
            {
                filter = filter & builder.Eq("TargetType", queryMblx);
            }

            return mTargetCollection.Count(filter);
        }


        public OverViewInfo GetOverviewById(ObjectId objId)
        {
            var filter = Builders<OverViewInfo>.Filter.Eq("_id", objId);
              
            var cursor = mOverViewCollection.Find(filter).ToList();
            List<OverViewInfo> info = (List<OverViewInfo>)cursor;
            if(null != info)
            {
                if(info.Count>0)
                {
                    return info[0];
                }
            }
            return null;
        }

        public byte[] GetGridFilesById(ObjectId objId)
        {
            
            return mGridFSBucket.DownloadAsBytes(objId);
        }

        public Image GetImageFileByName(string mFileName)
        {
            Image img = null;
            try
            {
                MemoryStream ms = new MemoryStream(mGridFSBucket.DownloadAsBytesByName(mFileName));
                img =Image.FromStream(ms);
            }
            catch
            {
                
            }
            
            return img;
        }

        public void UploadImageToGridFS(string imageFileName , byte[] bytes )
        {
            try
            {
                GridFSUploadOptions options = new GridFSUploadOptions();
                options.ChunkSizeBytes = 24 * 1024;
                mGridFSBucket.UploadFromBytes(imageFileName, bytes, options);
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
           
        }


        public bool AddUser(UserInfo userInfo)
        {
             mUserInfoCollection.InsertOne(userInfo);
             return true;
        }

        public bool  SaveTargetResult(TargetInfo targetInfo)
        {
            mTargetCollection.InsertOne(targetInfo);
            return true;
        }

        public bool  CheckUserInfo(UserInfo userInfo)
        {
            bool retVal = false;
            var filter = Builders<UserInfo>.Filter.Eq("UserName", userInfo.UserName);
            var coursor =  mUserInfoCollection.Find(filter).ToList();
            //var count =  mUserInfoCollection.CountAsync(new BsonDocument());
            List<UserInfo> resultUserInfos = (List<UserInfo>)coursor;
            if (null != resultUserInfos)
            {
                if (resultUserInfos.Count > 0)
                {
                    retVal = true;
                }
            }
            return retVal;
        }


        public bool CheckUserLogin(UserInfo userInfo)
        {
            bool retVal = false;
            try
            {
                var builder = Builders<UserInfo>.Filter;
                var filter = builder.Eq("UserName", userInfo.UserName);
                filter = filter & builder.Eq("Password", userInfo.Password);
                var coursor = mUserInfoCollection.Find(filter).ToList();
                List<UserInfo> resultUserInfos = (List<UserInfo>)coursor;
                if (null != resultUserInfos)
                {
                    if (resultUserInfos.Count > 0)
                    {
                        retVal = true;
                    }
                }
            }catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            
          
            return retVal;
        }

        public List<UserInfo> GetAllUser()
        {
            // var builder = Builders<UserInfo>.Filter;
            var coursor = mUserInfoCollection.Find(new BsonDocument()).ToList();
            List<UserInfo> resultUserInfos = (List<UserInfo>)coursor;
            return resultUserInfos;
        }


        public bool  DeleteUserInfo(UserInfo userInfo)
        {
            bool ret = false;
            var builder = Builders<UserInfo>.Filter;
            var filter = builder.Eq("_id", userInfo.Id);
            var result =  mUserInfoCollection.DeleteOne(filter);
            if (result.DeletedCount >=1)
            {
                ret = true;
            }
            return ret;
        }


       
    }
}
