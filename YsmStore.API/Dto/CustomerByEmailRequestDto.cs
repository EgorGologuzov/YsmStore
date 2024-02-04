namespace YsmStore.API.Dto
{
    public class CustomerByEmailRequestDto
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public string? Email { get; set; }
    }
}
