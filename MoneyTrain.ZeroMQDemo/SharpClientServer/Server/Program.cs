using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = ZmqContext.Create())
            {
                using (var server = context.CreateSocket(SocketType.REP))
                {
                    server.Bind("tcp://*:5555");
                    //server.Bind("ipc://server.ipc");

                    while (true)
                    {
                        string msg = server.Receive(Encoding.UTF8);

                        Console.WriteLine("Received request : {0}", msg);

                        Thread.Sleep(1000);

                        server.Send("World", Encoding.UTF8);
                    }
                }
            }
        }
    }
}
