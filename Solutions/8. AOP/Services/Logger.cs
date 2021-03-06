﻿using System.IO;
using System.Text;
using System.Xml.Serialization;
using Topshelf.Logging;

namespace WindowsService
{
    public static class Logger
    {
        public static void LogAfter(string methodName, object returnValue)
        {
            var message = $"Result {methodName}: [{ObjectToString(returnValue)}]";
            HostLogger.Get<FileMonitoringService>().Debug(message);
        }

        public static void LogBefore(string methodName, object[] arguments)
        {
            var argsString = GetArgsString(arguments);
            var message = $"Call {methodName}({argsString})";
            HostLogger.Get<FileMonitoringService>().Debug(message);
        }

        private static string GetArgsString(object[] args)
        {
            var result = new StringBuilder();
            foreach (var arg in args)
            {
                result.Append($"[{ObjectToString(arg)}], ");
            }

            if (result.Length > 2)
            {
                result.Remove(result.Length - 2, 2);
            }

            return result.ToString();
        }

        private static string ObjectToString(object obj)
        {
            var objType = obj.GetType();
            if (objType == typeof(string))
            {
                return obj.ToString();
            }

            try
            {
                var serializer = new XmlSerializer(objType);
                var writer = new StringWriter();
                serializer.Serialize(writer, obj);
                return writer.ToString();
            }
            catch
            {
                return "<Not serializable>";
            }
        }
    }
}
