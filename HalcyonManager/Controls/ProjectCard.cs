using System.Windows.Input;

namespace HalcyonManager.Controls
{
    public class ProjectCard : ContentView
    {


        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ProjectCard), Color.FromRgb(135, 135, 135));
        public static readonly BindableProperty CardColorProperty = BindableProperty.Create(nameof(CardColor), typeof(Color), typeof(ProjectCard), Color.FromRgb(255, 255, 255));
        public static readonly BindableProperty IconBackgroundColorProperty = BindableProperty.Create(nameof(IconBackgroundColor), typeof(Color), typeof(ProjectCard), Color.FromRgb(217, 217, 217));


        public static readonly BindableProperty EditCommandProperty = BindableProperty.Create(nameof(EditCommand), typeof(ICommand), typeof(ProjectCard), null);
        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

        public static readonly BindableProperty EditCommandParameterProperty = BindableProperty.Create(nameof(EditCommandParameter), typeof(object), typeof(ProjectCard), null);
        public object EditCommandParameter
        {
            get { return GetValue(EditCommandParameterProperty); }
            set { SetValue(EditCommandParameterProperty, value); }
        }

        public static readonly BindableProperty WorkTaskTitleProperty = BindableProperty.Create(nameof(WorkTaskTitle), typeof(string), typeof(ProjectCard), string.Empty);
        public string WorkTaskTitle
        {
            get => (string)GetValue(WorkTaskTitleProperty);
            set => SetValue(WorkTaskTitleProperty, value);
        }

        public static readonly BindableProperty WorkTaskDescriptionProperty = BindableProperty.Create(nameof(WorkTaskDescription), typeof(string), typeof(ProjectCard), string.Empty);
        public string WorkTaskDescription
        {
            get => (string)GetValue(WorkTaskDescriptionProperty);
            set => SetValue(WorkTaskDescriptionProperty, value);
        }

        public static readonly BindableProperty WorkTaskStartDateProperty = BindableProperty.Create(nameof(WorkTaskStartDate), typeof(string), typeof(ProjectCard), string.Empty);
        public string WorkTaskStartDate
        {
            get => (string)GetValue(WorkTaskStartDateProperty);
            set => SetValue(WorkTaskStartDateProperty, value);
        }

        public static readonly BindableProperty WorkTaskTargetDateProperty = BindableProperty.Create(nameof(WorkTaskTargetDate), typeof(string), typeof(ProjectCard), string.Empty);
        public string WorkTaskTargetDate
        {
            get => (string)GetValue(WorkTaskTargetDateProperty);
            set => SetValue(WorkTaskTargetDateProperty, value);
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
