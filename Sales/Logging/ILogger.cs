using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Logging
{
    internal interface ILogger
    {
        void Log(String message);
        void Log(String message, LogLevel Level);
        void Log(String message, LogLevel Level, String CkassName, String MetodName);

        void Log(String message, LogLevel Level, String ClassName, String MetodName, object info);
    }
}
