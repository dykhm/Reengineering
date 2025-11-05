using System.Security.Cryptography;

namespace EchoServer.Udp
{
    public class UdpMessageBuilder
    {
        private ushort _sequenceNumber = 0;

        public byte[] BuildMessage()
        {
            byte[] samples = new byte[1024];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(samples);
            }

            _sequenceNumber++;
            byte[] header = new byte[] { 0x04, 0x84 };
            byte[] sequence = BitConverter.GetBytes(_sequenceNumber);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(sequence);

            return header.Concat(sequence).Concat(samples).ToArray();
        }

        public ushort GetCurrentSequenceNumber() => _sequenceNumber;
    }
}