using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YsmStore.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegularEntry : ContentView
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

        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create(nameof(Text), typeof(Keyboard), typeof(RegularEntry), null, BindingMode.TwoWay);
        public Keyboard Keyboard
        {
            get => (Keyboard)GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
        }

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public RegularEntry()
        {
            InitializeComponent();

            entry.SetBinding(Entry.PlaceholderProperty, new Binding(nameof(Placeholder), source: this, mode: BindingMode.TwoWay));
            entry.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this, mode: BindingMode.TwoWay));
            entry.SetBinding(Entry.KeyboardProperty, new Binding(nameof(Keyboard), source: this, mode: BindingMode.TwoWay));
            frame.SetBinding(Frame.BackgroundColorProperty, new Binding(nameof(BackgroundColor), source: this, mode: BindingMode.TwoWay));

            entry.TextChanged += (s, e) => TextChanged?.Invoke(this, e);
        }
    }
}