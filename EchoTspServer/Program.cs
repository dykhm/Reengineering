using EchoServer.Handlers;
using EchoServer.Infrastructure;
using EchoServer.Udp;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace EchoServer
{
    public class Program
    {
        [ExcludeFromCodeCoverage]
        public static void Main(string[] args)
        {
            MainInternal(args, Console.In).GetAwaiter().GetResult();
        }

        public static async Task MainInternal(string[] args, TextReader input)
        {
            input ??= Console.In;

            var listener = new TcpListenerWrapper(IPAddress.Any, 5000);
            var clientHandler = new EchoClientHandler();
            var server = new Server.EchoServer(listener, clientHandler);

            _ = Task.Run(() => server.StartAsync());

            string host = "127.0.0.1";
            int port = 60000;
            int intervalMilliseconds = 5000;

            using var sender = new UdpTimedSender(host, port);
            sender.StartSending(intervalMilliseconds);

            await WaitForQuitKey(input);

            sender.StopSending();
            server.Stop();
            Console.WriteLine("Sender stopped.");
        }

        public static async Task WaitForQuitKey(TextReader input)
        {
            while (true)
            {
                var line = await input.ReadLineAsync();
                if (line?.ToUpper() == "Q") break;
                await Task.Delay(50);
            }
        }
    }
}