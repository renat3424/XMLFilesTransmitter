using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace server.Models
{
    [Serializable]
    [XmlRoot("document")]
    public class FileXML: INotifyPropertyChanged
    {
        private string from;
        private string to;
        private string text;
        private string color;
        private string image;

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("version")]
        public string FormatVersion { get; set; }
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
        public string To
        {
            get
            {
                return to;
            }
            set
            {

                to = value;
                NotifyPropertyChanged("To");

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
