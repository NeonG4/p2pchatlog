using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace p2pchatlog
{
    class PeerClient
    {
        private TcpClient client;
        private NetworkStream stream;

        public PeerClient(string serverIp, int serverPort)
        {
            client = new TcpClient(serverIp, serverPort);
            stream = client.GetStream();
        }

        public void StartListening()
        {
            Thread listenThread = new Thread(ListenForMessages);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void ListenForMessages()
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Message Received: " + message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while receiving message: " + ex.Message);
                    break;
                }
            }
        }
        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
            Console.WriteLine("Message sent: " + message);
        }

        public void Close()
        {
            client.Close();
        }
    }
}
