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

namespace TeslaManager.Views
{
    /// <summary>
    /// Interaction logic for TeslaManagerVIew.xaml
    /// </summary>
    public partial class TeslaManagerView : Window
    {
        public TeslaManagerView()
        {
            InitializeComponent();

            LoginView view = new LoginView();
            view.Show();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
