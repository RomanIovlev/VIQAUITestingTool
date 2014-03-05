using System;
using System.Collections.Concurrent;
using System.IO;
using VIQA.Common.Interfaces;

namespace VIQA.Common
{
    public class DefaultLogger : ILogger
    {
        private readonly string _logFileFormat = "{0}_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log";
        private static readonly ConcurrentDictionary<string, object> LogFileSyncRoots = new ConcurrentDictionary<string, object>();
        private static readonly string LogRecordTemplate = Environment.NewLine + "[{0}] {1}: {2}" + Environment.NewLine;

        private static string GetLogRecord(string typeName, string msg)
        {
            return string.Format(LogRecordTemplate, typeName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), msg);
        }

        private void InLog(string fileName, string typeName, string msg)
        {
            var logDirectory = ".Log\\" + fileName + "s\\";
            CreateDirectory(logDirectory);
            //var files = new DirectoryInfo(LogDirectory).GetFiles();
            var logFileName = logDirectory + string.Format(_logFileFormat, fileName);

            var logFileSyncRoot = LogFileSyncRoots.GetOrAdd(logFileName, s => s);
            lock (logFileSyncRoot)
            {
                File.AppendAllText(logFileName, GetLogRecord(typeName, msg));
            }
        }

        private static void CreateDirectory(string directoryName)
        {
            if (!File.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
        }

        public void Event(string msg)
        {
            InLog("Event", "Event", msg);
        }

        public void Error(string msg)
        {
            InLog("Error", "Error", msg);
            InLog("Event", "Error", msg);
        }
    }
}
