using client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace client
{
    internal class Client
    {

        private FileXML file;
        TcpClient client;
        BooleanModel booleanBehaviour;
        bool isClosed;
        //создание объекта клиент и его подключение к серверу, создание потока чтения сообщений от сервера
        public Client(string ip, int port, FileXML _file, BooleanModel _booleanBehaviour)
        {
            isClosed = false;
            booleanBehaviour = _booleanBehaviour;
            file = _file;
            client = new TcpClient();

            client.Connect(IPAddress.Parse(ip), port);

            Task.Run(ReadMessages);

        }

        //остановка клиента, оправка серверу сообщения о остановке клиентом работы
        public void CloseClient()
        {
            EndOfConnectionMessage();
            client.Close();
            Console.WriteLine("Client closed");

        }

        //запрос на переотправку сервером данных
        public void ResendMessage()
        {
            
            
            var stream = client.GetStream();
            
                using (var writer = new StreamWriter(stream, Encoding.UTF8, 4096, leaveOpen: true))
                {
                    writer.WriteLine("ResendMessage");

                    Console.WriteLine("resend message sent");

                }
            
        
        }

        //отправка сообщения серверу о прекращении работы клиента 
        private void EndOfConnectionMessage()
        {
            var stream = client.GetStream();

            using (var writer = new StreamWriter(stream, Encoding.UTF8, 4096, leaveOpen: true))
            {
                writer.WriteLine("EndOfConnection");

                

            }

        }


        //чтение сообщений от сервера
        private void ReadMessages()
        {
            var stream = client.GetStream();
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {


                while (true)
                {
                    try { 
                    var msg = reader.ReadLine();

                    if (msg != null)
                    {
                        //получение сообщения от сервера и его обработка при помощи регулярного выражения

                        string pattern = @"(\d)message_split(.*)";

                        Match match = Regex.Match(msg, pattern);
                        int num = int.Parse(match.Groups[1].Value);

                        string msg1 = match.Groups[2].Value;
                        //передача объекту данных отправленного xml

                        switch (num)
                        {
                            case 1:
                                file.From = msg1; break;
                            case 2:
                                file.Color = msg1; break;
                            case 3:
                                file.Text = msg1; break;
                            case 4:
                                file.Image = msg1; break;
                            case 5:
                                file.Date = msg1; break;

                        }


                    }

                }
                
                catch (ObjectDisposedException ex) {
                        //при прекращении чтения сервером данных изменение параметров ui элементов и закрытие соединения
                        //завершение работы потока
                        
                        Console.WriteLine("End of connection");
                        break;
                    }
                    catch (IOException ex)
                    {
                        ChangeParametrsAndClose();
                        Console.WriteLine("Server stopped reading");
                        

                    }
                    catch (Exception ex) {

                        Console.WriteLine(ex.ToString());
                       
                            }

                    


                }
                


            }
        }

        //метод для завершения работы клиента и изменения параметров ui
        private void ChangeParametrsAndClose()
        {
            if (!isClosed)
            {
                isClosed = true;
                booleanBehaviour.ConnectEnability = true;
                booleanBehaviour.DisconnectEnability = false;
                booleanBehaviour.ResendEnability = false;
                booleanBehaviour.IpEnability = true;
                booleanBehaviour.PortEnability = true;
                booleanBehaviour.ConnectionStatus = "Not Connected";
                client.Close();

            }
        }
    }
}
