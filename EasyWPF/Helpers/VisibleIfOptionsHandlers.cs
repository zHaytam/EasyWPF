using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows;

namespace EasyWPF.Helpers
{
    public static class VisibleIfOptionsHandlers
    {

        #region Fields

        private static readonly Dictionary<INotifyCollectionChanged, FrameworkElement> ElementsOfLists = new Dictionary<INotifyCollectionChanged, FrameworkElement>();

        private static readonly Dictionary<VisibleIfOption, Action<FrameworkElement, object, object>> Handlers = new Dictionary<VisibleIfOption, Action<FrameworkElement, object, object>>();

        #endregion

        static VisibleIfOptionsHandlers()
        {
            foreach (var method in typeof(VisibleIfOptionsHandlers).GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
            {
                if (!method.Name.StartsWith("Handle"))
                    continue;

                var optionName = method.Name.Substring(6, method.Name.Length - 6);
                if (!Enum.TryParse(optionName, out VisibleIfOption option))
                    continue;

                var deleg = (Action<FrameworkElement, object, object>)method.CreateDelegate(typeof(Action<FrameworkElement, object, object>));
                Handlers.Add(option, deleg);
            }
        }

        #region Public Methods

        public static void HandleVisibleIf(VisibleIfOption option, FrameworkElement element, object oldValue, object newValue)
        {
            Handlers[option].Invoke(element, oldValue, newValue);
        }

        #endregion

        #region Private Methods

        private static void HandleAlwaysVisible(FrameworkElement element, object oldValue, object newValue)
        {
            if (!element.IsVisible)
            {
                ShowOrHide(element, true);
            }
        }

        private static void HandleHasItems(FrameworkElement element, object oldValue, object newValue)
        {
            if (oldValue is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= Element_CollectionChanged;
                ElementsOfLists.Remove(oldCollection);
            }

            if (!(newValue is INotifyCollectionChanged newCollection))
                return;

            newCollection.CollectionChanged += Element_CollectionChanged;
            ElementsOfLists.Add(newCollection, element);

            // In case the new collection has no elements and the element is visible
            SetVisibilityFromCollection(newCollection);
        }

        private static void HandleIsNull(FrameworkElement element, object oldValue, object newValue)
        {
            if (newValue == null && !element.IsVisible)
            {
                ShowOrHide(element, true);
            }
            else if (newValue != null && element.IsVisible)
            {
                ShowOrHide(element, false);
            }
        }

        private static void HandleIsNotNull(FrameworkElement element, object oldValue, object newValue)
        {
            if (newValue != null && !element.IsVisible)
            {
                ShowOrHide(element, true);
            }
            else if (newValue == null && element.IsVisible)
            {
                ShowOrHide(element, false);
            }
        }

        private static void HandleIsGreaterThanZero(FrameworkElement element, object oldValue, object newValue)
        {
            if (int.TryParse(newValue.ToString(), out int result))
            {
                if (result > 0)
                {
                    if (!element.IsVisible)
                    {
                        ShowOrHide(element, true);
                    }

                    return;
                }
            }

            // Not an else-if because this also covers the case where the value is <= zero
            if (element.IsVisible)
            {
                ShowOrHide(element, false);
            }
        }

        private static void HandleIsLessThanZero(FrameworkElement element, object oldValue, object newValue)
        {
            if (int.TryParse(newValue.ToString(), out int result))
            {
                if (result < 0)
                {
                    if (!element.IsVisible)
                    {
                        ShowOrHide(element, true);
                    }

                    return;
                }
            }

            // Not an else-if because this also covers the case where the value is >= zero
            if (element.IsVisible)
            {
                ShowOrHide(element, false);
            }
        }

        private static void HandleIsEqualToZero(FrameworkElement element, object oldValue, object newValue)
        {
            if (int.TryParse(newValue.ToString(), out int result))
            {
                if (result == 0)
                {
                    if (!element.IsVisible)
                    {
                        ShowOrHide(element, true);
                    }

                    return;
                }
            }

            // Not an else-if because this also covers the case where the value is != zero
            if (element.IsVisible)
            {
                ShowOrHide(element, false);
            }
        }

        private static void HandleIsDifferentThanZero(FrameworkElement element, object oldValue, object newValue)
        {
            if (int.TryParse(newValue.ToString(), out int result))
            {
                if (result != 0)
                {
                    if (!element.IsVisible)
                    {
                        ShowOrHide(element, true);
                    }

                    return;
                }
            }

            // Not an else-if because this also covers the case where the value is != zero
            if (element.IsVisible)
            {
                ShowOrHide(element, false);
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

        private static void ShowOrHide(FrameworkElement element, bool show)
        {
            if (show)
            {
                element.Visibility = Visibility.Visible;
            }
            else
            {
                bool collapse = (bool)element.GetValue(VisibilityHelper.VisibleIfCollapseProperty);
                element.Visibility = collapse ? Visibility.Collapsed : Visibility.Hidden;
            }
        }

        #endregion

    }
}
