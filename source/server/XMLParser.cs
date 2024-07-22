using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace server
{
    //парсинг xml файла
    internal class XMLParser<T>
    {
        public void inFile(T fileObject, string fileName)
        {

            var xmlSerializer = new XmlSerializer(typeof(T));
           
            string path = fileName;
            using (var writer = new StreamWriter(path))
            {
                xmlSerializer.Serialize(writer, fileObject);
            }


        }


        public T FromFile(string fileName)
        {



            var xmlSerializer = new XmlSerializer(typeof(T));
            T member;
            
            using (var reader = new StreamReader(fileName))
            {
                member = (T)xmlSerializer.Deserialize(reader);
            }
            return member;

        }

    }
}
