using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp8.ViewModel;

namespace WpfApp8.View
{
    /// <summary>   
    /// Логика взаимодействия для AddNewProduct.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
            BackgroundImage.ImageSource = new BitmapImage(new Uri("Assets/background2.jpg", UriKind.Relative));
            DataContext = new ApplicationViewModel();
        }
    }
}
