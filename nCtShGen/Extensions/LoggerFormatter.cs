using System.Drawing.Printing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace nCtShGen
{
    public class LoggerFormatter : Microsoft.Extensions.Logging.Console.ConsoleFormatter, IDisposable
    {
        private readonly IDisposable reloadToken;
        private nCtShGen.LoggerOptions loggerOptions = default!;

        public LoggerFormatter(IOptionsMonitor<nCtShGen.LoggerOptions> options) : base("nCtShGenLoggerFormatter")
            => (reloadToken, loggerOptions) = (options.OnChange(ReloadLoggerOptions), options.CurrentValue);


        private void ReloadLoggerOptions(nCtShGen.LoggerOptions options) => loggerOptions = options;

        public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
        {
            string? message =
                logEntry.Formatter?.Invoke(
                    logEntry.State, logEntry.Exception);


            if (message is null)
            {
                return;
            }

            string currentTime = DateTime.Now.ToString(loggerOptions.TimestampFormat);

            //textWriter.Write($"{currentTime} ");
            Console.Write($"{currentTime} ");

            WriteLevel(logEntry.LogLevel);

            Console.WriteLine($" {logEntry.State}");
        }

        private void WriteLevel(LogLevel logLevel)
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;
            ConsoleColor backgroundColor = Console.BackgroundColor;
            string txt = logLevel.ToString();
            switch (logLevel)
            {
                case LogLevel.Information:
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Blue;
                        txt = "Info ";
                        break;
                    }
                case LogLevel.Warning:
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        txt = "Warn ";
                        break;
                    }
                case LogLevel.Error:
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Red;
                        txt = "Error";
                        break;
                    }
                case LogLevel.Critical:
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        txt = "Crit ";
                        break;
                    }
                default:
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        break;
                    }
            }

            Console.Write($" {txt} ");
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write($"");
        }

        public void Dispose() => reloadToken?.Dispose();
    }
}