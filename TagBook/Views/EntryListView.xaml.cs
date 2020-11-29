using System;
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
using TagBook.UserControls;
using TagModel.Model;

namespace TagBook.Views
{
    /// <summary>
    /// Interaction logic for EntryListView.xaml
    /// </summary>
    public partial class EntryListView : UserControl
    {
        public EntryListView()
        {
            InitializeComponent();

            var link = new LinkEntry("Test", "This is a test");
            link.AddLink("Test", "www.test.com");
            link.AddLink("Test 2", "www.test.com");
            link.AddTag("test:tag");
            link.AddTag("another tag");
            //entries.Items.Add(new LinkEntryDisplay { DataContext = link });
            var entryList = new List<Entry>();
            entryList.Add(link);
            entries.ItemsSource = entryList;
        }
    }
}
