using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;

namespace EasyWPF.Helpers
{
    public static class VisibieIfOptionsHandlers
    {

        #region Fields

        private static readonly Dictionary<INotifyCollectionChanged, FrameworkElement> ElementsOfLists = new Dictionary<INotifyCollectionChanged, FrameworkElement>();

        private static readonly Dictionary<VisibleIfOption, Action<FrameworkElement, object, object>> Handlers = new Dictionary<VisibleIfOption, Action<FrameworkElement, object, object>>
        {
            { VisibleIfOption.AlwaysVisible, HandleAlwaysVisible },
            { VisibleIfOption.HasItems, HandleHasItems },
            { VisibleIfOption.IsNull, HandleIsNull },
            { VisibleIfOption.IsNotNull, HandleIsNotNull },
            { VisibleIfOption.IsGreaterThanZero, HandleIsGreaterThanZero },
            { VisibleIfOption.IsLessThanZero, HandleIsLessThanZero },
            { VisibleIfOption.IsEqualToZero, HandleIsEqualToZero },
            { VisibleIfOption.IsDifferentThanZero, HandleIsDifferentThanZero }
        };

        #endregion

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
                element.Visibility = Visibility.Visible;
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
                element.Visibility = Visibility.Visible;
            }
            else if (newValue != null && element.IsVisible)
            {
                element.Visibility = Visibility.Hidden;
            }
        }

        private static void HandleIsNotNull(FrameworkElement element, object oldValue, object newValue)
        {
            if (newValue != null && !element.IsVisible)
            {
                element.Visibility = Visibility.Visible;
            }
            else if (newValue == null && element.IsVisible)
            {
                element.Visibility = Visibility.Hidden;
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
                        element.Visibility = Visibility.Visible;
                    }

                    return;
                }
            }

            // Not an else-if because this also covers the case where the value is <= zero
            if (element.IsVisible)
            {
                element.Visibility = Visibility.Hidden;
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
                        element.Visibility = Visibility.Visible;
                    }

                    return;
                }
            }

            // Not an else-if because this also covers the case where the value is >= zero
            if (element.IsVisible)
            {
                element.Visibility = Visibility.Hidden;
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
                        element.Visibility = Visibility.Visible;
                    }

                    return;
                }
            }

            // Not an else-if because this also covers the case where the value is != zero
            if (element.IsVisible)
            {
                element.Visibility = Visibility.Hidden;
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
                        element.Visibility = Visibility.Visible;
                    }

                    return;
                }
            }

            // Not an else-if because this also covers the case where the value is != zero
            if (element.IsVisible)
            {
                element.Visibility = Visibility.Hidden;
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
}
