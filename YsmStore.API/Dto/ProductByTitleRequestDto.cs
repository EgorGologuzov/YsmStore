namespace YsmStore.API.Dto
{
    public class ProductByTitleRequestDto
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public string Title { get; set; }
    }
}
