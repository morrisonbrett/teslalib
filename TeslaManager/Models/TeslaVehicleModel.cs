using System;
using System.Linq;
using System.Collections.Generic;
using TeslaLib.Models;

using SimpleMvvmToolkit;

namespace TeslaManager
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

        private bool isFrunkOpen;
        public bool IsFrunkOpen
        {
            get { return isFrunkOpen; }
            set
            {
                isFrunkOpen = value;
                NotifyPropertyChanged(m => m.IsFrunkOpen);
            }
        }

        private bool isTrunkOpen;
        public bool IsTrunkOpen
        {
            get { return isTrunkOpen; }
            set
            {
                isTrunkOpen = value;
                NotifyPropertyChanged(m => m.IsTrunkOpen);
            }
        }

        private bool isLeftFrontDoorOpen;
        public bool IsLeftFrontDoorOpen
        {
            get { return isLeftFrontDoorOpen; }
            set
            {
                isLeftFrontDoorOpen = value;
                NotifyPropertyChanged(m => m.IsLeftFrontDoorOpen);
            }
        }

        private bool isLeftRearDoorOpen;
        public bool IsLeftRearDoorOpen
        {
            get { return isLeftRearDoorOpen; }
            set
            {
                isLeftRearDoorOpen = value;
                NotifyPropertyChanged(m => m.IsLeftRearDoorOpen);
            }
        }

        private bool isRightFrontDoorOpen;
        public bool IsRightFrontDoorOpen
        {
            get { return isRightFrontDoorOpen; }
            set
            {
                isRightFrontDoorOpen = value;
                NotifyPropertyChanged(m => m.IsRightFrontDoorOpen);
            }
        }

        private bool isRightRearDoorOpen;
        public bool IsRightRearDoorOpen
        {
            get { return isRightRearDoorOpen; }
            set
            {
                isRightRearDoorOpen = value;
                NotifyPropertyChanged(m => m.IsRightRearDoorOpen);
            }
        }
                
    }
}
