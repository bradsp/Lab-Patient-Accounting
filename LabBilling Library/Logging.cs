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

            //var configuration = new NLog.Config.LoggingConfiguration();

            //var fileTarget = new NLog.Targets.FileTarget("logfile") { FileName = "c:\\temp\\lab-billing-log.txt" };
            //var consoleTarget = new NLog.Targets.ConsoleTarget("logconsole");
            //var dbTarget = new NLog.Targets.DatabaseTarget("database") 
            //{ 
            //    ConnectionString = connectionString,
            //    CommandType = System.Data.CommandType.TableDirect,
            //    CommandText = @"INSERT INTO Logs(CreatedOn,Message,Level,Exception,StackTrace,Logger,HostName,Username,CallingSite,CallingSiteLineNumber,AppVersion) VALUES (@datetime,@msg,@level,@exception,@trace,@logger,@hostname,@user,@callsite,@lineno,@version)",
            //};

            //dbTarget.Parameters.Add(new DatabaseParameterInfo("@datetime", new NLog.Layouts.SimpleLayout("${date}")));
            //dbTarget.Parameters.Add(new DatabaseParameterInfo("@msg", new NLog.Layouts.SimpleLayout("${message}")));
            //dbTarget.Parameters.Add(new DatabaseParameterInfo("@level", new NLog.Layouts.SimpleLayout("${level}")));
            //dbTarget.Parameters.Add(new DatabaseParameterInfo("@exception", new NLog.Layouts.SimpleLayout("${exception}")));
            //dbTarget.Parameters.Add(new DatabaseParameterInfo("@trace", new NLog.Layouts.SimpleLayout("${stacktrace}")));
            //dbTarget.Parameters.Add(new DatabaseParameterInfo("@logger", new NLog.Layouts.SimpleLayout("${logger}")));
            //dbTarget.Parameters.Add(new DatabaseParameterInfo("@hostname", new NLog.Layouts.SimpleLayout("${hostname}")));
            //dbTarget.Parameters.Add(new DatabaseParameterInfo("@user", new NLog.Layouts.SimpleLayout("${environment-user}")));
            //dbTarget.Parameters.Add(new DatabaseParameterInfo("@callsite", new NLog.Layouts.SimpleLayout("${callsite}")));
            //dbTarget.Parameters.Add(new DatabaseParameterInfo("@lineno", new NLog.Layouts.SimpleLayout("${callsite-linenumber}")));
            //dbTarget.Parameters.Add(new DatabaseParameterInfo("@version", new NLog.Layouts.SimpleLayout("${assembly-version}")));

            //var dbRule = new LoggingRule("*", LogLevel.Trace, dbTarget);

            //configuration.AddRule(dbRule);

            //NLog.LogManager.Configuration = configuration;       

        }

    }
}
