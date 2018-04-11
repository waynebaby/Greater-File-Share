using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Common;
using GreaterFileShare.Hosts.WPF.ViewModels;

namespace GreaterFileShare.Hosts.WPF.Glue
{
    public class PickFolderCommand : FrameworkElement, ICommand
    {


        //protected override void OnAttached()
        //{
        //    base.OnAttached();


        //    AssociatedObject.Click += AssociatedObject_Click;
        //}






        public bool IsClearValueAfterFailNeeded
        {
            get { return (bool)GetValue(IsClearValueAfterFailNeededProperty); }
            set { SetValue(IsClearValueAfterFailNeededProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsClearValueAfterFailNeeded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsClearValueAfterFailNeededProperty =
            DependencyProperty.Register("IsClearValueAfterFailNeeded", typeof(bool), typeof(PickFolderCommand), new PropertyMetadata(false));



        public string Folder
        {
            get { return (string)GetValue(FolderProperty); }
            set { SetValue(FolderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Folder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FolderProperty =
            DependencyProperty.Register("Folder", typeof(string), typeof(PickFolderCommand), new PropertyMetadata(""));

        public event EventHandler CanExecuteChanged;

        //protected override void OnDetaching()
        //{
        //    base.OnDetaching();
        //    AssociatedObject.Click -= AssociatedObject_Click;
        //}

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Window w;
            FrameworkElement e = parameter as FrameworkElement;

            while (e.Parent != null)
            {
                e = e.Parent as FrameworkElement;
            }
            w = e as Window;


            var vm = w.GetViewDisguise().ViewModel as MainWindow_Model;
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
                else if (IsClearValueAfterFailNeeded)
                {
                    Folder = null;
                }
            }

            if (vm?.CurrentTask != null)
            {
                vm.CurrentTask.Path = Folder;
            }
        }
    }
}
