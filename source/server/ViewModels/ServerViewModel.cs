using Microsoft.Win32;
using server.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace server.ViewModels
{
    public class ServerViewModel : INotifyPropertyChanged
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
        public string Ip
        {
            get
            {
                return ip;
            }
            set
            {

                ip = value;
                NotifyPropertyChanged("Ip");

            }
        }
        public string Port
        {
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
        public string DataFile
        {
            get
            {
                return dataFile;
            }
            set
            {

                 dataFile=value;
                NotifyPropertyChanged("DataFile");

            }
        }
        public ClientInfo ConnectedUser
        {
            get
            {
                return connectedUser;
            }
            set
            {

                connectedUser = value;
                NotifyPropertyChanged("ConnectedUser");

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
        
        
        
        
        
        
        
        
        private string ip;
        private string port;
        private FileXML file;
        private BitmapImage bitImage;
        private RelayCommand connectToServer;
        private AsyncRelayCommand getXML;
        private BitmapClass instruments;
        private Server server;
        private string dataFile;
        private ClientInfo connectedUser;
        private BooleanModel booleanBehaviour;
        private RelayCommand disconnectFromServer;




        public RelayCommand Connect
        {


            get
            {

                return connectToServer ?? new RelayCommand(obj =>
                {
                    ConnectMethod();

                });

            }
        }
        public AsyncRelayCommand GetXML
        {
            get
            {

                return getXML ?? new AsyncRelayCommand(async obj =>
                {
                    await GetXMLMethod();

                });

            }


        }
        public RelayCommand Disconnect
        {


            get
            {

                return disconnectFromServer ?? new RelayCommand(obj =>
                {
                    DisconnectMethod();

                });

            }
        }




        //создание ViewModel для управления View
        public ServerViewModel()
        {
            //определение объекта boolean behaviour для ограничения элементов ui
            BooleanBehaviour = new BooleanModel()
            {
                ConnectEnability = true,
                DisconnectEnability = false,
                OpenXMLEnability = false,
                IpEnability = true,
                PortEnability = true

            };
            file = new FileXML();
            instruments = new BitmapClass();
        }




        //Метод для завершения работы сервера
        private void DisconnectMethod()
        {   
            server.StopServer();
            BooleanBehaviour.ConnectEnability = true;
            BooleanBehaviour.DisconnectEnability = false;
            BooleanBehaviour.OpenXMLEnability = false;
            BooleanBehaviour.IpEnability = true;
            BooleanBehaviour.PortEnability = true;
        }

        //Метод для окрытия и обработки файла xml и отправки его данных подключенным клиентам
        private async Task GetXMLMethod()
        {
            
            try
            {
                OpenFileDialog sfd = new OpenFileDialog();
                sfd.Filter = "XML|*.xml";
                bool? result = sfd.ShowDialog();
                if (result == true)
                {
                    string file = sfd.FileName;
                    
                    DataFile = file;

                    XMLParser<FileXML> parser = new XMLParser<FileXML>();
                    file = file.Replace('/', '\\');
                    Console.WriteLine(file);
                    File =parser.FromFile(file);
                    Bitmap b = instruments.Base64StringToImage(File.Image);
                    BitImage = instruments.BitmapToImageSource(b);
                    server._file = File;
                    server.xmlchosen = true;
                    await server.BroadCastInfo();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }


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


        //создание объекта сервер, передача ему необходимых данных 
        private void ConnectMethod()
        {
            ConnectedUser=new ClientInfo();
            try
            {

                Console.WriteLine($"port: {port}, ip: {ip}");
                server = new Server(ip, int.Parse(port), File, ConnectedUser);
                BooleanBehaviour.ConnectEnability = false;
                BooleanBehaviour.DisconnectEnability = true;
                BooleanBehaviour.OpenXMLEnability = true;
                BooleanBehaviour.IpEnability = false;
                BooleanBehaviour.PortEnability = false;


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
