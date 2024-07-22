using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Models
{
     public class BooleanModel: INotifyPropertyChanged
    {
        private bool portEnability;
        private bool ipEnability;
        private bool resendEnability;
        private bool disconnectEnability;
        private bool connectEnability;
        private string connectionStatus;

        public bool ConnectEnability
        {
            get
            {
                return connectEnability;
            }
            set
            {

                connectEnability = value;
                NotifyPropertyChanged("ConnectEnability");

            }
        }
        public bool DisconnectEnability
        {
            get
            {
                return disconnectEnability;
            }
            set
            {

                disconnectEnability = value;
                NotifyPropertyChanged("DisconnectEnability");

            }
        }
        public bool ResendEnability
        {
            get
            {
                return resendEnability;
            }
            set
            {

                resendEnability = value;
                NotifyPropertyChanged("ResendEnability");

            }
        }

        public bool IpEnability
        {
            get
            {
                return ipEnability;
            }
            set
            {

                ipEnability = value;
                NotifyPropertyChanged("IpEnability");

            }
        }

        public bool PortEnability
        {
            get
            {
                return portEnability;
            }
            set
            {

                portEnability = value;
                NotifyPropertyChanged("PortEnability");

            }
        }

        public string ConnectionStatus
        {
            get
            {
                return connectionStatus;
            }
            set
            {

                connectionStatus = value;
                NotifyPropertyChanged("ConnectionStatus");

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
