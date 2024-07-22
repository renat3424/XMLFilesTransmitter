using client.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Data;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Input;

namespace client.ViewModels
{
    public class ClientViewModel:INotifyPropertyChanged
    {

       
        public FileXML File
        {
            get
            {
                return file;
            }
            set
            {
                
                file = value;
                NotifyPropertyChanged("File");
                
            }
        }
        public BitmapImage BitImage
        {
            get
            {
                return bitImage;
            }
            set
            {

                bitImage = value;
                NotifyPropertyChanged("BitImage");

            }
        }
        public string Ip { get {
                return ip;
            } set { 
                
                ip = value;
                NotifyPropertyChanged("Ip");

            } }
        public string Port {
            get
            {
                return port;
            }
            set
            {

                port = value;
                NotifyPropertyChanged("Port");

            }
        }
        public BooleanModel BooleanBehaviour
        {
            get
            {
                return booleanBehaviour;
            }
            set
            {

                booleanBehaviour = value;
                NotifyPropertyChanged("BooleanBehaviour");

            }
        }



        private RelayCommand connectToServer;
        private Client client;
        private string ip;
        private string port;
    
        private FileXML file;
        private BitmapImage bitImage;
        BitmapClass instruments;
        private RelayCommand resendMessage;
        private RelayCommand disconnectCommand;
        private BooleanModel booleanBehaviour;




        public RelayCommand ConnectToServer
        {


            get
            {

                return connectToServer ?? new RelayCommand(obj =>
            {
                ConnectToServerMethod();

            });

            }
        }
        public RelayCommand ResendMessage
        {


            get
            {

                return resendMessage ?? new RelayCommand(obj =>
                {
                    ResendMessageMethod();

                });

            }
        }
        public RelayCommand DisconnectCommand
        {


            get
            {

                return disconnectCommand ?? new RelayCommand(obj =>
                {
                    DisconnectCommandMethod();

                });

            }
        }





        
        public ClientViewModel()
        {
            BooleanBehaviour = new BooleanModel()
            {
                ConnectEnability = true,
                DisconnectEnability = false,
                ResendEnability = false,
                IpEnability = true,
                PortEnability = true,
                ConnectionStatus = "Not connected"
            };
            
            File = new FileXML();
            File.PropertyChanged += File_PropertyChanged;
            instruments=new BitmapClass();
        }





        //при изменении аттрибута модели объекта xml файла, который содержит в себе закодированную строку изображения
        //происходит его раскодировка и передача данных объекту bitmap source для привязки к xaml 
        private void File_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(File.Image))
            {
                try
                {
                    Bitmap b = instruments.Base64StringToImage(File.Image);
                    BitImage = instruments.BitmapToImageSource(b);
                    if (BitImage != null && !BitImage.IsFrozen)
                    {
                        
                        BitImage.Freeze(); 
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        //подсоедиение к серверу и обработка возможных ошибок, ограничение элементов ui
        private void ConnectToServerMethod()
        {
            


            try {

                
                
                client = new Client(ip, int.Parse(port), File, BooleanBehaviour);
                BooleanBehaviour.ConnectEnability = false;
                BooleanBehaviour.DisconnectEnability = true;
                BooleanBehaviour.ResendEnability = true;
                BooleanBehaviour.IpEnability = false;
                BooleanBehaviour.PortEnability = false;
                BooleanBehaviour.ConnectionStatus = "Connected";


            }
            catch (SocketException ex)
            {
                MessageBox.Show($"SocketException: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        //запрос на переотправку сообщения
        private void ResendMessageMethod()
        {
            client.ResendMessage();
        }
        //отключение клиента
        private void DisconnectCommandMethod()
        {
            client.CloseClient();
            BooleanBehaviour.ConnectEnability = true;
            BooleanBehaviour.DisconnectEnability = false;
            BooleanBehaviour.ResendEnability = false;
            BooleanBehaviour.IpEnability = true;
            BooleanBehaviour.PortEnability = true;
            BooleanBehaviour.ConnectionStatus = "Not Connected";
        }
        //остановка работы клиента и свзяанных с ним потоков
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (BooleanBehaviour.DisconnectEnability)
            {
                client.CloseClient();

            }
        }

        
        
        
        
        
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }
}
