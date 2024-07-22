using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace client.Models
{
    
    public class FileXML: INotifyPropertyChanged
    {
        private string from;
        private string text;
        private string color;
        private string image;
        private string date;

        public string From
        {
            get
            {
                return from;
            }
            set
            {

                from = value;
                NotifyPropertyChanged("From");

            }
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {

                text = value;
                NotifyPropertyChanged("Text");

            }
        }

        public string Color
        {
            get
            {
                return color;
            }
            set
            {

                color = value;
                NotifyPropertyChanged("Color");

            }
        }

        public string Image
        {
            get
            {
                return image;
            }
            set
            {

                image = value;
                NotifyPropertyChanged("Image");

            }
        }

        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                
                date = value;
                NotifyPropertyChanged(nameof(Date));

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
