using SimpleMvvmToolkit;
using TeslaManager.Models;

namespace TeslaManager.ViewModels
{
    public class TeslaManagerViewModel : ViewModelDetailBase<TeslaManagerViewModel, TeslaManagerModel>
    {
        public TeslaManagerViewModel()
        {
            Model = new TeslaManagerModel();
        }

        public override sealed TeslaManagerModel Model
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