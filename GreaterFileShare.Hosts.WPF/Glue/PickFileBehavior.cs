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
    public class PickFileBehavior : Behavior<Button>
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
                    IsFolderPicker = false ,
                     
                })
            {

                var f = dialog.ShowDialog(w);
                if (f == CommonFileDialogResult.Ok)
                {
                    File = dialog.FileName;
                }
                else if(IsClearValueAfterFailNeeded)
                {
                    File = null;
                }
            }
        }




        public bool IsClearValueAfterFailNeeded
        {
            get { return (bool)GetValue(IsClearValueAfterFailNeededProperty); }
            set { SetValue(IsClearValueAfterFailNeededProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsClearValueAfterFailNeeded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsClearValueAfterFailNeededProperty =
            DependencyProperty.Register("IsClearValueAfterFailNeeded", typeof(bool), typeof(PickFileBehavior), new PropertyMetadata(false));




        public string File
        {
            get { return (string)GetValue(FileProperty); }
            set { SetValue(FileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for File.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileProperty =
            DependencyProperty.Register("File", typeof(string), typeof(PickFileBehavior), new PropertyMetadata(null));




        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Click -= AssociatedObject_Click;
        }
    }
}
