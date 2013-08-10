using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public enum StreamingOutputFormat : int
    {
        PLAIN_TEXT = 0,
        KML_PLACEMARK = 1,
        KML_PATH = 2,
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class StreamingFormatAttribute : Attribute
    {
        readonly StreamingOutputFormat outputFormat;

        public StreamingFormatAttribute(StreamingOutputFormat outputFormat)
        {
            this.outputFormat = outputFormat;
        }

        public StreamingOutputFormat OutputFormat
        {
            get { return outputFormat; }
        }
    }
}
