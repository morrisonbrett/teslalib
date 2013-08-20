using System.IO;

namespace TeslaLib.Streaming
{
    [StreamingFormat(StreamingOutputFormat.PLAIN_TEXT)]
    public class PlainTextStreamer : AStreamer
    {
        private StreamWriter _writer;

        public override void Setup(string filePath, string valueString, string tripTitle)
        {
            _writer = new StreamWriter(filePath) {AutoFlush = true};
        }

        public override void AfterStreaming()
        {
            if (NeedsToClose())
            {
                _writer.Close();
            }
        }

        public override void DataRecevied(string line)
        {
            _writer.WriteLine(line);
        }

        public override bool NeedsToClose()
        {
            return _writer != null && _writer.BaseStream != null;
        }
    }
}
