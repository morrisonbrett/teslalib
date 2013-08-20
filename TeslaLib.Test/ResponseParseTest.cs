using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TeslaLib.Models;

namespace TeslaLib.Test
{
    [TestClass]
    public class ResponseParseTest
    {
        private readonly TeslaClient _client;

        public ResponseParseTest()
        {
            _client = new TeslaClient(true);
        }

        [TestMethod]
        public void ParseVehiclesTest()
        {
            var cars = _client.ParseVehicles(@"[{
            ""color"": null,
            ""display_name"": null,
            ""id"": 321,
            ""option_codes"": ""MS01,RENA,TM00,DRLH,PF00,BT85,PBCW,RFPO,WT19,IBMB,IDPB,TR00,SU01,SC01,TP01,AU01,CH00,HP00,PA00,PS00,AD02,X020,X025,X001,X003,X007,X011,X013"",
            ""user_id"": 123,
            ""vehicle_id"": 1234567890,
            ""vin"": ""5YJSA1CN5CFP01657"",
            ""tokens"": [""x"", ""x""],
            ""state"": ""online""
        }]");

            Assert.IsNotNull(cars);

            Assert.AreEqual(1, cars.Count);

            var car = cars.FirstOrDefault();

            Assert.IsNotNull(car);

            Assert.AreEqual(null, car.Color);
            Assert.AreEqual(null, car.DisplayName);
            Assert.AreEqual(321, car.Id);

            // TODO: Option Codes

            Assert.AreEqual(VehicleState.ONLINE, car.State);
            Assert.AreEqual(2, car.Tokens.Count);
            Assert.AreEqual(123, car.UserId);
            Assert.AreEqual(1234567890, car.VehicleId);
            Assert.AreEqual("5YJSA1CN5CFP01657", car.VIN);
        }

