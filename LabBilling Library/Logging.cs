using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Targets;

namespace LabBilling.Logging
{
    public static class Log
    {
        public static Logger Instance { get; private set; }

        static Log()
        {
            Instance = LogManager.GetCurrentClassLogger();

        }

    }
}
