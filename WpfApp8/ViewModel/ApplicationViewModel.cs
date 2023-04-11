using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using WpfApp8.Model;
using WpfApp8.View;
using QRCoder;
namespace WpfApp8.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public double? Price { get; set; }
        public string Description { get; set; }
        public string? QrCode { get; set; }
        public string? ProductImage { get; set; }

        #region Все продукты
        private ObservableCollection<Product> _allproducts = DataWorker.GetProducts();
        public ObservableCollection<Product> AllProducts
        {
            get { return _allproducts; }
            set
            {
                _allproducts = value;
                OnPropertyChanged("AllProducts");
            }
        }

        public static ObservableCollection<Product> ProductCart = new ObservableCollection<Product>();
        public Product SelectedCartProduct { get; set; }
        private double _allprice {get; set;}
        public double AllPrice { get { return _allprice; }
            set
            {
                _allprice = value;
                OnPropertyChanged("AllPrice");
            }
        }
        #endregion

        #region Выбранный продукт
        public static Product SelectedProduct { get; set;}

        public static string SelectedName { get; set; }
        public static string SelectedDescription { get; set; }
        public static double? SelectedPrice { get; set; }
        #endregion

        private RelayCommand _addnewproductcommand;
        public RelayCommand AddNewProductCommand
        {
            get
            {
                return _addnewproductcommand ?? new RelayCommand(obj =>
                {
                    Window wnd = obj as Window;
                    bool resultstr;
                    if (!Regexp.IsProductNameValid(Name) || !Regexp.IsPriceValid(Price) || !Regexp.IsDescriptionValid(Description))
                    {
                        OpenMessageWindow("Неправильно введенные данные");
                    }
                    else
                    {
                        resultstr = DataWorker.AddProduct(new Guid(),Name, Price, Description, QrCode, ProductImage);
                        if(resultstr == true) 
                        {
                            UpdateListWindow();
                            OpenMessageWindow("Готово");
                            SetNullValueProperties();
                            wnd.Close();
                        }
                        else
                        {
                            OpenMessageWindow("Такой продукт уже существует");
                        }
                        
                    }
                }
                );
            }
        }
        private RelayCommand _editproductcommand;
        public RelayCommand EditProductCommand
        {
            get
            {
                return _editproductcommand ?? new RelayCommand(obj =>
                {
                    Window wnd = obj as Window;
                    string resultstr;
                    if (!Regexp.IsProductNameValid(Name) || !Regexp.IsPriceValid(Price) || !Regexp.IsDescriptionValid(Description)) 
                    {
                        OpenMessageWindow("Неправильно введенные данные");
                       
                    }
                    else
                    {
                        resultstr = DataWorker.EditProduct(SelectedProduct, Name, Price, Description, QrCode, ProductImage);
                        UpdateListWindow();
                        OpenMessageWindow("Готово");
                        SetNullValueProperties();
                        SetNullSelectedItemValueProperties();
                        wnd.Close();
                    }
                }
                );
            }
        }

        private RelayCommand _deleteproductcommand;

        public RelayCommand DeleteProductCommand
        {
            get
            {
                return _deleteproductcommand ?? new RelayCommand(obj =>
                {
                    string resultstr;
                    if(SelectedProduct == null)
                    {
                        OpenMessageWindow("Продукт не выбран");
                    }
                    else
                    {
                        resultstr = DataWorker.RemoveProduct(SelectedProduct);
                        UpdateListWindow();
                        OpenMessageWindow("Готово");
                        SetNullValueProperties();
                    }
                });
            }
        }

        private RelayCommand _deleteproductfromcartcommand;

        public RelayCommand DeleteProductFromCartCommand
        {
            get
            {
                return _deleteproductfromcartcommand ?? new RelayCommand(obj =>
                {
                    
                    if (SelectedCartProduct == null)
                    {
                        OpenMessageWindow("Продукт не выбран");
                    }
                    else
                    {
                        AllPrice -= Convert.ToDouble(SelectedCartProduct.Price);
                        DeleteProductFromCart();
                        UpdateOrderWindow();
                        OpenMessageWindow("Готово");
                    }
                });
            }
        }

        private void DeleteProductFromCart()
        {
            ProductCart.Remove(SelectedCartProduct);
        }

        private RelayCommand _addproductincartcommand;

        public RelayCommand AddProductInCartCommand
        {
            get
            {
                return _addproductincartcommand ?? new RelayCommand(obj =>
                {
                    bool resultstr;
                    if (SelectedProduct == null)
                    {
                        OpenMessageWindow("Продукт не выбран");
                    }
                    else
                    {
                        resultstr = AddProductInCart();
                        AllPrice += Convert.ToDouble(SelectedProduct.Price);
                        OpenMessageWindow("Продукт добавлен в корзину");
                    }
                });
            }
        }

        private bool AddProductInCart()
        {
            ProductCart.Add(SelectedProduct);
            return true;
        }

        private RelayCommand _confirmordercommnad;
        public RelayCommand ConfirmOrderCommand
        {
            get
            {
                return _confirmordercommnad ?? new RelayCommand(obj =>
                {
                    Window wnd = obj as Window;
                    if (ProductCart.Count == 0)
                    {
                        OpenMessageWindow("Заказ не может быть оформлен, т.к. отсутствуют продукты в корзине");
                        return;
                    }
                    OpenDialogtWindow($"Сумма заказа: {AllPrice}\nПодтвердить заказ?");
                    if (DialogWindow.btnresult == true)
                    {
                        OpenMessageWindow("Заказ успешно оформлен");
                        ProductCart.Clear();
                        AllPrice = 0;
                        wnd.Close();
                    }
                });
            }
        }

        private void SetNullValueProperties()
        {
            Name = null;
            Description = null;
            Price = null;
            QrCode= null;
            ProductImage= null;
        }

        #region Команды добавления изображения
        private RelayCommand _addimagecommand;

        public RelayCommand AddImageCommand
        {
            get
            {
                return _addimagecommand ?? new RelayCommand(obj =>
                {
                    Window window = obj as Window;
                    AddImage(window);
                    if (string.IsNullOrEmpty(ProductImage))
                        OpenMessageWindow("Неправильный путь к изображению");

                }
                );

            }
        }
        #endregion

        #region Метод добавления изображения в БД и привязки к окну
        private void AddImage(Window wnd)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Png files (*.png)|*.png| Jpg files (*.jpg)|*.jpg";
            openFileDialog.ShowDialog();
            if (string.IsNullOrEmpty(openFileDialog.FileName))
                return;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(openFileDialog.FileName);
            image.EndInit();
            SetImageInWindow(wnd, image.UriSource);
            ProductImage = ImageToStringConverter.ConvertImageToString(image);
        }
        public void SetImageInWindow(Window wnd, Uri uri)
        {
            Image image = wnd.FindName("ProductImage") as Image;
            image.Source = new BitmapImage(uri);

        }
        #endregion

        #region Команды добавления Qr кода

        private RelayCommand _addqrcodecommand;

        public RelayCommand AddQrCodeCommand
        {
            get
            {
                return _addqrcodecommand ?? new RelayCommand(obj =>
                {
                    Window window = obj as Window;
                    AddQrCode(window);
                }
                );

            }
        }
        #endregion

        #region Методы добавления Qr кода в БД и привязки к окну
        private void AddQrCode(Window wnd)
        {
             
            BitmapImage qrcodeimage = QrCodeToImageGenerator.QrCodeGenerator(Name, Description, Price);
            
            SetImageInWindow(wnd, qrcodeimage);
            QrCode = ImageToStringConverter.ConvertImageToString(qrcodeimage);
        }

        public void SetImageInWindow(Window wnd,BitmapImage qrcode)
        {
            Image image = wnd.FindName("QrCodeImage") as Image;
            image.Source = qrcode;
            
        }
        #endregion

        #region Команды открытия окон
        private RelayCommand _openaddProductwindowcommand;
        public RelayCommand OpenAddProductWindowCommand
        {
            get
            {
                return _openaddProductwindowcommand ?? new RelayCommand(obj =>
                    { OpenAddProductWindow(); });
            }
        }

        private RelayCommand _openorderwindowcommand;
        public RelayCommand OpenOrderWindowCommand
        {
            get
            {
                return _openorderwindowcommand ?? new RelayCommand(obj =>
                { OpenOrderWindow(); });
            }
        }

        private RelayCommand _openeditproductwindowcommand;
        public RelayCommand OpenEditProductWindowCommand
        {
            get
            {
                return _openeditproductwindowcommand ?? new RelayCommand(obj =>
                {
                    if (SelectedProduct == null)
                    {
                        OpenMessageWindow("Продукт не выбран");
                    }
                    else
                    {
                        SetSelectedItemValues();
                        OpenEditProductWindow();
                    }
                });
            }
        }

        public void SetSelectedItemValues()
        {
            SelectedName = SelectedProduct.Name;
            SelectedDescription = SelectedProduct.Description;
            SelectedPrice = SelectedProduct.Price;
        }
        public void SetNullSelectedItemValueProperties()
        {
            SelectedName = null;
            SelectedPrice = null;
            SelectedDescription = null;
        }

        #endregion

        #region Методы открытия окон и обновление основного
        private void OpenAddProductWindow()
        {
            AddProductWindow addProductWindow = new AddProductWindow();
            SetCenterPositionAndOwner(addProductWindow);
            
        }
        private void OpenDialogtWindow(string message)
        {
            DialogWindow dialogWindow = new DialogWindow(message);
            SetCenterPositionAndOwner(dialogWindow);

        }
        private void OpenOrderWindow()
        {
            OrderWindow orderWindow = new OrderWindow();
            orderWindow.DataContext= this;
            OrderWindow.ListProductCart.ItemsSource = ProductCart;
            SetCenterPositionAndOwner(orderWindow);

        }
        private void OpenEditProductWindow()
        {
            EditProductWindow editProductWindow = new EditProductWindow();
            SetCenterPositionAndOwner(editProductWindow);

        }
        private void OpenMessageWindow(string message)
        {
            MessageWindow messagewindow = new MessageWindow(message);
            SetCenterPositionAndOwner(messagewindow);
        }
        private void SetCenterPositionAndOwner(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
        private void UpdateListWindow()
        {
            AllProducts = DataWorker.GetProducts();
            ListWindow.AllProductsList.ItemsSource = null;
            ListWindow.AllProductsList.Items.Clear();
            ListWindow.AllProductsList.ItemsSource = AllProducts;

        }
        private void UpdateOrderWindow()
        {
            OrderWindow.ListProductCart.ItemsSource = null;
            OrderWindow.ListProductCart.Items.Clear();
            OrderWindow.ListProductCart.ItemsSource = ProductCart;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
