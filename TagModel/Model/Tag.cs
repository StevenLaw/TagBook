using System.Collections.Generic;
using System.Linq;

namespace TagModel.Model
{
    public class Tag
    {
        public string Value { get; set; }
        public string Category { get; set; }
        public string FullTag { get => string.IsNullOrWhiteSpace(Category) ? Value : $"{Category}:{Value}"; }

        public Tag(string value, string category)
        {
            Value = value;
            Category = category;
        }

        public Tag(string fullTag)
        {
            var split = fullTag.Split(':');
            if (split.Length >= 2)
            {
                Category = split[0];
                Value = string.Join(":", split.Skip(1));
            }
            else
            {
                Value = fullTag;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Tag tag &&
                   Value == tag.Value &&
                   Category == tag.Category &&
                   FullTag == tag.FullTag;
        }

        public override int GetHashCode()
        {
            int hashCode = 1009568215;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Category);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FullTag);
            return hashCode;
        }

        public override string ToString() => FullTag;
    }
}
