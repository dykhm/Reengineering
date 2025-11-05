namespace EchoServer.Interfaces
{
    public interface IUdpSender : IDisposable
    {
        void StartSending(int intervalMilliseconds);
        void StopSending();
    }
}