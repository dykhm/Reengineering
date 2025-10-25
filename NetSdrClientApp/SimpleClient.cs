using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EchoServer;

namespace NetSdrClientApp
{
    public class SimpleClient
    {
        public async Task RunServer()
        {
            EchoServer.EchoServer server = new EchoServer.EchoServer(5000);

            _ = Task.Run(() => server.StartAsync());

            Console.WriteLine("Server is running. Press any key to stop...");
            Console.ReadKey();

            server.Stop();
        }

    }
}
