using System.ComponentModel.DataAnnotations.Schema;

namespace YsmStore.API.Models
{
    public class Option
    {
        public string ProductTitle { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public string[] Variants { get; set; }
    }
}
