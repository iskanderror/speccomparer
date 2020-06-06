using Comparator.Extensions;
using System;

namespace Comparator.Logger
{
    public class SimpleLogger : NotifiedEntity, ILogger
    {
        private string logData;

        public string LogData { get => logData; set => SetProperty(ref logData, value); }
        public SimpleLogger()
        {
            logData = "";
        }

        public void Log(string str)
        {
            LogData += $"{DateTime.Now.ToString()} - {str}\n";
        }

    }
}
