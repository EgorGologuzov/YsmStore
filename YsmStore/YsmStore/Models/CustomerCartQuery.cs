using System;

namespace YsmStore.Models
{
    public class CustomerCartQuery : Query
    {
        public Guid CustomerId { get; set; }
    }
}
