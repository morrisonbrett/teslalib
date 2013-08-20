using System.Threading.Tasks;
using SimpleMvvmToolkit;
using System;
using TeslaLib;
using TeslaManager.Models;

namespace TeslaManager.ViewModels
{
    public class LoginViewModel : ViewModelDetailBase<LoginViewModel, LoginModel>
    {
        public LoginViewModel()
        {
            Model = new LoginModel();
            
            _client = new TeslaClient();
        }

        public override sealed LoginModel Model
        {
            get { return base.Model; }
            set { base.Model = value; }
        }

        #region Properties

        private readonly TeslaClient _client;

        #endregion

        #region Events
        
        public event EventHandler<NotificationEventArgs<bool>> DialogResultNotice;

        #endregion

        #region Commands

        private DelegateCommand _loginCommand;
        public DelegateCommand LoginCommand
        {
            get { return _loginCommand ?? (_loginCommand = new DelegateCommand(Login)); }
            private set { _loginCommand = value; }
        }

        #endregion

        #region Methods

        public void Login()
        {
            Task.Factory.StartNew(() => _client.LogIn(Model.Email, Model.Password));

            System.Windows.MessageBox.Show(_client.IsLoggedIn.ToString());

            if (_client.IsLoggedIn)
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