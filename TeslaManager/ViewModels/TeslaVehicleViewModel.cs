using SimpleMvvmToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeslaLib.Models;

namespace TeslaManager.ViewModels
{
    public class TeslaVehicleViewModel : ViewModelDetailBase<TeslaVehicleViewModel, TeslaVehicleModel>
    {
        public TeslaVehicleViewModel()
        {
            this.Model = new TeslaVehicleModel();
        }

        #region Properties

        #endregion

        #region Commands

        #endregion

        #region Methods

        #endregion
    }
}