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
        Console.WriteLine("Press c!");
        string serverOrClient = Console.ReadLine();
        if (serverOrClient == "c")
        {
            Console.WriteLine("Starting client...");
            Console.WriteLine("What is the server's local IPv4 address? (open CMD, type \"ipconfig\" to find out.)");
            PeerClient client = new PeerClient(Console.ReadLine(), port);
            client.StartListening();

            Console.WriteLine("Enter message to send (or type \"exit\" to quit");
            string message;
            while ((message = Console.ReadLine()) != "exit")
            {
                client.SendMessage(message);
            }
            client.Close();
        }
        else if (serverOrClient == "s")
        {
            Console.WriteLine("Starting server...");
            PeerServer server = new PeerServer(port);
            server.Start();

            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();
            server.Stop();
        }
        else
        {
            Console.WriteLine("Invalid input");
        }
    }
}

// client program