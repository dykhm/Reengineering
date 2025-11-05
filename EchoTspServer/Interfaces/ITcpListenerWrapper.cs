using System.Net.Sockets;

namespace EchoServer.Interfaces
{
    public interface ITcpListenerWrapper
    {
        void Start();
        void Stop();
        Task<TcpClient> AcceptTcpClientAsync();
    }
}