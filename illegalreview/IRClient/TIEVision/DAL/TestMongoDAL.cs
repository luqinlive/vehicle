using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIEVision.DAL
{
    public class TestObject
    {

        public ObjectId Id;

        public byte[] BinaryData;

    }
    public class TestMongoDAL
    {
        public static IMongoClient mMongoClient;
        public static IMongoDatabase mMongoDataBase;
        public static IMongoCollection<TestObject> mTestCollection;
        private static TestMongoDAL mContext = null;

        public static TestMongoDAL GetInstance()
        {
            if (mContext == null)
            {
                mContext = new TestMongoDAL();
            }
            return mContext;
        }

        public TestMongoDAL()
        {
            string mongo_connect = System.Configuration.ConfigurationManager.AppSettings["MongoConn"].ToString();
            mMongoClient = new MongoClient(mongo_connect);

            mMongoDataBase = mMongoClient.GetDatabase("ferret");
            mTestCollection = mMongoDataBase.GetCollection<TestObject>("tests");
        }


        public void Add(TestObject obj)
        {
            mTestCollection.InsertOne(obj);
        }


    }
}
