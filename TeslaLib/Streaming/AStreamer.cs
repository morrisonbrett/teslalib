using System;

namespace TeslaLib.Streaming
{
    public abstract class AStreamer
    {
        public abstract bool NeedsToClose();

        public virtual void Setup(string filePath, string valueString, string tripTitle)
        {
        }

        public virtual void BeforeStreaming()
        {
        }

        public virtual void AfterStreaming()
        {
        }

        public virtual void DataRecevied(string line)
        {
        }
    }

    public enum StreamingOutputFormat
    {
        PLAIN_TEXT = 0,
        KML_PLACEMARK = 1,
        KML_PATH = 2,
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class StreamingFormatAttribute : Attribute
    {
        readonly StreamingOutputFormat _outputFormat;

        public StreamingFormatAttribute(StreamingOutputFormat outputFormat)
        {
            _outputFormat = outputFormat;
        }

        public StreamingOutputFormat OutputFormat
        {
            get { return _outputFormat; }
        }
    }
}
