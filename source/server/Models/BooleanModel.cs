using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Models
{
    public class BooleanModel : INotifyPropertyChanged
    {
        private bool portEnability;
        private bool ipEnability;
        private bool disconnectEnability;
        private bool connectEnability;
        private bool openXMLEnability;

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
        public bool OpenXMLEnability
        {
            get
            {
                return openXMLEnability;
            }
            set
            {

                openXMLEnability = value;
                NotifyPropertyChanged("OpenXMLEnability");

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
