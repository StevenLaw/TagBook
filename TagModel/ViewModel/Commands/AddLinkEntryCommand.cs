using System;
using System.Windows.Input;
using TagModel.Model;

namespace TagModel.ViewModel.Commands
{
    public class AddLinkEntryCommand : ICommand
    {
        public TagViewModel VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public AddLinkEntryCommand(TagViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(VM.Filename);
        }

        public void Execute(object parameter)
        {
            VM.AddEntry(typeof(LinkEntry));
        }
    }
}
