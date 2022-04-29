using Microsoft.Extensions.Logging;

namespace vmProjectBFF
{
    public class AppLogger
    {
        private static ILoggerFactory _Factory = null;

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_Factory == null)
                {
                    _Factory = new LoggerFactory();
                }
                return _Factory;
            }
            set { _Factory = value; }
        }
        public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
    }
}
