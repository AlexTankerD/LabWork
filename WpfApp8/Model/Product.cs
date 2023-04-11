using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
namespace WpfApp8.Model
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public string Description { get; set; }
        public string? QrCode { get; set; }
        public string? ProductImage { get; set; }
        
    }
}
