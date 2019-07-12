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

        public static int General_Id=0;
        static void Main(string[] args)
        {

            //var image = new Bitmap(@"C:\Users\Documents\Desktop\Wpf\media_player-hor.png");
            //ImageConverter imageconverter = new ImageConverter();
            //var imagebytes=((byte[])imageconverter.ConvertTo(image, typeof(byte[])));
            Console.WriteLine("======================SERVER====================");
            #region Client Message
            IPEndPoint endp = new IPEndPoint(IPAddress.Parse("10.1.16.38"), 1031);
            Socket socket;
            socket= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endp);
            socket.Listen(10);
            byte[] buffer = new byte[1024];
            List<CustomSocket> clients = new List<CustomSocket>();
            int counter = 0;
            CustomSocket client = new CustomSocket();
            client.Client = null;
            Task accept = Task.Run(() =>
            {
                while (true)
                {
                    client.Client = socket.Accept();
                    client.Id = counter;
                    ++counter;
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
                                if (item.Id == 0 && General_Id == 0)
                                {
                                item.Client.Send(Encoding.ASCII.GetBytes(message));
                                }
                                else if (item.Id == 1 && General_Id == 1)
                                {
                                    item.Client.Send(Encoding.ASCII.GetBytes(message));
                                }

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
                            var len = Encoding.ASCII.GetString(buffer, 0, length).Length;
                            General_Id = Convert.ToInt32(Encoding.ASCII.GetString(buffer, 0, length)[len - 1]);
                            var message=Encoding.ASCII.GetString(buffer, 0, length);
                            foreach (var item in clients)
                            {
                                if (item.Id == 0 && General_Id == 0)
                                {
                                    item.Client.Send(Encoding.ASCII.GetBytes(message));
                                }
                                else if (item.Id == 1 && General_Id == 1)
                                {
                                    item.Client.Send(Encoding.ASCII.GetBytes(message));
                                }

                                //item.Send(imagebytes);
                            }
                            Console.WriteLine("Server 1 " + message);
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
