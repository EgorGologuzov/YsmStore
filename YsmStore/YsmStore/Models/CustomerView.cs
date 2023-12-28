using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace YsmStore.Models
{
    public class CustomerView : YsmStoreView<Customer>
    {
        private const string DEFAULT_EMAIL_SUBJECT = "Администрация YsmStore";

        public CustomerView(Customer customer) : base(customer)
        {
            WriteEmail = new Command(WriteEmail_Execute, WriteEmail_CanExecute);
            Model.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Model.FullName))
                    InvokePropertyChanged(nameof(Greetings));
            };
        }

        public string Greetings
        {
            get
            {
                TimeSpan time = DateTime.Now.TimeOfDay;
                if (time <= new TimeSpan(6, 0, 0)) return "Доброй ночи, " + Model.FullName;
                if (time <= new TimeSpan(12, 0, 0)) return "Доброе утро, " + Model.FullName;
                if (time <= new TimeSpan(18, 0, 0)) return "Добрый день, " + Model.FullName;
                return "Добрый вечер, " + Model.FullName;
            }
        }

        private string _emailSubject;
        public string EmailSubject
        {
            get => _emailSubject ?? DEFAULT_EMAIL_SUBJECT;
            set { _emailSubject = value; InvokePropertyChanged(nameof(EmailSubject)); }
        }

        public ICommand WriteEmail { get; private set; }

        private void WriteEmail_Execute()
        {
            try
            {
                Email.ComposeAsync(EmailSubject, string.Empty, Model.Login);
            }
            catch { }
        }

        private bool WriteEmail_CanExecute() => true;
    }
}
