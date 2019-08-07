using DataTypes;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TIEVison.COMMON;
using ZeroMQ;

namespace TIEVision.COMMON
{
    class ZmqComparison
    {

        public delegate void ShowOBJEventHandler(TargetFeatureMsg targetFeatureMsg );
        public static event ShowOBJEventHandler ShowOBJInfo;

        private static ZmqComparison mContext = null;
        private ZContext context = new ZContext();
        private ZSocket socket;

        private ZSocket receiver;
        private Thread thReceiverMsg = null;

        public static string CenterServer = "";
        private bool receiving = true;

        public static ZmqComparison GetInstance()
        {
            if (mContext == null)
            {
                mContext = new ZmqComparison();
            }
            return mContext;
        }

        ZmqComparison()
        {
            CenterServer = System.Configuration.ConfigurationManager.AppSettings["CenterServer"].ToString();
            string server_address = "tcp://" + CenterServer + ":7001";
            socket = new ZSocket(context, ZSocketType.REQ);
            socket.Connect(server_address);

            receiver = new ZSocket(context, ZSocketType.PULL);
            receiver.Bind("tcp://*:7003");

            if (thReceiverMsg != null)
            {
                thReceiverMsg.Abort();
            }
            thReceiverMsg = new Thread(new ThreadStart(ReceiveMsg));
            thReceiverMsg.Start();
        }

        ~ZmqComparison()
        {

            if (thReceiverMsg != null)
            {
                thReceiverMsg.Abort();
            }
        }

        private void ReceiveMsg()
        {
            ZError errors;
            Console.WriteLine("ZmqComparison Thread Start Receive Message!");
            while (receiving)
            {
                using (ZFrame frame = receiver.ReceiveFrame(ZSocketFlags.DontWait, out errors))
                {
                    if (null != frame)
                    {
                        // Process task
                        TargetFeatureMsg msg_receive = new TargetFeatureMsg();
                        msg_receive.MergeFrom(frame.Read());
                        //Console.WriteLine("" + msg_receive.Processed);
                        if (ShowOBJInfo != null)
                        {
                            try
                            {
                                ShowOBJInfo(msg_receive);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
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
            Console.WriteLine("ZmqComparison Thread End Receive Message!");
        }

        public void RleaseZmqComparison()
        {
            receiving = false;
            //if (thReceiverMsg != null)
            //{
            //    thReceiverMsg.Abort();
            //}
        }

        public TargetFeatureMsg Comparison(TargetFeatureMsg msg)
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
                if (ShowOBJInfo != null)
                {
                    try
                    {
                        ShowOBJInfo(msg_reply);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return msg_reply;
            }
            return null;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    LogHelper.WriteLog(typeof(ZmqComparison), ip.ToString());
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
