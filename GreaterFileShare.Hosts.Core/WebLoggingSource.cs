using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.Core
{
    public class WebLoggingSource : IObservable<string>
    {


        public IDisposable Subscribe(IObserver<string> observer)
        {
            return Observable.FromEventPattern<string>(
                  eh => Web.EventLogger.Instance.NewLogMessage += eh,
                  eh => Web.EventLogger.Instance.NewLogMessage -= eh
              )
              .Select(x => x.EventArgs)
              .Subscribe(observer);
        }
    }
}
