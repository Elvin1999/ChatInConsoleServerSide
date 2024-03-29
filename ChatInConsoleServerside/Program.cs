﻿using System;
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
        public static int General_Id = 0;
        static void Main(string[] args)
        {
            //var image = new Bitmap(@"C:\Users\Documents\Desktop\Wpf\media_player-hor.png");
            //ImageConverter imageconverter = new ImageConverter();
            //var imagebytes=((byte[])imageconverter.ConvertTo(image, typeof(byte[])));
            Console.WriteLine("======================SERVER====================");
            #region Client Message
            IPEndPoint endp = new IPEndPoint(IPAddress.Parse("192.168.1.103"), 1031);
            Socket socket;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
                    client = new CustomSocket();
                    client.Client = socket.Accept();
                    client.Id = counter++;
                    clients.Add(client);
                    Console.WriteLine("Connected");
                }

            });
            //Task sender = Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        string message = Console.ReadLine();
            //        if (message == "quit")
            //        {
            //            foreach (var item in clients)
            //            {

            //                item.Client.Shutdown(SocketShutdown.Send);
            //            }
            //            break;
            //        }
            //        else
            //        {
            //            Console.WriteLine("Not good zone");
            //            Task ss = Task.Run(() =>
            //            {
            //                foreach (var item in clients)
            //                {
            //                    if (item.Id == General_Id)
            //                    {
            //                        item.Client.Send(Encoding.ASCII.GetBytes(message));
            //                        Console.WriteLine("Server sent " + message);

            //                    }
            //                    else
            //                    {
            //                        item.Client.Send(Encoding.ASCII.GetBytes(message));
            //                        Console.WriteLine("Server sent " + message);

            //                    }
            //                    //item.Send(imagebytes);
            //                }
            //            });
            //        }
            //    }
            //});

            Task receiver = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {

                        foreach (var item in clients)
                        {
                            if (item.Client.Available != 0)
                            {
                                int length = item.Client.Receive(buffer);
                                if (length != 0)
                                {
                                    var len = Encoding.ASCII.GetString(buffer, 0, length).Length;
                                    //var gId = Convert.ToChar(Encoding.ASCII.GetString(buffer, 0, length)[len - 1]).ToString();
                                    //General_Id = Convert.ToInt32(gId);
                                    //Console.WriteLine("Id -  > " + General_Id.ToString());
                                    var message = Encoding.ASCII.GetString(buffer, 0, length);
                                    //if (General_Id == 0)
                                    //{
                                    //    clients[1].Client.Send(Encoding.ASCII.GetBytes(message));
                                    //    Console.WriteLine("Server sent " + message);
                                    //}
                                    //else if (General_Id == 1)
                                    //{
                                    //    clients[0].Client.Send(Encoding.ASCII.GetBytes(message));
                                    //    Console.WriteLine("Server sent " + message);
                                    //}
                                    //new test

                                    var client_index = clients.FindIndex(x => x.Client == item.Client);
                                    if (client_index == 0)
                                    {
                                        clients[1].Client.Send(Encoding.ASCII.GetBytes(message));
                                        Console.WriteLine("Server sent " + message);
                                    }
                                    else if (client_index == 1)
                                    {
                                        clients[0].Client.Send(Encoding.ASCII.GetBytes(message));
                                        Console.WriteLine("Server sent " + message);
                                    }
                                    //break;
                                }
                            }




                        }


                    }
                    catch (Exception)
                    {

                    }

                }
            });
            receiver.Wait();
            //  Task.WaitAll(sender, receiver);
            #endregion
        }

    }
}
