using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;
using Google.Protobuf;
using TIEVision.Model;
using MongoDB.Bson;
using TIEVision.DAL;
using TIEVison.COMMON;

namespace TIEVision.COMMON
{
    public class ZmqVehicleSink
    {
        public static ZmqVehicleSink mContext = null;
        public static ZContext context = new ZContext();
        private ZSocket socket;
        private List<ZSocket> receiverSocketList = new List<ZSocket>();
        private Thread thReceiverMsg = null;
        private bool receiving = true;

        public static ZmqVehicleSink GetInstance()
        {
            if (mContext == null)
            {
                mContext = new ZmqVehicleSink();
            }
            return mContext;
        }

        public ZmqVehicleSink()
        {
            socket = new ZSocket(context, ZSocketType.PUSH);
            socket.Bind("inproc://vehiclesink");


            System.Threading.WaitCallback waitCallback = new WaitCallback(ReceiveMsg);
            for (int i = 0; i < 4; i++)
            {
                ZSocket receiver = new ZSocket(context, ZSocketType.PULL);
                receiver.Connect("inproc://vehiclesink");
                receiverSocketList.Add(receiver);
                ThreadPool.QueueUserWorkItem(waitCallback, i );
            }

            

            //if (thReceiverMsg != null)
            //{
            //    thReceiverMsg.Abort();
            //}
            //thReceiverMsg = new Thread(new ThreadStart(ReceiveMsg));
            //thReceiverMsg.Start();

        }


        public void SendVehicles(List<string> vehicles)
        {
            try
            {
                string vehicleList = JsonConvert.SerializeObject(vehicles);
                socket.Send(new ZFrame(System.Text.Encoding.UTF8.GetBytes(vehicleList)));
                LogHelper.WriteLog(typeof(ZmqVehicleSink), "send count " + vehicles.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void ReceiveMsg(Object threadContext)
        {
            ZError errors;
            int threadIndex = (int)threadContext;
            Console.WriteLine("thread {0} ZmqFaceSink Start Receive Message !", threadIndex);
            while (receiving)
            {
                using (ZFrame frame = receiverSocketList[threadIndex].ReceiveFrame(ZSocketFlags.DontWait, out errors))
                {
                    if (null != frame)
                    {
                        // Process task
                        string str = System.Text.Encoding.UTF8.GetString(frame.Read());
                        List<string> mFileNameList = JsonConvert.DeserializeObject<List<string>>(str);
                        LogHelper.WriteLog(typeof(ZmqVehicleSink), "recv count " + mFileNameList.Count);
                        List<VehicleObject> mVehObjectList = new List<VehicleObject>();
                        foreach(var fileName in  mFileNameList)
                        {
                             using (var requester = new ZSocket(context, ZSocketType.REQ))
                             {
                                 requester.Connect("tcp://127.0.0.1:50559");
                                 string imagepath = fileName;
                                 if (File.Exists(imagepath))
                                 {
                                     Guid guid = Guid.NewGuid();
                                     DataTypes.VehicleInfo info = new DataTypes.VehicleInfo();
                                     info.Id = "11111";
                                     info.Imagepath = (imagepath);
                                     info.Uuid = guid.ToString().Replace("-", "");
                                     ZMessage zMsg = new ZMessage();
                                     zMsg.Add(new ZFrame(info.ToByteArray()));
                                     requester.Send(zMsg);
                                     using (ZFrame reply = requester.ReceiveFrame())
                                     {

                                         DataTypes.VehicleInfo msg2 = new DataTypes.VehicleInfo();
                                         msg2.MergeFrom(reply.Read());
                                         VehicleRecogResult  vehicleResults=  JsonConvert.DeserializeObject<VehicleRecogResult>(msg2.Jsonresult);
                                         if (null != vehicleResults.Veh)
                                         {
                                             foreach (var item in vehicleResults.Veh)
                                             {
                                                 VehicleObject vehicleObj = new VehicleObject();
                                                 string FileName = fileName;
                                                 vehicleObj.ImagePath = FileName;
                                                 vehicleObj.TaskId = "";
                                                 vehicleObj.CreateTime = new BsonDateTime(DateTime.Now);
                                                 vehicleObj.vehicle = item;
                                                 mVehObjectList.Add(vehicleObj);
                                             }
                                             //Console.WriteLine(" Received:  {0}!", msg2.Jsonresult);
                                         }
                                     }
                                 }
                             }
                            
                        }
                        if (mVehObjectList.Count > 0)
                        {
                            VehicleMongoDAL.GetInstance().AddVehicleObject(mVehObjectList);
                        }
                        Console.WriteLine("thread {0} receive count:{1}", threadIndex,mFileNameList.Count);
                    }
                    else
                    {
                        if (errors == ZError.ETERM)
                            return;	// Interrupted
                        if (errors != ZError.EAGAIN)
                            throw new ZException(errors);
                    }
                }
                Thread.Sleep(10);
            }
            Console.WriteLine("thread {0} ZmqVehicleSink End Receive Message !", threadIndex);
        }

        public void RleaseZmqVehicleSink()
        {
            receiving = false;
        }

        public static string get_uft8(string unicodeString)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(unicodeString);
            var win1252Bytes = Encoding.Convert(
                Encoding.UTF8, Encoding.ASCII, encodedBytes);
            String decodedString = Encoding.ASCII.GetString(win1252Bytes);
            return decodedString;
        }

        public VehicleRecogResult GetVehicleByPic(string imageFileName)
        {
            VehicleRecogResult vehicleResults = new VehicleRecogResult();
            using (var requester = new ZSocket(context, ZSocketType.REQ))
            {
                requester.Connect("tcp://127.0.0.1:50559");
                string imagepath = imageFileName;
                if (File.Exists(imagepath))
                {
                    Guid guid = Guid.NewGuid();
                    DataTypes.VehicleInfo info = new DataTypes.VehicleInfo();
                    info.Id = "11111";
                    info.Imagepath = (imagepath);
                    info.Uuid = guid.ToString().Replace("-", "");
                    ZMessage zMsg = new ZMessage();
                    zMsg.Add(new ZFrame(info.ToByteArray()));
                    requester.Send(zMsg);
                    using (ZFrame reply = requester.ReceiveFrame())
                    {

                        DataTypes.VehicleInfo msg2 = new DataTypes.VehicleInfo();
                        msg2.MergeFrom(reply.Read());
                        vehicleResults = JsonConvert.DeserializeObject<VehicleRecogResult>(msg2.Jsonresult);
                        
                        //Console.WriteLine(" Received:  {0}!", msg2.Jsonresult);
                    }
                }
            }

            return vehicleResults;
        }
    }
}
