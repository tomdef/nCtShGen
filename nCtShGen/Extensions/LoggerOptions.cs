using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace nCtShGen
{
    public class LoggerOptions : Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions
    {
        public LoggerOptions() : base()
        {
            this.TimestampFormat = "HH:mm:ss.fff";
        }
    }
}