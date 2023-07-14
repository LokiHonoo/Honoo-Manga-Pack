using System.Windows;
using System.Windows.Input;

namespace HonooUI.WPF.Controls
{
    public class Window : System.Windows.Window
    {
        public static readonly DependencyProperty CaptionContentProperty =
            DependencyProperty.Register("CaptionContent", typeof(object), typeof(Window));

        public static readonly DependencyProperty CaptionHeightProperty =
            DependencyProperty.Register("CaptionHeight", typeof(double), typeof(Window), new FrameworkPropertyMetadata(40d));

        public static readonly DependencyProperty IconDisplayProperty =
            DependencyProperty.Register("IconDisplay", typeof(bool), typeof(Window), new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty SystemButtonsDisplayProperty =
            DependencyProperty.Register("SystemButtonsDisplay", typeof(bool), typeof(Window), new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty TitleDisplayProperty =
            DependencyProperty.Register("TitleDisplay", typeof(bool), typeof(Window), new FrameworkPropertyMetadata(true));

        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));

            CommandManager.RegisterClassCommandBinding(typeof(Window),
                new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindowCommandExecuted));
            CommandManager.RegisterClassCommandBinding(typeof(Window),
                new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindowCommandExecuted));
        }

        public object CaptionContent
        {
            get { return GetValue(CaptionContentProperty); }
            set { SetValue(CaptionContentProperty, value); }
        }

        public double CaptionHeight
        {
            get { return (double)GetValue(CaptionHeightProperty); }
            set { SetValue(CaptionHeightProperty, value); }
        }

        public bool IconDisplay
        {
            get { return (bool)GetValue(IconDisplayProperty); }
            set { SetValue(IconDisplayProperty, value); }
        }

        public bool SystemButtonsDisplay
        {
            get { return (bool)GetValue(SystemButtonsDisplayProperty); }
            set { SetValue(SystemButtonsDisplayProperty, value); }
        }

        public bool TitleDisplay
        {
            get { return (bool)GetValue(TitleDisplayProperty); }
            set { SetValue(TitleDisplayProperty, value); }
        }

        private static void CloseWindowCommandExecuted(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow((Window)e.Source);
        }

        private static void MinimizeWindowCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow((Window)e.Source);
        }
    }
}