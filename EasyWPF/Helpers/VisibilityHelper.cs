using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;

namespace EasyWPF.Helpers
{
    public class VisibilityHelper : DependencyObject
    {

        #region Fields

        private static readonly Dictionary<INotifyCollectionChanged, FrameworkElement> ElementsOfLists = new Dictionary<INotifyCollectionChanged, FrameworkElement>();

        #endregion

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

        #endregion

        #region Private methods

        private static void VisibleIf_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            var option = (VisibleIfOption)d.GetValue(VisibleIfOptionProperty);

            switch (option)
            {
                case VisibleIfOption.AlwaysVisible:

                    if (!element.IsVisible)
                    {
                        element.Visibility = Visibility.Visible;
                    }

                    break;
                case VisibleIfOption.HasItems:

                    if (e.OldValue is INotifyCollectionChanged oldCollection)
                    {
                        oldCollection.CollectionChanged -= Element_CollectionChanged;
                        ElementsOfLists.Remove(oldCollection);
                    }

                    if (e.NewValue is INotifyCollectionChanged newCollection)
                    {
                        newCollection.CollectionChanged += Element_CollectionChanged;
                        ElementsOfLists.Add(newCollection, element);
                        
                        // In case the new collection has no elements and the element is visible
                        SetVisibilityFromCollection(newCollection);
                    }

                    break;
                case VisibleIfOption.IsNotNull:

                    if (e.NewValue != null && !element.IsVisible)
                    {
                        element.Visibility = Visibility.Visible;
                    }
                    else if (e.NewValue == null && element.IsVisible)
                    {
                        element.Visibility = Visibility.Hidden;
                    }

                    break;
                case VisibleIfOption.IsNull:

                    if (e.NewValue == null && !element.IsVisible)
                    {
                        element.Visibility = Visibility.Visible;
                    }
                    else if (e.NewValue != null && element.IsVisible)
                    {
                        element.Visibility = Visibility.Hidden;
                    }

                    break;
            }
        }

        private static void Element_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetVisibilityFromCollection(sender as INotifyCollectionChanged);
        }

        private static void SetVisibilityFromCollection(INotifyCollectionChanged collection)
        {
            if (!ElementsOfLists.ContainsKey(collection))
                return;

            var element = ElementsOfLists[collection];
            var count = (collection as IList).Count;

            if (count == 0 && element.IsVisible)
            {
                element.Visibility = Visibility.Hidden;
            }
            else if (count > 0 && !element.IsVisible)
            {
                element.Visibility = Visibility.Visible;
            }
        }

        #endregion

    }

    public enum VisibleIfOption
    {
        AlwaysVisible,
        HasItems,
        IsNull,
        IsNotNull
    }
}
