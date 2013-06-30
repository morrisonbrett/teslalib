using SimpleMvvmToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeslaLib;
using TeslaManager.Models;

namespace TeslaManager.ViewModels
{
    public class LoginViewModel : ViewModelDetailBase<LoginViewModel, LoginModel>
    {
        public LoginViewModel()
        {
            Model = new LoginModel();
            
            client = new TeslaClient(true);
        }

        #region Properties

        private TeslaClient client;

        #endregion

        #region Events
        
        public event EventHandler<NotificationEventArgs<bool>> DialogResultNotice;

        #endregion

        #region Commands

        private DelegateCommand loginCommand;
        public DelegateCommand LoginCommand
        {
            get { return loginCommand ?? (loginCommand = new DelegateCommand(Login)); }
            private set { loginCommand = value; }
        }

        #endregion

        #region Methods

        public void Login()
        {

            //Task.Factory.StartNew(() => client.LogIn(Model.Email, Model.Password));

            //System.Windows.MessageBox.Show(client.IsLoggedIn.ToString());

            if (client.IsLoggedIn)
            {
                NotifyDialogResultHelper(true);
            }
        }

        #endregion

        #region Helpers

        public void NotifyDialogResultHelper(bool result)
        {
            Notify(DialogResultNotice, new NotificationEventArgs<bool>(null, result));
        }

        #endregion

    }
}