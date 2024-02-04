using System;

namespace YsmStore.Models
{
    public class CustomerOrdersQuery : Query
    {
        public Guid CustomerId { get; set; }
    }
}
