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
    public class TaskStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ShareFileTask st = value as ShareFileTask;
            if (st != null)
            {

                return st.IsLastStartFailed ? "Error" : (st.IsHosting ?"Hosting" : "NoError");
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
