using Microsoft.AspNetCore.Server.Kestrel.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreaterFileShare.Web
{
    public class EventLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return new Disposable(() => { });
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string s;
            if (formatter != null)
            {
                s = string.Format("Level:{0}\t\t{1}", logLevel, formatter(state, exception));
            }
            else
            {
                s = string.Format("Level:{0}\t\t{1}", logLevel, state?.ToString(), exception?.Message);

            }
            NewLogMessage?.Invoke(this, s);
        }

        public static EventLogger Instance { get; private set; } = new EventLogger();
        public event EventHandler<string> NewLogMessage;
    }
}
