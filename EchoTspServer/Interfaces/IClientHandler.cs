namespace EchoServer.Interfaces
{
    public interface IClientHandler
    {
        Task HandleClientAsync(Stream stream, CancellationToken token);
    }
}