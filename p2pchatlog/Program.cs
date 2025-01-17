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
    public string connectUsername;
    static void Main(string[] args)
    {
        int port = 64005;
        // server
        Console.WriteLine("What is your username?");
        string username = Console.ReadLine();
        if (username == null)
        {
            return;
        }
        Console.WriteLine("Starting Server...");
        PeerServer server = new PeerServer(port);
        server.Start();
        // client
        Console.WriteLine("Starting Client");

        Console.WriteLine("What is the server's local IPv4 address? (open CMD, type \"ipconfig\" to find out.)");
        PeerClient client = new PeerClient(Console.ReadLine(), port);
        client.StartListening();
        client.SendMessage(username);
        // should read data from client
        //while ((client.messagesReceived == 0))
        //{

        //}
        string message;
        while ((message = Console.ReadLine()) != "exit")
        {
            client.SendMessage(message);
        }
        client.Close();
    }
}

// client program