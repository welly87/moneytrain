using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(ZmqVersion.Current);
            
            using (var context = ZmqContext.Create())
            {
                using (var client = context.CreateSocket(SocketType.REQ))
                {
                    client.Connect("tcp://localhost:5555");
                    //client.Connect("ipc://server.ipc");
                    string request = "Hello";

                    for (int i = 0; i < 100; i++)
                    {
                        Console.WriteLine("Sending request {0} ...", i);
                        client.Send(request, Encoding.UTF8);

                        string reply = client.Receive(Encoding.UTF8);
                        Console.WriteLine("Received reply {0} : {1}", i, reply);
                    }
                }
            }
        }
    }
}
