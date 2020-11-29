using System;
using System.Collections.Generic;

namespace TagModel.Model
{
    /// <summary>
    /// Abstract entry class.
    /// </summary>
    /// <remarks>
    /// The unit tests are conducted in the <see cref="LinkEntry"/> unit test class
    /// </remarks>
    public abstract class Entry
    {
        public int Id { get; set; }
        public HashSet<Tag> Tags { get; set; } = new HashSet<Tag>();
        public string Name { get; set; }

        public bool AddTag(Tag tag) => Tags.Add(tag);

        public Tag AddTag(string tagString)
        {
            Tag tag = new Tag(tagString);
            if (AddTag(tag))
                return tag;
            else return null;
        }

        public Tag AddTag(string value, string category)
        {
            Tag tag = new Tag(value, category);
            if (AddTag(tag))
                return tag;
            else return null;
        }

        public int AddTags(IEnumerable<Tag> tags)
        {
            int count = 0;
            foreach (Tag tag in tags)
            {
                if (Tags.Add(tag))
                    count++;
            }
            return count;
        }

        public bool RemoveTag(Tag tag) => Tags.Remove(tag);

        public int RemoveTagsWhere(Predicate<Tag> where) => Tags.RemoveWhere(where);
    }
}
