using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GreaterFileShare.Hosts.WPF.Glue
{
    public class TreeViewSelectedItemBindingBehavior : Behavior<TreeView>
    {

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectedItemChanged += AssociatedObject_SelectedItemChanged;

        }




        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { /*SetValue(SelectedItemProperty, value);*/ }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(TreeViewSelectedItemBindingBehavior), new PropertyMetadata(null));



        private void AssociatedObject_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.SelectedItemChanged -= AssociatedObject_SelectedItemChanged;
            base.OnDetaching();
        }
    }
}
