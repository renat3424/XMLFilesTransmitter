using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace server
{
    internal class BitmapClass
    {
        //класс иструментов для работы с bitmap изображениями
        //кодирование изображения в строку
        public Bitmap Base64StringToImage(string base64)
        {
            byte[] arr = Convert.FromBase64String(base64);
            using (MemoryStream ms = new MemoryStream(arr))
            {
                Bitmap bitmap = new Bitmap(ms);

                return bitmap;
            }
        }

        //перевод bitmap изображения в bitmap source для показа изображения
        public BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
