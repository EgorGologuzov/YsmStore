using System.ComponentModel.DataAnnotations.Schema;

namespace YsmStore.API.Dto
{
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Avatar { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public Dictionary<string, string>? Properties { get; set; }
    }
}
