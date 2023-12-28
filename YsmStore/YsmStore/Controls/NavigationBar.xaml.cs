using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YsmStore.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationBar : ContentView
    {
        public static readonly BindableProperty BarContentProperty =
            BindableProperty.Create(nameof(BarContent), typeof(View), typeof(RegularEntry), null, BindingMode.TwoWay);
        public View BarContent
        {
            get => (View)GetValue(BarContentProperty);
            set => SetValue(BarContentProperty, value);
        }

        public static readonly BindableProperty RightButtonProperty =
            BindableProperty.Create(nameof(RightButton), typeof(ImageButton), typeof(RegularEntry), null, BindingMode.TwoWay);
        public ImageButton RightButton
        {
            get => (ImageButton)GetValue(RightButtonProperty);
            set => SetValue(RightButtonProperty, value);
        }

        public static readonly BindableProperty IsBackButtonVisibleProperty =
            BindableProperty.Create(nameof(IsBackButtonVisible), typeof(bool), typeof(RegularEntry), true, BindingMode.TwoWay);
        public bool IsBackButtonVisible
        {
            get => (bool)GetValue(IsBackButtonVisibleProperty);
            set => SetValue(IsBackButtonVisibleProperty, value);
        }

        public NavigationBar()
        {
            InitializeComponent();

            contentFrame.SetBinding(Frame.ContentProperty, new Binding(nameof(BarContent), source: this, mode: BindingMode.TwoWay));
            rightButtonFrame.SetBinding(Frame.ContentProperty, new Binding(nameof(RightButton), source: this, mode: BindingMode.TwoWay));
            backButton.SetBinding(Button.IsVisibleProperty, new Binding(nameof(IsBackButtonVisible), source: this, mode: BindingMode.TwoWay));
        }

        private void BackButton_Tapped(object sender, EventArgs e)
        {
            Navigation?.PopAsync();
        }
    }
}