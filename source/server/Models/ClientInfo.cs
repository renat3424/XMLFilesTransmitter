using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Models
{
    public class ClientInfo : INotifyPropertyChanged
    {

        public string ClientIp
        {
            get
            {
                return clientip;
            }
            set
            {

                clientip = value;
                NotifyPropertyChanged("ClientIp");

            }
        }
        public string ClientPort
        {
            get
            {
                return clientport;
            }
            set
            {
                
                clientport = value;
                NotifyPropertyChanged("ClientPort");

            }
        }



        private string clientip;
        private string clientport;





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
