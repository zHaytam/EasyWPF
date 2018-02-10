using System.Windows;

namespace EasyWPF.Helpers
{
    public class VisibilityHelper : DependencyObject
    {

        #region Properties

        public static readonly DependencyProperty VisibleIfProperty = DependencyProperty.RegisterAttached(
            "VisibleIf",
            typeof(object),
            typeof(VisibilityHelper),
            new PropertyMetadata("Unset", VisibleIf_PropertyChanged));

        public static readonly DependencyProperty VisibleIfOptionProperty = DependencyProperty.RegisterAttached(
            "VisibleIfOption",
            typeof(VisibleIfOption),
            typeof(VisibilityHelper));

        public static readonly DependencyProperty VisibleIfCollapseProperty = DependencyProperty.RegisterAttached(
            "VisibleIfCollapse",
            typeof(bool),
            typeof(VisibilityHelper));

        #endregion

        #region Getters / Setters

        public static void SetVisibleIf(UIElement element, object value)
        {
            element.SetValue(VisibleIfProperty, value);
        }

        public static object GetVisibleIf(UIElement element)
        {
            return element.GetValue(VisibleIfProperty);
        }

        public static void SetVisibleIfOption(UIElement element, VisibleIfOption value)
        {
            element.SetValue(VisibleIfProperty, value);
        }

        public static VisibleIfOption GetVisibleIfOption(UIElement element)
        {
            return (VisibleIfOption)element.GetValue(VisibleIfProperty);
        }

        public static void SetVisibleIfCollapse(UIElement element, bool value)
        {
            element.SetValue(VisibleIfProperty, value);
        }

        public static bool GetVisibleIfCollapse(UIElement element)
        {
            return (bool)element.GetValue(VisibleIfProperty);
        }

        #endregion

        #region Private methods

        private static void VisibleIf_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            var option = (VisibleIfOption)d.GetValue(VisibleIfOptionProperty);
            VisibleIfOptionsHandlers.HandleVisibleIf(option, element, e.OldValue, e.NewValue);
        }

        #endregion

    }

    public enum VisibleIfOption
    {
        AlwaysVisible,
        HasItems,
        IsNull,
        IsNotNull,
        IsGreaterThanZero,
        IsLessThanZero,
        IsEqualToZero,
        IsDifferentThanZero
    }
}
