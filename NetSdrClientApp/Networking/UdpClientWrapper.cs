﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetSdrClientApp.Networking;
using System.Diagnostics.CodeAnalysis;

public class UdpClientWrapper : IUdpClient
{
    private readonly IPEndPoint _localEndPoint;
    private CancellationTokenSource? _cts;
    private UdpClient? _udpClient;

    public event EventHandler<byte[]>? MessageReceived;

    [ExcludeFromCodeCoverage]
    public UdpClientWrapper(int port)
    {
        _localEndPoint = new IPEndPoint(IPAddress.Any, port);
    }

    [ExcludeFromCodeCoverage]
    public async Task StartListeningAsync()
    {
        _cts = new CancellationTokenSource();
        Console.WriteLine("Start listening for UDP messages...");

        try
        {
            _udpClient = new UdpClient(_localEndPoint);
            while (!_cts.Token.IsCancellationRequested)
            {
                UdpReceiveResult result = await _udpClient.ReceiveAsync(_cts.Token);
                MessageReceived?.Invoke(this, result.Buffer);

                Console.WriteLine($"Received from {result.RemoteEndPoint}");
            }
        }
        catch (OperationCanceledException ex)
        {
            //empty
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error receiving message: {ex.Message}");
        }
    }

    [ExcludeFromCodeCoverage]
    public void StopListening()
    {
        SafeStop("Stopped listening for UDP messages.");
    }

    [ExcludeFromCodeCoverage]
    public void Exit()
    {
        SafeStop("Stopped listening for UDP messages.");
    }

    [ExcludeFromCodeCoverage]
    private void SafeStop(string message)
    {
        try
        {
            _cts?.Cancel();
            _udpClient?.Close();
            Console.WriteLine(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while stopping: {ex.Message}");
        }
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        var payload = $"{nameof(UdpClientWrapper)}|{_localEndPoint.Address}|{_localEndPoint.Port}";

        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(payload));

        return BitConverter.ToInt32(hash, 0);
    }
}