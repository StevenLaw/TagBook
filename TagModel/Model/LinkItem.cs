using System.Collections.Generic;

namespace TagModel.Model
{
    public class LinkItem
    {
        public string Name { get; set; }
        public string URL { get; set; }

        public LinkItem() { }

        public LinkItem(string name, string link)
        {
            Name = name;
            URL = link;
        }

        public override bool Equals(object obj)
        {
            return obj is LinkItem item &&
                   Name == item.Name &&
                   URL == item.URL;
        }

        public override int GetHashCode()
        {
            int hashCode = -1372958849;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(URL);
            return hashCode;
        }

        public override string ToString()
        {
            return $"{Name}: {URL}";
        }
    }
}
