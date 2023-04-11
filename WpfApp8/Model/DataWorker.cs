using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp8.Model
{
    public static class DataWorker
    {
        public static ObservableCollection<Product> GetProducts()
        {
            using (AppContext db = new AppContext())
            {
                return new ObservableCollection<Product>(db.Products.ToList());
            }
        }
        public static string RemoveProduct(Product product)
        {
            
            using(AppContext db = new AppContext())
            {
                Product productdb = db.Products.Where(x => x.Id == product.Id).FirstOrDefault();
                if(productdb != null)
                {
                    db.Products.Remove(productdb);
                    db.SaveChanges();
                    return "Готово";
                }
                return "Такого работника не существует";
            }
        }
        public static bool AddProduct(Guid id,string name, double? price, string description, string qrcode, string productimage)
        {
            using (AppContext db = new AppContext())
            {
                Product productdb = db.Products.Where(x => x.Name == name && x.Price == price && x.Description == description).FirstOrDefault();
                if (productdb == null)
                {
                    Product newproduct = new Product
                    {
                        Name = name,
                        Price= price,
                        Description = description,
                        QrCode= qrcode,
                        ProductImage = productimage
                    };
                    db.Products.Add(newproduct);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        public static string EditProduct(Product oldproduct, string name, double? price, string description, string qrcode, string productimage)
        {
            using (AppContext db = new AppContext())
            {
                Product productdb = db.Products.Where(x => x.Id == oldproduct.Id).FirstOrDefault();
                if (productdb != null)
                {
                    productdb.Name = name;
                    productdb.Price = price;
                    productdb.Description = description;
                    productdb.QrCode = qrcode;
                    productdb.ProductImage = productimage;
                    db.SaveChanges();
                    return "Готово";
                }
                return "Такого работника не существует";    
            }
        }
    }
}
