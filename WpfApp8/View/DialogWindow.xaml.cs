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

namespace WpfApp8.View
{
    /// <summary>
    /// Логика взаимодействия для MessageView.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public static bool btnresult;
        public DialogWindow(string message)
        {
            InitializeComponent();

            MessageText.Text = message;
        }

        private void yesbtn_Click(object sender, RoutedEventArgs e)
        {
            btnresult= true;
            this.Close();
        }

        private void nobtn_Click(object sender, RoutedEventArgs e)
        {
            btnresult= false;
            this.Close();
        }
    }
}
