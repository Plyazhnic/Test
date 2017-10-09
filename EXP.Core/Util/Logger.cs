using System;
using System.Diagnostics;
using log4net;

namespace EXP.Core.Util
{
    public static class Logger
    {
        static Logger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private static ILog Log
        {
            get
            {
                Type type;

                try
                {
                    int index = 0;
                    var stackTrace = new StackTrace();
                    while (stackTrace.GetFrame(index).GetMethod().DeclaringType == typeof(Logger))
                    {
                        index++;
                    }

                    type = stackTrace.GetFrame(index).GetMethod().DeclaringType;
                }
                catch
                {
                    type = typeof(Logger);
                }

                return LogManager.GetLogger(type);
            }
        }

        public static void DebugFormat(string format, params object[] args)
        {
            Log.DebugFormat(format, args);
        }

        public static void Debug(object message)
        {
            Log.Debug(message);
        }

        public static void Debug(object message, Exception exc)
        {
            Log.Debug(message, exc);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            Log.ErrorFormat(format, args);
        }

        public static void Error(object message)
        {
            Log.Error(message);
        }

        public static void Error(object message, Exception exc)
        {
            Log.Error(message, exc);
        }
    }
}