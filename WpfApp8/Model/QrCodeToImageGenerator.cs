using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using QRCoder;

namespace WpfApp8.Model
{
    public static class QrCodeToImageGenerator
    {
        public static BitmapImage QrCodeGenerator(string Name, string Description, double? Price)
        {
            string text = $"Имя товара: {Name}\nЦена: {Price}\nОписание: {Description}";
            Bitmap qrcode;
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qr = new QRCode(qRCodeData);
            qrcode = qr.GetGraphic(30);

            return Converter(qrcode);
        }
        private static BitmapImage Converter(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((Bitmap)src).Save(ms, ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}
