using DataTypes;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace TIEVision.COMMON
{
    class ZmqDetection
    {
        private static ZmqDetection mContext = null;
        private ZContext context = new ZContext();
        private ZSocket socket;
        public static string CenterServer = "";

        public static ZmqDetection GetInstance()
        {
            if (mContext == null)
            {
                mContext = new ZmqDetection();
            }
            return mContext;
        }

        ZmqDetection()
        {
            CenterServer = System.Configuration.ConfigurationManager.AppSettings["CenterServer"].ToString();
            string server_address = "tcp://" + CenterServer + ":7002";
            socket = new ZSocket(context, ZSocketType.REQ);
            socket.Connect(server_address);
        }

        ~ZmqDetection()
        {
           
        }
       
        public TargetFeatureMsg Detection(TargetFeatureMsg msg)
        {
            try
            {
                TargetFeatureMsg msg_reply = new TargetFeatureMsg();
                ZMessage zMsg = new ZMessage();
                zMsg.Add(new ZFrame(msg.ToByteArray()));
                socket.Send(zMsg);

                using (ZFrame reply = socket.ReceiveFrame())
                {
                    //Console.WriteLine(" Received: {0} {1}!", requestText, reply.ReadString());


                    msg_reply.MergeFrom(reply.Read());
                    Console.WriteLine("" + msg_reply.ImageBase64);

                    return msg_reply;
                }
            }catch(Exception ex)
            {
                //LogHelper.WriteLog(typeof(ZmqDetection), ex.Message);
            }
            
            return null;
        }

    }
}
