using SimpleMvvmToolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private string email;
        [Required]
        [EmailAddress]
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                NotifyPropertyChanged(m => m.Email);
                ValidateProperty(m => m.Email, value);
            }
        }

        private string password;
        [Required]
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                NotifyPropertyChanged(m => m.Password);
                ValidateProperty(m => m.Password, value);
            }
        }
        #endregion
    }
}
