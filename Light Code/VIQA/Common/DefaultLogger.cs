using System;
using System.Configuration;
using System.IO;
using System.Collections.Concurrent;
using System.Linq;
using VIQA.Common.Interfaces;

namespace VIQA.Common
{
    public class DefaultLogger : ILogger
    {
        private static readonly string _logFileFormat = "{0}_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log";
        private static readonly ConcurrentDictionary<string, object> LogFileSyncRoots = new ConcurrentDictionary<string, object>();
        private static readonly string LogRecordTemplate = Environment.NewLine + "[{0}] {1}: {2}" + Environment.NewLine;
        private string LogDirectoryRoot = ".Log\\";

        private static string GetLogRecord(string typeName, string msg)
        {
            return string.Format(LogRecordTemplate, typeName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), msg);
        }

        public DefaultLogger()
        {
            var appSetting = GetValidUrl(ConfigurationSettings.AppSettings["VILogPath"]);
            if (!string.IsNullOrEmpty(appSetting))
                LogDirectoryRoot = appSetting;
        }

        public DefaultLogger(string path)
        {
            LogDirectoryRoot = path;
        }

        private string GetValidUrl(string logPath)
        {
            if (string.IsNullOrEmpty(logPath))
                return "";
            var result = logPath.Replace("/", "\\");
            if (result[1] != ':' && result.Substring(0, 3) != "..\\")
                result = (result[0] == '\\')
                    ? ".." + result
                    : "..\\" + result;
            return (result.Last() == '\\')
                ? result
                : result + "\\";
        }

        private void InLog(string fileName, string typeName, string msg)
        {
            var logDirectory = GetValidUrl(LogDirectoryRoot) + fileName + "s\\";
            CreateDirectory(logDirectory);
            var logFileName = logDirectory + string.Format(_logFileFormat, fileName);

            var logFileSyncRoot = LogFileSyncRoots.GetOrAdd(logFileName, s => s);
            lock (logFileSyncRoot)
            {
                File.AppendAllText(logFileName, GetLogRecord(typeName, msg));
            }
        }

        private void CreateDirectory(string directoryName)
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

