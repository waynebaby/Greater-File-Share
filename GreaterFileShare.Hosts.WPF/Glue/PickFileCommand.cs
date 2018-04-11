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

namespace GreaterFileShare.Hosts.WPF.Glue
{
    public class PickFileCommand : DependencyObject, ICommand
    {




        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            FrameworkElement e = parameter as FrameworkElement;

            while (e.Parent != null)
            {
                e = e.Parent as FrameworkElement;
            }
            var w = e as Window;


            using (CommonFileDialog dialog =
                        (DialogType == DialogType.Open ?
                            new CommonOpenFileDialog()
                            {
                                IsFolderPicker = false,

                            } as CommonFileDialog
                        :
                            new CommonSaveFileDialog()
                            {

                            } as CommonFileDialog))
            {
                dialog.Filters.Add(new CommonFileDialogFilter(FileExtension, FileExtension));
                dialog.Title = Title;
                var f = dialog.ShowDialog(w);
                if (f == CommonFileDialogResult.Ok)
                {
                    File = dialog.FileName;
                }
                else if (IsClearValueAfterFailNeeded)
                {
                    File = null;
                }

            }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PickFileCommand), new PropertyMetadata(""));



        public string FileExtension
        {
            get { return (string)GetValue(FileExtensionProperty); }
            set { SetValue(FileExtensionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileExtension.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileExtensionProperty =
            DependencyProperty.Register("FileExtension", typeof(string), typeof(PickFileCommand), new PropertyMetadata(Consts.SettingExtension));



        public bool IsClearValueAfterFailNeeded
        {
            get { return (bool)GetValue(IsClearValueAfterFailNeededProperty); }
            set { SetValue(IsClearValueAfterFailNeededProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsClearValueAfterFailNeeded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsClearValueAfterFailNeededProperty =
            DependencyProperty.Register("IsClearValueAfterFailNeeded", typeof(bool), typeof(PickFileCommand), new PropertyMetadata(true));




        public string File
        {
            get { return (string)GetValue(FileProperty); }
            set { SetValue(FileProperty, value); }
        }
        // Using a DependencyProperty as the backing store for File.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileProperty =
            DependencyProperty.Register("File", typeof(string), typeof(PickFileCommand), new PropertyMetadata(null));





        public DialogType DialogType
        {
            get { return (DialogType)GetValue(DialogTypeProperty); }
            set { SetValue(DialogTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DialogType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DialogTypeProperty =
            DependencyProperty.Register("DialogType", typeof(DialogType), typeof(PickFileCommand), new PropertyMetadata(DialogType.Open));

        public event EventHandler CanExecuteChanged;
    }
    public enum DialogType
    {
        Open,
        Save
    }
}
