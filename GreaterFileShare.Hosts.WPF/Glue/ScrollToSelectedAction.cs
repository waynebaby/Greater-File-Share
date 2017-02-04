using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;
namespace GreaterFileShare.Hosts.WPF.Glue
{
    public class ScrollToSelectedAction :TriggerAction<ListBox>
    {
        protected override void Invoke(object parameter)
        {
            var lb = AssociatedObject;
            if (lb.SelectedItem != null)
            {
                lb.ScrollIntoView(lb.SelectedItem);
            }
        }

       
    }
}
