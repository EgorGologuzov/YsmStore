using System;

namespace YsmStore.Models
{
    public class Order : YsmStoreModel
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime OrderDate { get; set; }

        private string _city;
        public string City
        {
            get => _city;
            set { _city = value; InvokePropertyChanged(nameof(City)); }
        }

        private string _pickUpAdress;
        public string PickUpAdress
        {
            get => _pickUpAdress;
            set { _pickUpAdress = value; InvokePropertyChanged(nameof(PickUpAdress)); }
        }

        private string _pnoneNumber;
        public string PhoneNumber
        {
            get => _pnoneNumber;
            set { _pnoneNumber = value; InvokePropertyChanged(nameof(PhoneNumber)); }
        }

        private DateTime _deliveryDate;
        public DateTime DeliveryDate
        {
            get => _deliveryDate;
            set { _deliveryDate = value; InvokePropertyChanged(nameof(DeliveryDate)); }
        }

        private OrderStatus _status;
        public OrderStatus Status
        {
            get => _status;
            set { _status = value; InvokePropertyChanged(nameof(Status)); }
        }
    }
}
