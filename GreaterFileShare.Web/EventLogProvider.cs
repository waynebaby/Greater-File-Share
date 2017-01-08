using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreaterFileShare.Web
{
    public class EventLogProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return EventLogger.Instance;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
