using System.ComponentModel.DataAnnotations.Schema;

namespace YsmStore.API.Models
{
    public class Order
    {
        public int Id { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        [NotMapped]
        public string CustomerEmail { get => Customer is null ? null : Customer.Login; }

        public DateTime OrderDate { get; set; }
        public string PickUpAdress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderedProduct> Products { get; set; }
    }
}
