using SimpleMvvmToolkit;
using TeslaManager.Models;

namespace TeslaManager.ViewModels
{
    public class TeslaVehicleViewModel : ViewModelDetailBase<TeslaVehicleViewModel, TeslaVehicleModel>
    {
        public TeslaVehicleViewModel()
        {
            Model = new TeslaVehicleModel();
        }

        public override sealed TeslaVehicleModel Model
        {
            get { return base.Model; }
            set { base.Model = value; }
        }

        #region Properties

        #endregion

        #region Commands

        #endregion

        #region Methods

        #endregion
    }
}