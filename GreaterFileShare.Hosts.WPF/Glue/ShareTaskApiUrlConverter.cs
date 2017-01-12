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
    public class ShareTaskApiUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ShareFileTask ts;
            if ((ts=value as ShareFileTask)!=null)
            {
                return new Uri( $"http://localhost:{ts.Port}/{Consts.ApiRelativeUri}");
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
