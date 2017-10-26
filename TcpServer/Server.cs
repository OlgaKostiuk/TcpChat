using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpServer
{
    class Server
    {
        private TcpListener server;
        private string address;
        private int port;
        private Dictionary<TcpClient, int> clients;
        private int clientId;

        public Server(string address, int port)
        {
            this.address = address;
            this.port = port;
            server = new TcpListener(IPAddress.Parse(address), port);
            clients = new Dictionary<TcpClient, int>();
        }

        public void Start()
        {
            server.Start();
            Console.WriteLine($"Listening on {address} {port} ...");
            while (true)
            {
                TcpClient newClient = server.AcceptTcpClient();
                clients.Add(newClient, ++clientId);
                new BinaryWriter(newClient.GetStream()).Write("Welcome!");
                Console.WriteLine($"{clients[newClient]} joined");
                ThreadPool.QueueUserWorkItem(DoWork, newClient);
            }
        }

        private void DoWork(object client)
        {
            try
            {
                while (true)
                {
                    string message = new BinaryReader((client as TcpClient).GetStream()).ReadString();
                    Console.WriteLine($"{clients[client as TcpClient]}: {message}");

                    foreach (var item in clients.Keys)
                    {
                        new BinaryWriter(item.GetStream()).Write($"{clients[client as TcpClient]}: {message}");
                    }
                }
            }
            catch
            {
                (client as TcpClient).Close();
                Console.WriteLine($"Client {clients[client as TcpClient]} have disconnected");
                clients.Remove(client as TcpClient);
            }
        }
    }
}
