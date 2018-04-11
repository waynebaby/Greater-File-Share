using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
namespace GreaterFileShare.Hosts.WPF.Glue
{
    public class ScrollToSelectedCommand :ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {

            return true;
        }

        public void Execute(object parameter)
        {

            var lb = parameter as ListBox;
            if (lb.SelectedItem != null)
            {
                lb.ScrollIntoView(lb.SelectedItem);
            }
        }


       
    }
}
