using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Logging
{
    public class FileLogger : ILogger
    {
        private readonly String filename;

        public FileLogger()
        {
            String projectPath = AppContext.BaseDirectory.Substring(0,AppContext.BaseDirectory.IndexOf(@"\bin\"));
            filename = Path.Combine(projectPath, "logs.txt");
        }

        public void Log(String message) 
        {
            this.Log(message, LogLevel.Debug);
        }

        public void Log(String message, LogLevel Level)
        {
            this.Log(message, Level, "", "");
        }

        public void Log(String message, LogLevel Level, String ClassName, String MetodName)
        {
            this.Log(message, Level, ClassName, MetodName, null);
        }

        public void Log(String message, LogLevel Level, String ClassName, String MetodName, object info)
        {
            File.AppendAllText(filename, $"{DateTime.Now} | {Level} | {message} | {ClassName}.{MetodName} | {info}\r\n");
        }
    }
}
