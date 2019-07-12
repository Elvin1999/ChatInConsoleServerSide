using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
namespace ChatInConsoleServerside
{
    class Program
    {
        static byte[] buffer = new byte[1024];
        static void Main(string[] args)
        {
            //var image = new Bitmap(@"C:\Users\Documents\Desktop\Wpf\media_player-hor.png");
            //ImageConverter imageconverter = new ImageConverter();
            //var imagebytes=((byte[])imageconverter.ConvertTo(image, typeof(byte[])));
            Console.WriteLine("======================SERVER====================");
            #region Client Message
            IPEndPoint endp = new IPEndPoint(IPAddress.Parse("10.1.16.38"), 1031);
            Socket socket;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endp);
            socket.Listen(10);
            byte[] buffer = new byte[1024];
            // List<Socket> clients = new List<Socket>();
            List<CustomSocket> clients = new List<CustomSocket>();
            int counter = 0;
            CustomSocket client = new CustomSocket();
            client.Client = null;
            //Socket client = null;
            Task accept = Task.Run(() =>
            {
                while (true)
                {
                    client.Client = socket.Accept();
                    client.Id = -5;                  
                        try
                        {
                            int length = client.Client.Receive(buffer);
                            client.Id = Convert.ToInt32(Encoding.ASCII.GetString(buffer, 0, length));
                            
                            Console.WriteLine("Id : " + client.Id.ToString());
                        }
                        catch (Exception ex)
                        {
                            client.Id = -5;
                        Console.WriteLine(ex.Message);
                        }
                    
                    clients.Add(client);
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

                            item.Client.Shutdown(SocketShutdown.Send);
                        }
                        break;
                    }
                    else
                    {
                        Task ss = Task.Run(() =>
                        {
                            foreach (var item in clients)
                            {
                                item.Client.Send(Encoding.ASCII.GetBytes(message));
                                //item.Send(imagebytes);
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

                            int length = client.Client.Receive(buffer);
                            Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, length));
                        }
                        catch (Exception)
                        {
                        }
                    });
                }

            });
            Task.WaitAll(sender, receiver);
            #endregion
        }
        //public static void AcceptCallback(IAsyncResult ia)
        //{
        //    Socket socket = (Socket)ia.AsyncState;
        //    socket = socket.EndAccept(ia);

        //    Task.Run(() =>
        //    {
        //        while (true)
        //        {
        //            string message = Console.ReadLine();
        //            if (message == "quit")
        //            {
        //                socket.Shutdown(SocketShutdown.Send);
        //                break;
        //            }
        //            else
        //            {
        //                socket.Send(Encoding.ASCII.GetBytes(message));
        //            }
        //            Console.WriteLine("Server 1 " + message);
        //        }
        //    });

        //    Task.Run(() =>
        //    {
        //        while (true)
        //        {
        //            int length = socket.Receive(buffer);
        //            Console.WriteLine("Client  " + Encoding.ASCII.GetString(buffer, 0, length));
        //        }
        //    });
        //}
        //public static void StartServer()
        //{
        //    byte[] buffer = new byte[1024];
        //    IPEndPoint endp = new IPEndPoint(IPAddress.Parse("172.20.28.56"), 1031);
        //    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    socket.Bind(endp);
        //    socket.Listen(10);
        //    socket.BeginAccept(new AsyncCallback(AcceptCallback), socket);
        //}
    }
}
