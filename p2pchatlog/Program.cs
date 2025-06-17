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
        string? myIp = null;
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                myIp = ip.ToString();
            }
        }
        if (myIp == null)
        {
            throw new Exception("Null IP");
        }
        string connectUsername;
        int port = 65004;
        // server
        Console.WriteLine("What is your username?");
        string? username = Console.ReadLine();
        if (username == null)
        {
            return;
        }
        PeerServer server = new PeerServer(port);
        server.Start();
        // client

        Console.WriteLine($"What is your friend's local IPv4 address? (Yours is {myIp})");
        PeerClient client = new PeerClient(Console.ReadLine(), port);
        client.StartListening();
        client.SendMessage(username); // sends the username
        Console.Clear();
        while (server.messages.Count == 0) { } // waits for the username to be recieved, probably should use an interupt
        connectUsername = server.messages[0];
        server.user = connectUsername;
        string? message;
        Console.Clear();
        Console.WriteLine($"Talking with {connectUsername}");
        Console.ForegroundColor = ConsoleColor.Blue;
        while ((message = Console.ReadLine()) != "/exit")
        {
            if (message == null)
            {

            }
            else if (message == "/help")
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Type '/help' to get a list of commands");
                Console.WriteLine("Type '/clear' to clear the console");
                Console.WriteLine("Type '/exit' to close the connection");
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if (message == "/clear")
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                Console.WriteLine($"Talking with {connectUsername}");
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else 
            {
                client.SendMessage(message);
            }
        }
        client.Close();
        server.Stop();
    }
}

// client program