namespace YsmStore.API.Dto
{
    public class ProductByTitleAndOptionsDto
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public string Title { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
    }
}
