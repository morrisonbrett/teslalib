using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeslaLib.Models;

namespace TeslaLib.Test
{
    [TestClass]
    public class FieldAnalysisTest
    {

        private TeslaClient client;

        public FieldAnalysisTest()
        {
            client = new TeslaClient(true);
        }
        
        [TestMethod]
        public void AddedFieldsTest()
        {
            var fields = client.AnalyzeFields<ClimateStateStatus>(@"{
            ""inside_temp"": 17.0,
            ""outside_temp"": 9.5,
            ""driver_temp_setting"": 22.6,
            ""passenger_temp_setting"": 22.6,
            ""is_auto_conditioning_on"": false,
            ""is_front_defroster_on"": null,
            ""is_rear_defroster_on"": false,
            ""fan_status"": 0,
            ""new_field"" : 10,
            }");

            Assert.AreEqual(1, fields.Values.Where(f => f == FieldType.ADDED).Count());
            Assert.AreEqual(0, fields.Values.Where(f => f == FieldType.REMOVED).Count());
            Assert.AreEqual(8, fields.Values.Where(f => f == FieldType.UNCHANGED).Count());
        }

        [TestMethod]
        public void RemovedFieldsTest()
        {
            var fields = client.AnalyzeFields<ClimateStateStatus>(@"{
            ""inside_temp"": 17.0,
            ""outside_temp"": 9.5,
            ""driver_temp_setting"": 22.6,
            ""passenger_temp_setting"": 22.6,
            ""is_auto_conditioning_on"": false,
            ""is_front_defroster_on"": null,
            ""is_rear_defroster_on"": false,
            }");

            Assert.AreEqual(0, fields.Values.Where(f => f == FieldType.ADDED).Count());
            Assert.AreEqual(1, fields.Values.Where(f => f == FieldType.REMOVED).Count());
            Assert.AreEqual(7, fields.Values.Where(f => f == FieldType.UNCHANGED).Count());
        }

        [TestMethod]
        public void UnchagedFieldsTest()
        {
            var fields = client.AnalyzeFields<ClimateStateStatus>(@"{
            ""inside_temp"": 17.0,
            ""outside_temp"": 9.5,
            ""driver_temp_setting"": 22.6,
            ""passenger_temp_setting"": 22.6,
            ""is_auto_conditioning_on"": false,
            ""is_front_defroster_on"": null,
            ""is_rear_defroster_on"": false,
            ""fan_status"": 0,
            }");

            Assert.AreEqual(0, fields.Values.Where(f => f == FieldType.ADDED).Count());
            Assert.AreEqual(0, fields.Values.Where(f => f == FieldType.REMOVED).Count());
            Assert.AreEqual(8, fields.Values.Where(f => f == FieldType.UNCHANGED).Count());
        }
    }
}
