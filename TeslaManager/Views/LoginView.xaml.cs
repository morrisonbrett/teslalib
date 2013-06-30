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
using TeslaManager.ViewModels;

namespace TeslaManager.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {

        private LoginViewModel viewModel;
        public LoginView()
        {
            InitializeComponent();

            viewModel = (LoginViewModel)DataContext;

            viewModel.DialogResultNotice += viewModel_DialogResultNotice;
        }

        void viewModel_DialogResultNotice(object sender, SimpleMvvmToolkit.NotificationEventArgs<bool> e)
        {
            DialogResult = e.Data;
        }
    }
}
