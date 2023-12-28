using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YsmStore.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordEntry : ContentView
    {
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(RegularEntry), null, BindingMode.TwoWay);
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(RegularEntry), null, BindingMode.TwoWay);
        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(RegularEntry), null, BindingMode.TwoWay);
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public PasswordEntry()
        {
            InitializeComponent();

            entry.SetBinding(Entry.PlaceholderProperty, new Binding(nameof(Placeholder), source: this, mode: BindingMode.TwoWay));
            entry.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this, mode: BindingMode.TwoWay));
            frame.SetBinding(Frame.BackgroundColorProperty, new Binding(nameof(BackgroundColor), source: this, mode: BindingMode.TwoWay));
        }

        private void Image_Tapped(object sender, EventArgs e)
        {
            entry.IsPassword = !entry.IsPassword;
        }
    }
}