﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace p2pchatlog
{
    class PeerClient
    {
        public int messagesReceived;
        private TcpClient client;
        private NetworkStream stream;
        public List<string> messages = new List<string>();
        public string user = string.Empty;
        bool isRunning = true;
        Thread listenThread;

        public PeerClient(string serverIp, int serverPort)
        {
            client = new TcpClient(serverIp, serverPort);
            stream = client.GetStream();
        }

        public void StartListening()
        {
            listenThread = new Thread(ListenForMessages);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void ListenForMessages()
        {
            byte[] buffer = new byte[1024];
            while (isRunning)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine(user + ": " + message);
                    messages.Add(message);
                    messagesReceived++;
                }
                catch (Exception ex)
                {
                    if (!isRunning) { break; }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error while receiving message: " + ex.Message);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    return;
                }
            }
        }
        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
            // Console.WriteLine(message);
        }

        public void Close()
        {
            isRunning = false;
            client.Close();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You are free to close this window");
        }
    }
}
