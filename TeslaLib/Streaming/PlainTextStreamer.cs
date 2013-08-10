using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeslaLib.Streaming
{
    [StreamingFormat(StreamingOutputFormat.PLAIN_TEXT)]
    public class PlainTextStreamer : AStreamer
    {

        private StreamWriter writer;

        public override void Setup(string filePath, string valueString, string tripTitle)
        {
            writer = new StreamWriter(filePath);
            writer.AutoFlush = true;
        }

        public override void AfterStreaming()
        {
            if (NeedsToClose())
            {
                writer.Close();
            }
        }

        public override void DataRecevied(string line)
        {
            writer.WriteLine(line);
        }

        public override bool NeedsToClose()
        {
            return writer != null && writer.BaseStream != null;
        }
    }
}
