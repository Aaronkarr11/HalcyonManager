using System.Windows.Input;

namespace HalcyonManager.Controls
{
    public class CardView : ContentView
    {
        public static readonly BindableProperty CardTitleProperty = BindableProperty.Create(nameof(CardTitle), typeof(string), typeof(CardView), string.Empty);
        public static readonly BindableProperty CardDescriptionProperty = BindableProperty.Create(nameof(CardDescription), typeof(string), typeof(CardView), string.Empty);
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CardView), Color.FromRgb(135, 135, 135));
        public static readonly BindableProperty CardColorProperty = BindableProperty.Create(nameof(CardColor), typeof(Color), typeof(CardView), Color.FromRgb(255, 255, 255));
        public static readonly BindableProperty IconBackgroundColorProperty = BindableProperty.Create(nameof(IconBackgroundColor), typeof(Color), typeof(CardView), Color.FromRgb(217, 217, 217));
        public static readonly BindableProperty FulfillProperty = BindableProperty.Create(nameof(Fulfill), typeof(bool), typeof(CardView), false);

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(CardView), null);
        public static readonly BindableProperty FullfillCommandProperty = BindableProperty.Create(nameof(FullfillCommand), typeof(ICommand), typeof(CardView), null);
        public ICommand FullfillCommand
        {
            get { return (ICommand)GetValue(FullfillCommandProperty); }
            set { SetValue(FullfillCommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public string CardTitle
        {
            get => (string)GetValue(CardTitleProperty);
            set => SetValue(CardTitleProperty, value);
        }

        public bool Fulfill
        {
            get => (bool)GetValue(FulfillProperty);
            set => SetValue(FulfillProperty, value);
        }

        public string CardDescription
        {
            get => (string)GetValue(CardDescriptionProperty);
            set => SetValue(CardDescriptionProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public Color CardColor
        {
            get => (Color)GetValue(CardColorProperty);
            set => SetValue(CardColorProperty, value);
        }

        public Color IconBackgroundColor
        {
            get => (Color)GetValue(IconBackgroundColorProperty);
            set => SetValue(IconBackgroundColorProperty, value);
        }
    }
}
