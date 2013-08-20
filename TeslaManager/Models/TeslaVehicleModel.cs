using SimpleMvvmToolkit;
using TeslaLib.Models;

namespace TeslaManager.Models
{
    public class TeslaVehicleModel : ModelBase<TeslaVehicleModel>
    {
        public TeslaVehicleModel()
        {
            IsFrunkOpen = false;
            IsTrunkOpen = false;
            IsLeftFrontDoorOpen = false;
            IsLeftRearDoorOpen = false;
            IsRightFrontDoorOpen = false;
            IsRightRearDoorOpen = false;
        }

        public TeslaVehicleModel(VehicleStateStatus state)
        {
            IsFrunkOpen = state.IsFrontTrunkOpen;
            IsTrunkOpen = state.IsRearTrunkOpen;
            IsLeftFrontDoorOpen = state.IsDriverFrontDoorOpen;
            IsLeftRearDoorOpen = state.IsDriverRearDoorOpen;
            IsRightFrontDoorOpen = state.IsPassengerFrontDoorOpen;
            IsRightRearDoorOpen = state.IsPassengerRearDoorOpen;
        }

        private bool _isFrunkOpen;
        public bool IsFrunkOpen
        {
            get { return _isFrunkOpen; }
            set
            {
                _isFrunkOpen = value;
                NotifyPropertyChanged(m => m.IsFrunkOpen);
            }
        }

        private bool _isTrunkOpen;
        public bool IsTrunkOpen
        {
            get { return _isTrunkOpen; }
            set
            {
                _isTrunkOpen = value;
                NotifyPropertyChanged(m => m.IsTrunkOpen);
            }
        }

        private bool _isLeftFrontDoorOpen;
        public bool IsLeftFrontDoorOpen
        {
            get { return _isLeftFrontDoorOpen; }
            set
            {
                _isLeftFrontDoorOpen = value;
                NotifyPropertyChanged(m => m.IsLeftFrontDoorOpen);
            }
        }

        private bool _isLeftRearDoorOpen;
        public bool IsLeftRearDoorOpen
        {
            get { return _isLeftRearDoorOpen; }
            set
            {
                _isLeftRearDoorOpen = value;
                NotifyPropertyChanged(m => m.IsLeftRearDoorOpen);
            }
        }

        private bool _isRightFrontDoorOpen;
        public bool IsRightFrontDoorOpen
        {
            get { return _isRightFrontDoorOpen; }
            set
            {
                _isRightFrontDoorOpen = value;
                NotifyPropertyChanged(m => m.IsRightFrontDoorOpen);
            }
        }

        private bool _isRightRearDoorOpen;
        public bool IsRightRearDoorOpen
        {
            get { return _isRightRearDoorOpen; }
            set
            {
                _isRightRearDoorOpen = value;
                NotifyPropertyChanged(m => m.IsRightRearDoorOpen);
            }
        }
    }
}
