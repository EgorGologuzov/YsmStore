using System.Drawing;

namespace YsmStore.Models
{
    public class OptionVariant
    {
        public string Text { get; private set; }
        public Color Color { get; private set; }

        public bool IsNotAvailable { get; set; }
        public bool IsSelected { get; set; }

        public bool IsColor { get; private set; }
        public bool IsText { get; private set; }

        public OptionVariant(string text, bool available, bool selected)
        {
            Text = text;
            IsNotAvailable = !available;
            IsSelected = selected;

            Color color = Color.FromName(text);
            if (color.ToArgb() == 0)
            {
                IsText = true;
                IsColor = false;
            }
            else
            {
                IsText = false;
                IsColor = true;
                Color = color;
            }
        }
    }
}
