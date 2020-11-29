using System;
using TagModel.Model;

namespace TagModel.ViewModel
{
    public class AddEditEntryEventArgs : EventArgs
    {
        public Type EntryType { get; set; }
        public Entry Entry { get; set; }
        public AddEditEntryEventArgs(Type entryType)
        {
            if (entryType.IsSubclassOf(typeof(Entry)))
            {
                EntryType = entryType;
            }
            else
            {
                throw new ArgumentException($"'{entryType.FullName}' does not inherit from '{typeof(Entry).FullName}'");
            }
        }
        public AddEditEntryEventArgs(Entry entry)
        {
            EntryType = entry.GetType();
            Entry = entry;
        }
    }
}
