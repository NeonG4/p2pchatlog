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
        string connectUsername;
        int port = 65004;
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

        Console.WriteLine("What is your friend's local IPv4 address? (Open CMD, type 'ipconfig' and look for IPv4) (you must be on the same network to connect)");
        PeerClient client = new PeerClient(Console.ReadLine(), port);
        client.StartListening();
        client.SendMessage(username); // sends the username
        Console.WriteLine("Establishing connection...");
        // while (client.messagesReceived == 0) { }
        // connectUsername = server.messages[0];
        string message;
        while ((message = Console.ReadLine()) != "/exit")
        {
            if (client.messagesReceived == 1)
            {
                Console.WriteLine("Recieved first message!");
                Console.Clear();
            }
            if (message == null)
            {

            }
            else if (message == "/help")
            {
                Console.WriteLine("Type '/help' to get a list of commands");
                Console.WriteLine("Type '/clear' to clear the console");
                Console.WriteLine("Type '/exit' to close the connection");
            }
            else if (message == "/clear")
            {
                Console.Clear();
            }
            else 
            {
                client.SendMessage(message);
            }
        }
        client.Close();
    }
}

// client program