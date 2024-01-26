using System.ComponentModel.DataAnnotations.Schema;
using YsmStore.API.Models;

namespace YsmStore.API.Dto
{
    public class OrderCreateDto
    {
        public string PickUpAdress { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<OrderedProductDto> Products { get; set; }
    }
}
