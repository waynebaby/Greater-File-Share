using GreaterFileShare.Hosts.WPF.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GreaterFileShare.Hosts.WPF.Glue
{
    public class HostNameWCFUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return new Uri($"net.tcp://{value??"localhost"}:{Consts.WCFPort}/{Consts.WCFRelativeUri}");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
