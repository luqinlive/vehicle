using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace TIEVision.COMMON
{
    public class ZmqTargetResult
    {
        private static ZmqTargetResult mContext = null;
        public static ZContext context = new ZContext();
        private ZSocket socket;

        public static ZmqTargetResult GetInstance()
        {
            if (mContext == null)
            {
                mContext = new ZmqTargetResult();
            }
            return mContext;
        }

        public ZmqTargetResult()
        {
            socket = new ZSocket(context, ZSocketType.PUSH);
            socket.Bind("inproc://target");

        }

        ~ZmqTargetResult()
        {
           
        }

        public void SendTargetResult(string targetResult)
        {
            try 
	        {	        
		        socket.Send(new ZFrame(targetResult));
	        }
	        catch (Exception ex)
	        {
		        Console.WriteLine(ex.Message);
	        }
        }

    }
}
