using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeslaLib.Streaming
{
    [StreamingFormat(StreamingOutputFormat.KML_PLACEMARK)]
    public class KmlPlacemarkStreamer : AStreamer
    {

        private StreamWriter writer;

        public override void Setup(string filePath, string valueString, string tripTitle)
        {
            writer = new StreamWriter(filePath);
            writer.AutoFlush = true;
        }

        public override void BeforeStreaming()
        {
            writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2"">");
            writer.WriteLine("<Document>");
        }

        public override void AfterStreaming()
        {
            if (NeedsToClose())
            {
                writer.WriteLine("</Document>");
                writer.WriteLine("</kml>");

                writer.Close();
            }
        }

        public override bool NeedsToClose()
        {
            return writer != null && writer.BaseStream != null;
        }

        public override void DataRecevied(string line)
        {
            string speed = "";
            string latitude = "";
            string longitude = "";
            string elevation = "";

            writer.WriteLine("<Placemark>");
            writer.WriteLine("<Name>" + speed + "</Name");
            writer.WriteLine("<Point>");
            writer.WriteLine(string.Format("<coordinates>{0},{1},{2}</coordinates>", longitude, latitude, elevation));
            writer.WriteLine("</Point>");
            writer.WriteLine("</Placemark>");
        }
    }
}
