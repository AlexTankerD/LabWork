using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfApp8.Model
{
    public static class ImageToStringConverter
    {
        public static string ConvertImageToString(BitmapImage image)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                PngBitmapEncoder encoder= new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(ms);
                byte[] buffer = ms.ToArray();

                return Convert.ToBase64String(buffer);
            }

            
        }
        public static string ConvertImageToString(Uri uri)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = uri;
                image.EndInit();
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(ms);
                byte[] buffer = ms.ToArray();

                return Convert.ToBase64String(buffer);
            }


        }
    }
}
