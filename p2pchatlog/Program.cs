using p2pchatlog;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

// server program
class Program
{
    static void Main(string[] args)
    {
        int port = 64005;
        // server
        Console.WriteLine("Starting Server...");
        PeerServer server = new PeerServer(port);
        server.Start();
        // client
        Console.WriteLine("Starting Client");

        Console.WriteLine("What is the server's local IPv4 address? (open CMD, type \"ipconfig\" to find out.)");
        PeerClient client = new PeerClient(Console.ReadLine(), port);
        client.StartListening();
        string message;
        while ((message = Console.ReadLine()) != "exit")
        {
            client.SendMessage(message);
        }
        client.Close();
    }
}

// client program