using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YsmStore.Models
{
    public class Option
    {
        public string Name { get; set; }
        public ReadOnlyCollection<OptionVariant> Variants { get; set; }
        public bool IsSet { get; set; }

        public Option(string name, IList<OptionVariant> variants, bool isSet)
        {
            Name = name;
            Variants = new ReadOnlyCollection<OptionVariant>(variants);
            IsSet = isSet;
        }
    }
}
