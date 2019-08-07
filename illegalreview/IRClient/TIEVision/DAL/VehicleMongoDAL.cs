using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIEVision.Model;

namespace TIEVision.DAL
{
    public class VehicleMongoDAL
    {
        public static IMongoClient mMongoClient;
        public static IMongoDatabase mMongoDataBase;
        public static IMongoCollection<VehicleObject> mVehicleObjectCollection;
        public static IMongoCollection<VehicleTask> mVehicleTaskCollection;
        public static GridFSBucket mGridFSBucket;
        public static string GoHttpGridFs;
        private static VehicleMongoDAL mContext = null;

        public static VehicleMongoDAL GetInstance()
        {
            if (mContext == null)
            {
                mContext = new VehicleMongoDAL();
            }
            return mContext;
        }

        public VehicleMongoDAL()
        {

            string mongo_connect = System.Configuration.ConfigurationManager.AppSettings["MongoConn"].ToString();
            mMongoClient = new MongoClient(mongo_connect);

            mMongoDataBase = mMongoClient.GetDatabase("ferret");

            mVehicleTaskCollection = mMongoDataBase.GetCollection<VehicleTask>("vehicletask");
            mVehicleObjectCollection = mMongoDataBase.GetCollection<VehicleObject>("vehicles");
            
            mGridFSBucket = new GridFSBucket(mMongoDataBase);

            CreateIndex();

            GoHttpGridFs = System.Configuration.ConfigurationManager.AppSettings["GoHttp"].ToString();
        }

        private async Task CreateIndex()
        {
            //createIndex 
            await mVehicleObjectCollection.Indexes.CreateOneAsync(Builders<VehicleObject>.IndexKeys.Ascending(_ => _.CreateTime));
            await mVehicleObjectCollection.Indexes.CreateOneAsync(Builders<VehicleObject>.IndexKeys.Ascending(_ => _.vehicle.Clpp));
        }

        public bool AddVehicleTask(VehicleTask mFaceTask)
        {
            mVehicleTaskCollection.InsertOne(mFaceTask);
            return true;
        }


        public List<VehicleTask> GetAllVehicleTask()
        {
            var coursor = mVehicleTaskCollection.Find(new BsonDocument()).ToList();
            List<VehicleTask> resultVehicleTasks = (List<VehicleTask>)coursor;
            return resultVehicleTasks;
        }

        public bool UpdateVehicleTask(VehicleTask mFaceTask)
        {
            var filter = Builders<VehicleTask>.Filter.Eq(s => s.Id, mFaceTask.Id);

            mVehicleTaskCollection.ReplaceOne(filter, mFaceTask);
            return true;
        }

        public bool AddVehicleObject(List<VehicleObject> mVehileObjList)
        {
            mVehicleObjectCollection.InsertMany(mVehileObjList);
            return true;
        }

        public long GetVehicleCount()
        {
            return mVehicleObjectCollection.Count(new BsonDocument());
        }

        public long GetVehicleCount(HVehicleQuery mTargetQuery)
        {
            var builder = Builders<VehicleObject>.Filter;
            var filter = builder.Gt("CreateTime", new BsonDateTime(Convert.ToDateTime(mTargetQuery.timeStart))) &
                    builder.Lt("CreateTime", new BsonDateTime(Convert.ToDateTime(mTargetQuery.timeEnd)));
            if (!string.IsNullOrEmpty(mTargetQuery.queryCphm))
            {

                filter = filter & builder.Regex("vehicle.Hphm", new BsonRegularExpression(mTargetQuery.queryCphm));
            }
            if (!string.IsNullOrEmpty(mTargetQuery.queryClys))
            {
                string[] vehicle_color = mTargetQuery.queryClys.Split(',');
                filter = filter & builder.In("vehicle.Csys", vehicle_color);
            }
            if(!string.IsNullOrEmpty(mTargetQuery.queryCllx))
            {
                filter = filter & builder.Eq("vehicle.Cllx", mTargetQuery.queryCllx);
            } 
            if (!string.IsNullOrEmpty(mTargetQuery.queryClpp))
            {
                filter = filter & builder.Regex("vehicle.Clpp", mTargetQuery.queryClpp);
            }
            if(!string.IsNullOrEmpty(mTargetQuery.queryXwtz))
            {
                filter = filter & builder.Regex("vehicle.Xwtz", mTargetQuery.queryXwtz);
            }
            return mVehicleObjectCollection.Count(filter);
        }

