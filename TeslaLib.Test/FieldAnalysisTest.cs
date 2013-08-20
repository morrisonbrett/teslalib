using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TeslaLib.Models;

namespace TeslaLib.Test
{
    [TestClass]
    public class FieldAnalysisTest
    {
        private readonly TeslaClient _client;

        public FieldAnalysisTest()
        {
            _client = new TeslaClient(true);
        }
        
        [TestMethod]
        public void AddedFieldsTest()
        {
            var fields = _client.AnalyzeFields<ClimateStateStatus>(@"{
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

            Assert.AreEqual(1, fields.Values.Count(f => f == FieldType.ADDED));
            Assert.AreEqual(0, fields.Values.Count(f => f == FieldType.REMOVED));
            Assert.AreEqual(8, fields.Values.Count(f => f == FieldType.UNCHANGED));
        }

        [TestMethod]
        public void RemovedFieldsTest()
        {
            var fields = _client.AnalyzeFields<ClimateStateStatus>(@"{
            ""inside_temp"": 17.0,
            ""outside_temp"": 9.5,
            ""driver_temp_setting"": 22.6,
            ""passenger_temp_setting"": 22.6,
            ""is_auto_conditioning_on"": false,
            ""is_front_defroster_on"": null,
            ""is_rear_defroster_on"": false,
            }");

            Assert.AreEqual(0, fields.Values.Count(f => f == FieldType.ADDED));
            Assert.AreEqual(1, fields.Values.Count(f => f == FieldType.REMOVED));
            Assert.AreEqual(7, fields.Values.Count(f => f == FieldType.UNCHANGED));
        }

        [TestMethod]
        public void UnchagedFieldsTest()
        {
            var fields = _client.AnalyzeFields<ClimateStateStatus>(@"{
            ""inside_temp"": 17.0,
            ""outside_temp"": 9.5,
            ""driver_temp_setting"": 22.6,
            ""passenger_temp_setting"": 22.6,
            ""is_auto_conditioning_on"": false,
            ""is_front_defroster_on"": null,
            ""is_rear_defroster_on"": false,
            ""fan_status"": 0,
            }");

            Assert.AreEqual(0, fields.Values.Count(f => f == FieldType.ADDED));
            Assert.AreEqual(0, fields.Values.Count(f => f == FieldType.REMOVED));
            Assert.AreEqual(8, fields.Values.Count(f => f == FieldType.UNCHANGED));
        }
    }
}
