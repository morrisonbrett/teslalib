using SimpleMvvmToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeslaManager.Models;

namespace TeslaManager.ViewModels
{
    public class TeslaManagerViewModel : ViewModelDetailBase<TeslaManagerViewModel, TeslaManagerModel>
    {
        public TeslaManagerViewModel()
        {
            this.Model = new TeslaManagerModel();
        }

        #region Properties

        #endregion

        #region Commands

        #endregion

        #region Methods

        #endregion
    }
}