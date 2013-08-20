using System.IO;

namespace TeslaLib.Streaming
{
    [StreamingFormat(StreamingOutputFormat.KML_PLACEMARK)]
    public class KmlPlacemarkStreamer : AStreamer
    {
        private StreamWriter _writer;

        public override void Setup(string filePath, string valueString, string tripTitle)
        {
            _writer = new StreamWriter(filePath) {AutoFlush = true};
        }

        public override void BeforeStreaming()
        {
            _writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            _writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2"">");
            _writer.WriteLine("<Document>");
        }

        public override void AfterStreaming()
        {
            if (NeedsToClose())
            {
                _writer.WriteLine("</Document>");
                _writer.WriteLine("</kml>");

                _writer.Close();
            }
        }

        public override bool NeedsToClose()
        {
            return _writer != null && _writer.BaseStream != null;
        }

        public override void DataRecevied(string line)
        {
            const string speed = "";
            const string latitude = "";
            const string longitude = "";
            const string elevation = "";

            _writer.WriteLine("<Placemark>");
            _writer.WriteLine("<Name>" + speed + "</Name");
            _writer.WriteLine("<Point>");
            _writer.WriteLine("<coordinates>{0},{1},{2}</coordinates>", longitude, latitude, elevation);
            _writer.WriteLine("</Point>");
            _writer.WriteLine("</Placemark>");
        }
    }
}
