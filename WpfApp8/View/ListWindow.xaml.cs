using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp8.Model;
using WpfApp8.ViewModel;
namespace WpfApp8.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        public static ListBox AllProductsList;
        public ListWindow()
        {
            InitializeComponent();
            BackgroundImage.ImageSource = new BitmapImage(new Uri("Assets/background.jpg",UriKind.Relative));
            DataContext = new ApplicationViewModel();
            AllProductsList = ListAllProducts;
        }

    }
}