        [TestMethod]
        public void ParseMobileEnabledTest()
        {
            var status = _client.ParseMobileEnabledStatus(@"{
            ""reason"":"""",
            ""result"":true
            }");

            Assert.IsNotNull(status);
            Assert.AreEqual("", status.Reason);
            Assert.AreEqual(true, status.Result);
        }

        [TestMethod]
        public void ParseChargeStateStatus44Test()
        {
            var status = _client.ParseChargeStateStatus(@"{
            ""charging_state"": ""Complete"",
            ""charge_to_max_range"": false,
            ""max_range_charge_counter"": 0,
            ""fast_charger_present"": false,
            ""battery_range"": 239.02,
            ""est_battery_range"": 155.79,
            ""ideal_battery_range"": 275.09,
            ""battery_level"": 91,
            ""battery_current"": -0.6,
            ""charge_starting_range"": null,
            ""charge_starting_soc"": null,
            ""charger_voltage"": 0,
            ""charger_pilot_current"": 40,
            ""charger_actual_current"": 0,
            ""charger_power"": 0,
            ""time_to_full_charge"": null,
            ""charge_rate"": -1.0,
            ""charge_port_door_open"": true
            }");

            Assert.IsNotNull(status);
            Assert.AreEqual(-0.6, status.BatteryCurrent);
            Assert.AreEqual(91, status.BatteryLevel);
            Assert.AreEqual(239.02, status.BatteryRange);
            Assert.AreEqual(0, status.ChargerActualCurrent);
            Assert.AreEqual(-1.0, status.ChargeRate);
            Assert.AreEqual(40, status.ChargerPilotCurrent);
            Assert.AreEqual(0, status.ChargerPower);
            Assert.AreEqual(0, status.ChargerVoltage);
            Assert.AreEqual(null, status.ChargeStartingRange);
            Assert.AreEqual(null, status.ChargeStartingSoc);
            Assert.AreEqual(ChargingState.COMPLETE, status.ChargingState);
            Assert.AreEqual(155.79, status.EstimatedBatteryRange);
            Assert.AreEqual(275.09, status.IdealBatteryRange);
            Assert.AreEqual(true, status.IsChargePortDoorOpen);
            Assert.AreEqual(false, status.IsChargeToMaxRange);
            Assert.AreEqual(false, status.IsUsingSupercharger);
            Assert.AreEqual(0, status.MaxRangeChargeCounter);
            Assert.AreEqual(null, status.TimeUntilFullCharge);
        }

        [TestMethod]
        public void ParseChargeStateStatus45Test()
        {
            var status = _client.ParseChargeStateStatus(@"{
            ""battery_current"": -0.6,
            ""battery_heater_on"": false,
            ""battery_level"": 90,
            ""battery_range"": 234.1,
            ""charge_enable_request"": true,
            ""charge_limit_soc"": 90,
            ""charge_limit_soc_max"": 100,
            ""charge_limit_soc_min"": 50,
            ""charge_limit_soc_std"": 90,
            ""charge_port_door_open"": true,
            ""charge_rate"": -1.0,
            ""charge_starting_range"": null,
            ""charge_starting_soc"": null,
            ""charge_to_max_range"": false,
            ""charger_actual_current"": 0,
            ""charger_pilot_current"": 60,
            ""charger_power"": 0,
            ""charger_voltage"": 0,
            ""charging_state"": ""Complete"",
            ""est_battery_range"": 186.16,
            ""fast_charger_present"": false,
            ""ideal_battery_range"": 269.43,
            ""max_range_charge_counter"": 0,
            ""not_enough_power_to_heat"": false,
            ""scheduled_charging_pending"": false,
            ""scheduled_charging_start_time"": null,
            ""time_to_full_charge"": 0.0,
            ""user_charge_enable_request"": null
            }");

            Assert.IsNotNull(status);
            Assert.AreEqual(-0.6, status.BatteryCurrent);
            Assert.AreEqual(90, status.BatteryLevel);
            Assert.AreEqual(234.1, status.BatteryRange);
            Assert.AreEqual(90, status.ChargeLimitSoc);
            Assert.AreEqual(100, status.ChargeLimitSocMax);
            Assert.AreEqual(50, status.ChargeLimitSocMin);
            Assert.AreEqual(90, status.ChargeLimitSocStd);
            Assert.AreEqual(0, status.ChargerActualCurrent);
            Assert.AreEqual(-1.0, status.ChargeRate);
            Assert.AreEqual(60, status.ChargerPilotCurrent);
            Assert.AreEqual(0, status.ChargerPower);
            Assert.AreEqual(0, status.ChargerVoltage);
            Assert.AreEqual(null, status.ChargeStartingRange);
            Assert.AreEqual(null, status.ChargeStartingSoc);
            Assert.AreEqual(ChargingState.COMPLETE, status.ChargingState);
            Assert.AreEqual(186.16, status.EstimatedBatteryRange);
            Assert.AreEqual(269.43, status.IdealBatteryRange);
            Assert.AreEqual(false, status.IsBatteryHeaterOn);
            Assert.AreEqual(true, status.IsChargeEnableRequest);
            Assert.AreEqual(true, status.IsChargePortDoorOpen);
            Assert.AreEqual(false, status.IsChargeToMaxRange);
            Assert.AreEqual(false, status.IsNotEnoughPowerToHeat);
            Assert.AreEqual(null, status.IsUserChargeEnableRequest);
            Assert.AreEqual(false, status.IsUsingSupercharger);
            Assert.AreEqual(0, status.MaxRangeChargeCounter);
            Assert.AreEqual(false, status.ScheduledChargingPending);
            Assert.AreEqual(null, status.ScheduledChargingStartTime);
            Assert.AreEqual(0.0, status.TimeUntilFullCharge);
        }

        [TestMethod]
        public void ParseClimateStateStatusTest()
        {
            var status = _client.ParseClimateStateStatus(@"{
            ""inside_temp"": 17.0,
            ""outside_temp"": 9.5,
            ""driver_temp_setting"": 22.6,
            ""passenger_temp_setting"": 22.6,
            ""is_auto_conditioning_on"": false,
            ""is_front_defroster_on"": null,
            ""is_rear_defroster_on"": false,
            ""fan_status"": 0
            }");

            Assert.IsNotNull(status);

            Assert.AreEqual(22.6, status.DriverTemperatureSetting);
            Assert.AreEqual(0, status.FanStatus);
            Assert.AreEqual(17.0, status.InsideTemperature);
            Assert.AreEqual(false, status.IsAutoAirConditioning);
            Assert.AreEqual(null, status.IsFrontDefrosterOn);
            Assert.AreEqual(false, status.IsRearDefrosterOn);
            Assert.AreEqual(9.5, status.OutsideTemperature);
            Assert.AreEqual(22.6, status.PassengerTemperatureSetting);
        }

        [TestMethod]
        public void ParseDriveStateStatusTest()
        {
            var status = _client.ParseDriveStateStatus(@"{
            ""shift_state"": null,
            ""speed"": null,
            ""latitude"": 33.794839,
            ""longitude"": -84.401593,
            ""heading"": 4,
            ""gps_as_of"": 1359863204
            }");

            Assert.IsNotNull(status);
            Assert.AreEqual(1359863204, status.GpsAsOf.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()).Ticks / 10000000);
            Assert.AreEqual(4, status.Heading);
            Assert.AreEqual(33.794839, status.Latitude);
            Assert.AreEqual(-84.401593, status.Longitude);
            Assert.AreEqual(null, status.ShiftState);
            Assert.AreEqual(null, status.Speed);
        }

        [TestMethod]
        public void ParseGuiStateStatusTest()
        {
            var status = _client.ParseGuiStateStatus(@"{
            ""gui_distance_units"": ""mi/hr"",
            ""gui_temperature_units"": ""F"",
            ""gui_charge_rate_units"": ""mi/hr"",
            ""gui_24_hour_time"": false,
            ""gui_range_display"": ""Rated""
            }");

            Assert.IsNotNull(status);
            Assert.AreEqual("mi/hr", status.ChargeRateUnits);
            Assert.AreEqual("mi/hr", status.DistanceUnits);
            Assert.AreEqual(false, status.Is24HourTime);
            Assert.AreEqual("Rated", status.RangeDisplay);
            Assert.AreEqual(TemperatureUnits.FAHRENHEIT, status.TemperatureUnits);

        }

        [TestMethod]
        public void ParseVehicleStateStatusTest()
        {
            var status = _client.ParseVehicleStateStatus(@"{
            ""df"": false,
            ""dr"": false,
            ""pf"": false,
            ""pr"": false,
            ""ft"": false,
            ""rt"": false,
            ""car_verson"": ""1.19.42"",
            ""locked"": true,
            ""sun_roof_installed"": false,
            ""sun_roof_state"": ""unknown"",
            ""sun_roof_percent_open"": 0,
            ""dark_rims"": false,
            ""wheel_type"": ""Base19"",
            ""has_spoiler"": false,
            ""roof_color"": ""Colored"",
            ""perf_config"": ""Base""
            }");

            Assert.IsNotNull(status);
            Assert.AreEqual("1.19.42", status.CarVersion);
            Assert.AreEqual(false, status.HasPanoramicRoof);
            Assert.AreEqual(false, status.HasSpoiler);
            Assert.AreEqual(false, status.HasDarkRims);
            Assert.AreEqual(false, status.IsDriverFrontDoorOpen);
            Assert.AreEqual(false, status.IsDriverRearDoorOpen);
            Assert.AreEqual(false, status.IsFrontTrunkOpen);
            Assert.AreEqual(true, status.IsLocked);
            Assert.AreEqual(false, status.IsPassengerFrontDoorOpen);
            Assert.AreEqual(false, status.IsPassengerRearDoorOpen);
            Assert.AreEqual(false, status.IsRearTrunkOpen);
            Assert.AreEqual(0, status.PanoramicRoofPercentOpen);
            Assert.AreEqual(PanoramicRoofState.UNKNOWN, status.PanoramicRoofState);
            Assert.AreEqual(PerformanceConfiguration.BASE, status.PerformanceConfiguration);
            Assert.AreEqual(RoofType.COLORED, status.RoofColor);
            Assert.AreEqual(WheelType.BASE_19, status.WheelType);
        }

        [TestMethod]
        public void ParseResultStatusTest()
        {
            var status = _client.ParseMobileEnabledStatus(@"{
            ""reason"":""failure reason"",
            ""result"":false
            }");

            Assert.IsNotNull(status);
            Assert.AreEqual("failure reason", status.Reason);
            Assert.AreEqual(false, status.Result);
        }
    }
}
