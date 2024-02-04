namespace YsmStore.API.Dto
{
    public class ProductByCategoryRequestDto
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public string? Category { get; set; }
        public string? SortVariant { get; set; }
    }
}
