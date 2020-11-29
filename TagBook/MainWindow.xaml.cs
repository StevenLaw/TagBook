using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagBook.Views;
using TagModel.Model;
using TagModel.ViewModel;

namespace TagBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TagViewModel vm;
        public MainWindow()
        {
            InitializeComponent();

            vm = DataContext as TagViewModel;

            vm.ErrorEncountered += Vm_ErrorEncountered;
            vm.AddEditEntry += Vm_AddEditEntry;
            vm.ViewMainList += Vm_ViewMainList;

            NavigateToMainListView();
        }

        private void Vm_ErrorEncountered(object sender, ErrorEncounteredErrorEventArgs e)
        {
            switch (e.Type)
            {
                case ErrorType.FilenameNotSet:
                    MessageBox.Show(this, "The filename must be set before loading the file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case ErrorType.ExceptionFound:
                    MessageBox.Show(this, e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private void Vm_AddEditEntry(object sender, AddEditEntryEventArgs e)
        {
            if (e.Entry == null)
            {

            }
            else
            {
                switch (e.Entry)
                {
                    case LinkEntry linkEntry:
                        content.Content = new AddEditLinkEntryView { DataContext = new AddEditLinkEntryView() };
                        break;
                }
            }
        }

        private void Vm_ViewMainList(object sender, EventArgs e)
        {
            NavigateToMainListView();
        }

        private void NavigateToMainListView()
        {
            content.Content = new EntryListView() { DataContext = vm };
        }

        private void ExitCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void SetFileCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Database Files (*.db)|All Files(*.*)",
                CheckFileExists = false
            };
            var result = ofd.ShowDialog(this);
            if (result == true)
            {
                vm.Filename = ofd.FileName;
            }
        }
    }
}
