using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace p2pchatlog
{
    class PeerServer
    {
        public List<string> messages = new List<string>();
        private TcpListener listener;
        private bool isRunning;
        public string user = string.Empty;
        int clientsConnected = 0;

        public PeerServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            isRunning = false;
        }

        public void Start()
        {
            listener.Start();
            isRunning = true;
            Console.WriteLine("Server is listening for incoming connections...");

            Thread listenerThread = new Thread(ListenForConnections);
            listenerThread.Start();
        }

        private void ListenForConnections()
        {
            while (isRunning)
            {
                try
                {
                    var client = listener.AcceptTcpClient();
                    Console.WriteLine("Client Connected");
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
                    if (bytesRead != 0) // if a message was read
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        messages.Add(message);
                        Console.WriteLine($"{user}: {message}");
                        Console.WriteLine("Server");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading from stream\n" + ex.Message);
            }
            finally
            {
                Console.WriteLine("Closed client connection");
                client.Close();
            }
        }
        public void Stop()
        {
            isRunning = false;
            listener.Stop();
        }
    }
}
