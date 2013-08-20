using SimpleMvvmToolkit;
using System.ComponentModel.DataAnnotations;

namespace TeslaManager.Models
{
    public class LoginModel : ModelBase<LoginModel>
    {
        #region Constructors

        public LoginModel()
        {
            Email = "";
            Password = "";
        }

        #endregion

        #region Properties

        private string _email;
        [Required]
        [EmailAddress]
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                NotifyPropertyChanged(m => m.Email);
                ValidateProperty(m => m.Email, value);
            }
        }

        private string _password;
        [Required]
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyPropertyChanged(m => m.Password);
                ValidateProperty(m => m.Password, value);
            }
        }
        #endregion
    }
}
