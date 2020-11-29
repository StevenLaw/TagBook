using System.Collections.ObjectModel;
using System.Linq;
using TagModel.Model;

namespace TagModel.ViewModel
{
    public class AddEditLinkEntryViewModel : PropertyNotifier
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ObservableCollection<LinkItem> Links { get; set; }
        public ObservableCollection<Tag> Tags { get; set; }

        public AddEditLinkEntryViewModel(LinkEntry linkEntry)
        {

        }

        public LinkEntry GetLinkEntry()
        {
            return new LinkEntry(Name, Description)
            {
                Links = Links.ToList(),
                Tags = Tags.ToHashSet()
            };
        }
    }
}
