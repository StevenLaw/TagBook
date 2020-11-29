using System;
using System.Collections.Generic;
using System.Text;

namespace TagModel.Model
{
    public class LinkEntry : Entry
    {
        public string Description { get; set; }
        public List<LinkItem> Links { get; set; } = new List<LinkItem>();

        public LinkEntry(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void AddLink(LinkItem link) => Links.Add(link);

        public LinkItem AddLink(string name, string url)
        {
            LinkItem newLink = new LinkItem(name, url);
            Links.Add(newLink);
            return newLink;
        }

        public void AddLinks(IEnumerable<LinkItem> links) => Links.AddRange(links);

        public bool RemoveLink(LinkItem link) => Links.Remove(link);

        public void RemoveLinkAt(int index) => Links.RemoveAt(index);

        public void RemoveAllLinks(Predicate<LinkItem> match) => Links.RemoveAll(match);

        public void RemoveLinksRange(int index, int count) => Links.RemoveRange(index, count);

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Name: {Name}");
            sb.AppendLine($"Description: {Description}");
            sb.AppendLine("Links:");
            foreach(var link in Links)
            {
                sb.Append("\t");
                sb.AppendLine(link.ToString());
            }
            sb.AppendLine("Tags:");
            foreach(var tag in Tags)
            {
                sb.Append("\t");
                sb.AppendLine(tag.ToString());
            }
            return sb.ToString();
        }
    }
}
