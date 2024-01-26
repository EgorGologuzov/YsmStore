namespace YsmStore.API.Dto
{
    public class CustomerOrdersRequestDto
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
