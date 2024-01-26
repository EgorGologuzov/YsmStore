using System.ComponentModel.DataAnnotations.Schema;
using YsmStore.API.Models;

namespace YsmStore.API.Dto
{
    public class OrderReturnDto
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public string PickUpAdress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderedProductDto> Products { get; set; }
    }
}
