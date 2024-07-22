using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using server.Models;


namespace server
{
   

        internal class Server
        {
            TcpListener listener;
            Task acceptClientsTask;
            List<TcpClient> clients;
            private ClientInfo connectedUser;
            public FileXML _file;
            public bool xmlchosen;
            

            //Конструктор сервера, создание списка клиентов, создание потока прослушивания на предмет клиентов
            public Server(string ip, int port, FileXML file, ClientInfo _connectedUser)
            {   
                clients = new List<TcpClient>();
                connectedUser = _connectedUser;
                listener = new TcpListener(IPAddress.Parse(ip), port);

                listener.Start();
                

                acceptClientsTask = new Task(AcceptClients);
                acceptClientsTask.Start();
                _file = file;
                xmlchosen = false;



            }
            
            //Прослушивание на предмет клиентов, добавление клиентов в список,
            //запись ip адреса клиента и его порта, отправка клиенту данных из xml файла
            //создание потока прослушивания сообщения от клиента и его завершение при остановке прослушивания сервера
            private async void AcceptClients()
            {


                try
                {
                    while (true)
                    {
                    
                        var client = listener.AcceptTcpClient();
                        Console.WriteLine("Client is connected");
                        clients.Add(client);
                        IPEndPoint remoteIpEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
                        
                        connectedUser.ClientIp=remoteIpEndPoint.Address.ToString();
                        connectedUser.ClientPort= remoteIpEndPoint.Port.ToString();
                        await SendInfo(client);
                        Task.Run(() => HandleClient(client));
                        

                    }
                }
                catch (Exception e)
                {
                   
                    Console.WriteLine("Server is off");
                }






            }

           //передача данных клиенту
            public async Task BroadCastInfo()
           {

                for (int i = 0; i < clients.Count; i++)
                {
                    await SendInfo(clients[i]);
                }

            }

           //поток прослушивания сообщений от клиента
            private async Task HandleClient(TcpClient client)
            {

                var stream = client.GetStream();



                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {

                    while (true)
                    {
                    //чтение сообщений
                        try
                        {
                            string msg1 = reader.ReadLine();
                            //переотправка сообщения клиенту  
                            if (msg1.Contains("ResendMessage"))
                            {
                                
                                await SendInfo(client);

                            }
                            //удаление клиента из списка и закрытие с ним соединения 
                            if (msg1.Contains("EndOfConnection"))
                            {
                            
                                clients.Remove(client);
                                client.Close();
                            }

                        }
                        catch (ObjectDisposedException e)
                        {
                            //завершение потока при завершении работы клиента
                            Console.WriteLine("End of connection");
                            break;
                        }
                        catch (Exception e)
                        {
                            
                            Console.WriteLine($"{e.Message}");
                        }


                    }
                }





            }





            //отравка данных клиенту если выбран xml файл
            private async Task SendInfo(TcpClient client)
            {
            if (xmlchosen) {
                var stream = client.GetStream();
                using (var writer = new StreamWriter(stream, Encoding.UTF8, 4096, leaveOpen: true))
                {
                    await writer.WriteLineAsync($"1message_split{_file.From}");

                    await writer.WriteLineAsync($"2message_split{_file.Color}");
                    await writer.WriteLineAsync($"3message_split{_file.Text}");
                    await writer.WriteLineAsync($"4message_split{_file.Image}");
                    await writer.WriteLineAsync($"5message_split{DateTime.Now.ToString()}");


                }
            }

                return;
            }
            //завершение работы клиентов
            private void CloseClients()
            {

                for (int i = 0; i < clients.Count; i++)
                {
                    clients[i].Client.Close();
                    clients[i].Close();
                    
                }
                
            }
            //остановка сервера
            public void StopServer()
            {
                
                CloseClients();
                
                
                listener.Server.Close();
                Console.WriteLine($"Server stopped {clients.Count}");



        }
        }
    
}
