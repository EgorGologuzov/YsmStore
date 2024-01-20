using System.ComponentModel.DataAnnotations;

namespace YsmStore.API.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string? RecoveryPassword { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<ProductInCart> Cart { get; set; }
    }
}
