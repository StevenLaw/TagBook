using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TagModel.Model.Tests
{
    [TestClass()]
    public class LinkEntryTests
    {
        private const string EntryName = "Test";
        private const string DescriptionName = "This is a test";
        private const string LinkName = "Test Link";
        private const string LinkUrl = "www.test.com";
        private const string TagValue = "Test Tag";
        private const string TagCategory = "Tag Cat";

        [TestMethod()]
        public void LinkEntryTest()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);

            Assert.AreEqual(EntryName, entry.Name);
            Assert.AreEqual(DescriptionName, entry.Description);
        }

        [TestMethod()]
        public void AddLinkTest()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var link = new LinkItem(LinkName, LinkUrl);

            entry.AddLink(link);

            Assert.AreEqual(link, entry.Links.First());
            Assert.AreEqual(LinkName, entry.Links.First().Name);
            Assert.AreEqual(LinkUrl, entry.Links.First().URL);
        }

        [TestMethod()]
        public void AddLinkTestWithNameAndUrl()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);

            var link = entry.AddLink(LinkName, LinkUrl);

            Assert.AreEqual(link, entry.Links.First());
            Assert.AreEqual(LinkName, entry.Links.First().Name);
            Assert.AreEqual(LinkUrl, entry.Links.First().URL);
        }

        [TestMethod()]
        public void AddLinksTest()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var links = new List<LinkItem>();
            for (int i = 1; i <= 5; i++)
            {
                links.Add(new LinkItem($"{LinkName} {i}", $"{LinkUrl}/{i}"));
            }

            entry.AddLinks(links);

            Assert.AreEqual(5, entry.Links.Count());
            Assert.IsTrue(entry.Links.All(l => l.Name.StartsWith(LinkName) && l.URL.StartsWith(LinkUrl)),
                $"All of the links' names should start with the {LinkName} and the URLs should start with {LinkUrl}");
        }

        [TestMethod()]
        public void RemoveLinkTest()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var links = new List<LinkItem>();
            for (int i = 1; i <= 5; i++)
            {
                links.Add(new LinkItem($"{LinkName} {i}", $"{LinkUrl}/{i}"));
            }
            entry.AddLinks(links);

            bool success = entry.RemoveLink(links[2]);

            Assert.AreEqual(4, entry.Links.Count());
            Assert.IsTrue(!entry.Links.Any(l => l.Name.EndsWith("3") || l.URL.EndsWith("3")),
                $"None of the links' names or URLs should end with 3 as that should have been deleted");
            Assert.IsTrue(success);
        }

        [TestMethod()]
        public void RemoveLinkTestFailure()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var links = new List<LinkItem>();
            for (int i = 1; i <= 5; i++)
            {
                links.Add(new LinkItem($"{LinkName} {i}", $"{LinkUrl}/{i}"));
            }
            entry.AddLinks(links);

            bool success = entry.RemoveLink(new LinkItem("Wrong", "www.LinkItem.com"));

            Assert.AreEqual(5, entry.Links.Count());
            Assert.IsFalse(success);
        }

        [TestMethod()]
        public void RemoveLinkAtTest()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var links = new List<LinkItem>();
            for (int i = 1; i <= 5; i++)
            {
                links.Add(new LinkItem($"{LinkName} {i}", $"{LinkUrl}/{i}"));
            }
            entry.AddLinks(links);

            entry.RemoveLinkAt(2);

            Assert.AreEqual(4, entry.Links.Count());
            Assert.IsTrue(!entry.Links.Any(l => l.Name.EndsWith("3") || l.URL.EndsWith("3")),
                $"None of the links' names or URLs should end with 3 as that should have been deleted");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveLinkAtTestOutOfRange()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var links = new List<LinkItem>();
            for (int i = 1; i <= 5; i++)
            {
                links.Add(new LinkItem($"{LinkName} {i}", $"{LinkUrl}/{i}"));
            }
            entry.AddLinks(links);

            entry.RemoveLinkAt(5);
        }

        [TestMethod()]
        public void RemoveAllLinksTest()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var links = new List<LinkItem>();
            for (int i = 1; i <= 5; i++)
            {
                links.Add(new LinkItem($"{LinkName} {i}", $"{LinkUrl}/{i}"));
            }
            entry.AddLinks(links);

            entry.RemoveAllLinks(x => int.Parse(x.Name.Last().ToString()) % 2 == 0);

            Assert.AreEqual(3, entry.Links.Count());
            Assert.IsTrue(entry.Links.All(x => int.Parse(x.Name.Last().ToString()) % 2 == 1));
        }

        [TestMethod()]
        public void RemoveLinksRangeTest()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var links = new List<LinkItem>();
            for (int i = 1; i <= 5; i++)
            {
                links.Add(new LinkItem($"{LinkName} {i}", $"{LinkUrl}/{i}"));
            }
            entry.AddLinks(links);

            entry.RemoveLinksRange(1, 3);

            Assert.AreEqual(2, entry.Links.Count());
            Assert.AreEqual($"{LinkName} 1", entry.Links.First().Name);
            Assert.AreEqual($"{LinkName} 5", entry.Links.Last().Name);
        }

        /**
         * The tests for the Entry abstract class begin here.
         */

        [TestMethod()]
        public void AddTagTest()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var tag = new Tag(TagValue, TagCategory);

            bool value = entry.AddTag(tag);

            Assert.IsTrue(value);
            Assert.AreEqual(tag, entry.Tags.First());
            Assert.AreEqual(TagValue, entry.Tags.First().Value);
            Assert.AreEqual(TagCategory, entry.Tags.First().Category);
        }

        [TestMethod()]
        public void AddTagTestExisting()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var tag = new Tag(TagValue, TagCategory);

            entry.AddTag(tag);
            bool value = entry.AddTag(tag);

            Assert.IsFalse(value);
            Assert.AreEqual(1, entry.Tags.Count());
        }

        [TestMethod()]
        public void AddTagTestWithFullString()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);

            Tag tag = entry.AddTag($"{TagCategory}:{TagValue}");

            Assert.IsNotNull(tag);
            Assert.AreEqual(tag, entry.Tags.First());
            Assert.AreEqual(TagValue, entry.Tags.First().Value);
            Assert.AreEqual(TagCategory, entry.Tags.First().Category);
        }

        [TestMethod()]
        public void AddTagTestWithFullStringExisting()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);

            entry.AddTag($"{TagCategory}:{TagValue}");
            Tag tag = entry.AddTag($"{TagCategory}:{TagValue}");

            Assert.IsNull(tag);
            Assert.AreEqual(1, entry.Tags.Count());
        }

        [TestMethod()]
        public void AddTagTestWithFullStringNoCategory()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);

            Tag tag = entry.AddTag(TagValue);

            Assert.IsNotNull(tag);
            Assert.AreEqual(tag, entry.Tags.First());
            Assert.AreEqual(TagValue, entry.Tags.First().Value);
        }

        [TestMethod()]
        public void AddTagTestWithFullStringNoCategoryExisting()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            
            entry.AddTag(TagValue);
            Tag tag = entry.AddTag(TagValue);

            Assert.IsNull(tag);
            Assert.AreEqual(1, entry.Tags.Count());
        }

        [TestMethod()]
        public void AddTagTestWithValueAndCategory()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);

            Tag tag = entry.AddTag(TagValue, TagCategory);

            Assert.IsNotNull(tag);
            Assert.AreEqual(tag, entry.Tags.First());
            Assert.AreEqual(TagValue, entry.Tags.First().Value);
            Assert.AreEqual(TagCategory, entry.Tags.First().Category);
        }

        [TestMethod()]
        public void AddTagTestWithValueAndCategoryExisting()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);

            entry.AddTag(TagValue, TagCategory);
            Tag tag = entry.AddTag(TagValue, TagCategory);

            Assert.IsNull(tag);
            Assert.AreEqual(1, entry.Tags.Count());
        }

        [TestMethod()]
        public void AddTagsTest()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var list = new List<Tag>();
            for (int i = 1; i <= 5; i++)
            {
                list.Add(new Tag($"{TagValue} {i}", $"{TagCategory} {i}"));
            }

            int count = entry.AddTags(list);

            Assert.AreEqual(5, count);
            Assert.AreEqual(5, entry.Tags.Count());
        }

        [TestMethod()]
        public void AddTagsTestSomeExisting()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var list = new List<Tag>();
            for (int i = 1; i <= 5; i++)
            {
                list.Add(new Tag($"{TagValue} {i}", $"{TagCategory} {i}"));
            }
            list.Add(new Tag($"{TagValue} 2", $"{TagCategory} 2"));
            list.Add(new Tag($"{TagValue} 4", $"{TagCategory} 4"));

            int count = entry.AddTags(list);

            Assert.AreEqual(7, list.Count);
            Assert.AreEqual(5, count);
            Assert.AreEqual(5, entry.Tags.Count());
        }

        [TestMethod()]
        public void RemoveTagTest()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var list = new List<Tag>();
            for (int i = 1; i <= 5; i++)
            {
                list.Add(new Tag($"{TagValue} {i}", $"{TagCategory} {i}"));
            }
            entry.AddTags(list);

            var result = entry.RemoveTag(new Tag($"{TagValue} 2", $"{TagCategory} 2"));

            Assert.IsTrue(result);
            Assert.AreEqual(4, entry.Tags.Count());
            Assert.IsFalse(entry.Tags.Any(t => t.Value.EndsWith("2")));
        }

        [TestMethod()]
        public void RemoveTagTestFailure()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var list = new List<Tag>();
            for (int i = 1; i <= 5; i++)
            {
                list.Add(new Tag($"{TagValue} {i}", $"{TagCategory} {i}"));
            }
            entry.AddTags(list);

            var result = entry.RemoveTag(new Tag($"{TagValue} 6", $"{TagCategory} 6"));

            Assert.IsFalse(result);
            Assert.AreEqual(5, entry.Tags.Count());
        }

        [TestMethod()]
        public void RemoveTagsWhereTest()
        {
            var entry = new LinkEntry(EntryName, DescriptionName);
            var list = new List<Tag>();
            for (int i = 1; i <= 5; i++)
            {
                list.Add(new Tag($"{TagValue} {i}", $"{TagCategory} {i}"));
            }
            entry.AddTags(list);

            var count = entry.RemoveTagsWhere(x => int.Parse(x.Value.Last().ToString()) % 2 == 0);

            Assert.AreEqual(2, count);
            Assert.AreEqual(3, entry.Tags.Count());
        }

        [TestMethod()]
        public void ToStringTest()
        {
            var linkEntry = new LinkEntry("Test", "This is a test");
            linkEntry.AddLink("Test", "www.test.com");
            linkEntry.AddTag("Test");
            string expected = "Name: Test\n" +
                "Description: This is a test\n" +
                "Links:\n" +
                "\tTest: www.test.com\n" +
                "Tags:\n" +
                "\tTest\n";

            string output = linkEntry.ToString();
            // Remove carriage returns to ignore newline variance
            output = output.Replace("\r", "");

            Assert.AreEqual(expected, output);
        }
    }
}