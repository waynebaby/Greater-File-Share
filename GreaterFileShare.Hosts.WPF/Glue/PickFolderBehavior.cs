using Microsoft.WindowsAPICodePack.Dialogs;
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
    public class PickFolderBehavior : Behavior<Button>
    {

        protected override void OnAttached()
        {
            base.OnAttached();

            FrameworkElement e = AssociatedObject;

            while (e.Parent != null)
            {
                e = e.Parent as FrameworkElement;
            }
            w = e as Window;

            AssociatedObject.Click += AssociatedObject_Click;
        }

        Window w;

        private void AssociatedObject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog()
                {
                    IsFolderPicker = true
                })
            {

                var f = dialog.ShowDialog(w);
                if (f == CommonFileDialogResult.Ok)
                {
                    Folder = dialog.FileName;
                }
            }
        }



        public string Folder
        {
            get { return (string)GetValue(FolderProperty); }
            set { SetValue(FolderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Folder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FolderProperty =
            DependencyProperty.Register("Folder", typeof(string), typeof(PickFolderBehavior), new PropertyMetadata(""));



        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Click -= AssociatedObject_Click;
        }
    }
}
