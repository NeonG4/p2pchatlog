using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace p2pchatlog 
{
    class PeerClient
    {
        private TcpClient client;
        private NetworkStream stream;
        private TcpListener listener;
        private bool isRunning;
        int clientsConnected = 0;

        public PeerClient(string serverIp, int serverPort)
        {
            client = new TcpClient(serverIp, serverPort);
            stream = client.GetStream();

            listener = new TcpListener(IPAddress.Any, serverPort);
            isRunning = false;
        }

        public void StartListening()
        {
            Thread listenThread = new Thread(ListenForMessages);
            listenThread.IsBackground = true;
            listenThread.Start();
            listener.Start();
            isRunning = true;
            Console.WriteLine("Server is listening for incoming connections...");

            Thread listenerThread = new Thread(ListenForConnections);
            listenerThread.Start();
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
        private void ListenForConnections()
        {
            while (isRunning)
            {
                try
                {
                    var client = listener.AcceptTcpClient();
                    Console.WriteLine("Client Connected");
                    clientsConnected++;
                    HandleClient(client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error accepting connection " + ex.Message);
                }
            }
        }
        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    Console.WriteLine(bytesRead);
                    if (bytesRead != 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("Message Received from client: " + message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading from stream " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Closed client connection");
                client.Close();
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
