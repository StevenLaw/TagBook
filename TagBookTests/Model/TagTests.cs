using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TagModel.Model.Tests
{
    [TestClass()]
    public class TagTests
    {
        [TestMethod()]
        public void TagTestConstructWithValueCategory()
        {
            var tag = new Tag("Value", "Category");

            Assert.AreEqual("Value", tag.Value);
            Assert.AreEqual("Category", tag.Category);
            Assert.AreEqual(new Tag("Value", "Category"), tag);
            Assert.AreEqual(new Tag("Category:Value"), tag);
        }

        [TestMethod()]
        public void TagTestConstructWithFullTag()
        {
            var tag = new Tag("Category:Value");

            Assert.AreEqual("Value", tag.Value);
            Assert.AreEqual("Category", tag.Category);
            Assert.AreEqual(new Tag("Value", "Category"), tag);
            Assert.AreEqual(new Tag("Category:Value"), tag);
        }

        [TestMethod()]
        public void TagTestConstructWithFullTagNoCategory()
        {
            var tag = new Tag("Value");

            Assert.AreEqual("Value", tag.Value);
            Assert.IsNull(null, tag.Category);
            Assert.AreEqual(new Tag("Value"), tag);
        }

        [TestMethod()]
        public void TagTestConstructWithFullTagMoreThan2Colon()
        {
            var tag = new Tag("Category:Value:Extra:Stuff");

            Assert.AreEqual("Value:Extra:Stuff", tag.Value);
            Assert.AreEqual("Category", tag.Category);
            Assert.AreEqual(new Tag("Value:Extra:Stuff", "Category"), tag);
            Assert.AreEqual(new Tag("Category:Value:Extra:Stuff"), tag);
        }
    }
}