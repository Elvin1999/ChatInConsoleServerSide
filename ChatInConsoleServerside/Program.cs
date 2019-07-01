using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatInConsoleServerside
{
    class Program
    {
        static byte[] buffer = new byte[1024];
        static void Main(string[] args)
        {
            Console.WriteLine("======================SERVER====================");
            #region dictionary group by
            //List<string> list = new List<string>() { "Saleh", "ISlam", "Saleh" };
            //Dictionary<string, int> dict = list.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            //foreach(var item in dict)
            //{
            //    Console.WriteLine(item.Key + " " + item.Value);
            //}

            #endregion
            #region Client Message
            IPEndPoint endp = new IPEndPoint(IPAddress.Parse("192.168.1.103"), 1031);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endp);
            socket.Listen(10);
            byte[] buffer = new byte[1024];
            List<Socket> clients = new List<Socket>();
            int counter = 0;
            Socket client = null;
            Task accept = Task.Run(() =>
            {
                while (true)
                {
                    client = socket.Accept();
                    clients.Add(client);
                    ++counter;
                    if (counter == 2)
                    {
                        break;
                    }
                }

            });
            Task sender = Task.Run(() =>
            {
                while (true)
                {
                    string message = Console.ReadLine();
                    if (message == "quit")
                    {
                        foreach (var item in clients)
                        {

                            item.Shutdown(SocketShutdown.Send);
                        }
                        break;
                    }
                    else
                    {
                        Task ss = Task.Run(() =>
                        {
                            foreach (var item in clients)
                            {
                                item.Send(Encoding.ASCII.GetBytes(message));
                            }
                        });
                    }
                    Console.WriteLine("Server 1 " + message);
                }
            });
            Task receiver = Task.Run(() =>
            {
                while (true)
                {

                    Task ss1 = Task.Run(() =>
                    {
                        try
                        {

                            int length = client.Receive(buffer);
                            Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, length));


                        }
                        catch (Exception)
                        {
                        }
                    });


                }

            });
            Task.WaitAll(sender, receiver);
            //Parallel.Invoke(() => Send(client), () => Receive(client));
            #endregion
            #region Server

            //byte[] buffer = new byte[256];
            //TcpListener listener = new TcpListener(IPAddress.Parse("192.168.10.44"), 1031);

            //listener.Start();
            //Socket reciever = listener.AcceptSocket();

            //while (true)
            //{
            //    int uzunluq = reciever.Receive(buffer);
            //    Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, uzunluq));
            //}

            #endregion
            #region MessagingWithClient

            //byte[] buffer = new byte[256];

            //TcpListener listener = new TcpListener(IPAddress.Parse("192.168.10.44"), 1031);

            //listener.Start();
            //Socket socket = listener.AcceptSocket();

            //while (true)
            //{
            //    int uzunluq = socket.Receive(buffer);
            //    Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, uzunluq));
            //    string message = Console.ReadLine();
            //    socket.Send(Encoding.ASCII.GetBytes(message));
            //}


            #endregion
            //StartServer();
            //Thread.Sleep(-1);
        }
        public static void AcceptCallback(IAsyncResult ia)
        {
            Socket socket = (Socket)ia.AsyncState;
            socket = socket.EndAccept(ia);

            Task.Run(() =>
            {
                while (true)
                {
                    string message = Console.ReadLine();
                    if (message == "quit")
                    {
                        socket.Shutdown(SocketShutdown.Send);
                        break;
                    }
                    else
                    {
                        socket.Send(Encoding.ASCII.GetBytes(message));
                    }
                    Console.WriteLine("Server 1 " + message);
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    int length = socket.Receive(buffer);
                    Console.WriteLine("Client  " + Encoding.ASCII.GetString(buffer, 0, length));
                }
            });
        }
        public static void StartServer()
        {
            byte[] buffer = new byte[1024];
            IPEndPoint endp = new IPEndPoint(IPAddress.Parse("192.168.1.103"), 1031);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endp);
            socket.Listen(10);
            socket.BeginAccept(new AsyncCallback(AcceptCallback), socket);
        }
        //==========================================
        public static void Send(Socket client)
        {
            while (true)
            {
                string message = Console.ReadLine();
                client.Send(Encoding.ASCII.GetBytes(message));
                Console.WriteLine("Server 1 " + message);
            }
        }
        //===========================================
        public static void Receive(Socket client)
        {
            while (true)
            {
                byte[] buffer = new byte[256];
                int length = client.Receive(buffer);
                Console.WriteLine("Client 1 " + Encoding.ASCII.GetString(buffer, 0, length));
            }
        }

    }


}