        public List<VehicleObject> GetVehicleList(int PageNumber, int PageSize)
        {

            var cursor = mVehicleObjectCollection.Find(new BsonDocument()).Skip((PageNumber - 1) * PageSize).Limit(PageSize).ToList();
            return cursor;
        }

        public List<VehicleObject> GetVehicleList(HVehicleQuery mTargetQuery)
        {
            var builder = Builders<VehicleObject>.Filter;
            var filter = builder.Gt("CreateTime", new BsonDateTime(Convert.ToDateTime(mTargetQuery.timeStart))) &
                    builder.Lt("CreateTime", new BsonDateTime(Convert.ToDateTime(mTargetQuery.timeEnd)));
            if (!string.IsNullOrEmpty(mTargetQuery.queryCphm))
            {

                filter = filter & builder.Regex("vehicle.Hphm", new BsonRegularExpression(mTargetQuery.queryCphm));
            }
            if (!string.IsNullOrEmpty(mTargetQuery.queryClys))
            {
                string[] vehicle_color = mTargetQuery.queryClys.Split(',');
                filter = filter & builder.In("vehicle.Csys", vehicle_color);
            }
            if (!string.IsNullOrEmpty(mTargetQuery.queryCllx))
            {
                filter = filter & builder.Eq("vehicle.Cllx", mTargetQuery.queryCllx);
            }
            if(!string.IsNullOrEmpty(mTargetQuery.queryClpp))
            {
                filter = filter & builder.Regex("vehicle.Clpp", mTargetQuery.queryClpp);
            } 
            if (!string.IsNullOrEmpty(mTargetQuery.queryXwtz))
            {
                filter = filter & builder.Regex("vehicle.Xwtz", mTargetQuery.queryXwtz);
            }
            var sort = Builders<VehicleObject>.Sort.Descending("CreateTime");
            //var cursor = mVehicleObjectCollection.Find(new BsonDocument()).Skip((PageNumber - 1) * PageSize).Limit(PageSize).ToList();
            var cursor = mVehicleObjectCollection.Find(filter).Sort(sort).Skip((mTargetQuery.PageNumber - 1) * mTargetQuery.PageSize).Limit(mTargetQuery.PageSize).ToList();
            return cursor;
        }

        public ObjectId GetVehicleFirstId(HVehicleQuery mTargetQuery)
        {
            var builder = Builders<VehicleObject>.Filter;
            var filter = builder.Gt("CreateTime", new BsonDateTime(Convert.ToDateTime(mTargetQuery.timeStart))) &
                    builder.Lt("CreateTime", new BsonDateTime(Convert.ToDateTime(mTargetQuery.timeEnd)));
            ObjectId object_id = new ObjectId();
            try
            {
                if (!string.IsNullOrEmpty(mTargetQuery.queryClpp))
                {
                    filter = filter & builder.Regex("vehicle.Clpp", mTargetQuery.queryClpp);
                } 
                //var sort = Builders<VehicleObject>.Sort.Descending("CreateTime");
                var cursor = mVehicleObjectCollection.Find(filter).Limit(1).ToList();
                if (cursor.Count > 0)
                    object_id = cursor[cursor.Count - 1].Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return object_id;
        }

        public List<VehicleObject> GetVehicleListGtId(ObjectId id, HVehicleQuery mTargetQuery)
        {
            var builder = Builders<VehicleObject>.Filter;
            var filter = builder.Gt("CreateTime", new BsonDateTime(Convert.ToDateTime(mTargetQuery.timeStart))) &
                    builder.Lt("CreateTime", new BsonDateTime(Convert.ToDateTime(mTargetQuery.timeEnd)));
            if (!string.IsNullOrEmpty(mTargetQuery.queryClpp))
            {
                filter = filter & builder.Regex("vehicle.Clpp", mTargetQuery.queryClpp);
            } 
            filter =filter& builder.Gte("_id", id);
            //var sort = Builders<VehicleObject>.Sort.Ascending("CreateTime");
            var cursor = mVehicleObjectCollection.Find(filter).Limit(mTargetQuery.PageSize).ToList();
            return cursor;
        }
    }
}
