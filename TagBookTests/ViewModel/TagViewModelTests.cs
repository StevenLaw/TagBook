using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using TagModel.Model;

namespace TagModel.ViewModel.Tests
{
    [TestClass()]
    public class TagViewModelTests
    {
        private const string filename = "test.db";
        private TagViewModel vm;
        private const string EntryName = "Test";
        private const string Description = "This is a test";
        private const string TagValue = "Test Tag";

        [TestInitialize]
        public void InitTests()
        {
            if (File.Exists(filename))
                File.Delete(filename);
            vm = new TagViewModel();
            vm.Filename = filename;
        }

        [TestMethod()]
        public void LoadFileTest()
        {
            ErrorEncounteredErrorEventArgs errorArgs = null;
            vm.ErrorEncountered += (object sender, ErrorEncounteredErrorEventArgs e) =>
            {
                errorArgs = e;
            };
            vm.LoadFile();

            Assert.IsNull(errorArgs);
        }

        [TestMethod()]
        public void LoadFileTestFilenameNotSet()
        {
            vm.Filename = null;
            ErrorEncounteredErrorEventArgs errorArgs = null;
            vm.ErrorEncountered += (object sender, ErrorEncounteredErrorEventArgs e) =>
            {
                errorArgs = e;
            };
            vm.LoadFile();

            Assert.IsNotNull(errorArgs);
            Assert.AreEqual(ErrorType.FilenameNotSet, errorArgs.Type);
        }

        [TestMethod()]
        public void InsertEntryTest()
        {
            var entry = new LinkEntry(EntryName, Description);
            int? id = vm.InsertEntry(entry);

            Assert.IsNotNull(id);
            Assert.AreEqual(1, id);
            Assert.AreEqual(1, vm.Entries.Count());
            Assert.AreEqual(id, vm.Entries.First().Id);
            Assert.AreEqual(EntryName, vm.Entries.First().Name);
            Assert.AreEqual(Description, (vm.Entries.First() as LinkEntry).Description);
        }

        [TestMethod()]
        public void InsertEntryTestExisting()
        {
            var entry = new LinkEntry(EntryName, Description);
            entry.Id = vm.InsertEntry(entry) ?? -1;
            var id = vm.InsertEntry(entry);
            
            Assert.IsNull(id);
        }

        [TestMethod()]
        public void UpdateEntryTest()
        {
            var entry = new LinkEntry(EntryName, Description);
            entry.Id = vm.InsertEntry(entry) ?? -1;
            entry.Name += " 1";
            bool success = vm.UpdateEntry(entry);

            Assert.IsTrue(success);
            Assert.AreEqual($"{EntryName} 1", vm.Entries.First().Name);
        }

        [TestMethod()]
        public void UpdateEntryTestNotFound()
        {
            var entry = new LinkEntry(EntryName, Description);
            vm.InsertEntry(entry);
            entry.Id = int.MaxValue;
            entry.Name += " 1";
            bool success = vm.UpdateEntry(entry);

            Assert.IsFalse(success);
        }

        [TestMethod()]
        public void DeleteEntryTest()
        {
            var entry = new LinkEntry(EntryName, Description);
            var id = vm.InsertEntry(entry);

            Assert.AreEqual(1, vm.Entries.Count());

            bool success = vm.DeleteEntry(id.GetValueOrDefault());

            Assert.IsTrue(success);
            Assert.AreEqual(0, vm.Entries.Count());
        }

        [TestMethod()]
        public void GetTagsTestAllDifferent()
        {
            for (int i = 1; i <= 5; i++)
            {
                var entry = new LinkEntry($"{EntryName} {i}", Description);
                for (int j = 1; j <= 5; j++)
                {
                    entry.AddTag($"{TagValue} {i}:{j}");
                }    
                vm.InsertEntry(entry);
            }
            var tags = vm.GetTags();

            Assert.AreEqual(5 * 5, tags.Count());
        }

        [TestMethod()]
        public void GetTagsTestRepeats()
        {
            for (int i = 1; i <= 5; i++)
            {
                var entry = new LinkEntry($"{EntryName} {i}", Description);
                for (int j = 1; j <= 5; j++)
                {
                    entry.AddTag($"{TagValue} {j}");
                }    
                vm.InsertEntry(entry);
            }
            var tags = vm.GetTags();

            Assert.AreEqual(5, tags.Count());
        }

        [TestMethod()]
        public void ReloadTagsTest()
        {
            for (int i = 1; i <= 5; i++)
            {
                var entry = new LinkEntry($"{EntryName} {i}", Description);
                for (int j = 1; j <= 5; j++)
                {
                    entry.AddTag($"{TagValue} {j}");
                }
                vm.InsertEntry(entry);
            }
            vm.ReloadTags();

            Assert.AreEqual(5, vm.Tags.Count());
        }
    }
}