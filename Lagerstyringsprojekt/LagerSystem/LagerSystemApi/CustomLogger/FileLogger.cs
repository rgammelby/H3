using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace LagerSystemApi.CustomLogger
{
    public class FileLogger : ILogger
    {
        private readonly string _path;
        private readonly object _lock = new object();

        public FileLogger(string path)
        {
            _path = path;
        }

        public IDisposable? BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {formatter(state, exception)}";
            var separator = new string('-', 80);

            lock (_lock) // Prevent multiple threads from writing at the same time
            {
                File.AppendAllText(_path, $"{separator}{Environment.NewLine}{logEntry}{Environment.NewLine}{Environment.NewLine}");
            }
        }
    }
}
