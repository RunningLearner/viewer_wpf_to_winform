using Microsoft.Extensions.Logging;

namespace dicom_viewer_winform.Entities
{
    public static class Logging
    {
        private static ILoggerFactory? _factory;

        private static void EnsureFactory()
        {
            if (_factory == null)
            {
                _factory = LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                });
            }
        }

        public static ILogger GetLogger(string category)
        {
            EnsureFactory();
            return _factory!.CreateLogger(category);
        }

        public static ILogger GetLogger<T>()
        {
            EnsureFactory();
            return _factory!.CreateLogger<T>();
        }
    }
}
