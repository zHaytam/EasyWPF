# EasyWPF

EasyWPF is a utility library for WPF projects.
It provides helpful attached properties to avoid having to create a whole Converter for simple and/or specific stuff.

## Properties available for now:
 - **VisibleIf** (works with a Binding or a direct value).
 - **VisibleIfOption** (used by VisibleIf):
    - AlwaysVisible: *Makes sure the element is always visible no matter the value.*
    - HasItems: *The element is only visible when the bound collection has at least 1 item (needs VisibleIf to be bound to an ObservableCollection, it also handles the CollectionChanged event).*
    - IsNull: *The element is only visible when the value is null.*
    - IsNotNull: *The element is only visible when the value is not null.*
    - IsGreaterThanZero: *The element is only visible when the value is greater than zero (using int.TryParse).*
    - IsLessThanZero: *The element is only visible when the value is less than zero (using int.TryParse).*
    - IsEqualToZero: *The element is only visible when the value is equal to zero (using int.TryParse).*
    - IsDifferentThanZero: *The element is only visible when the value is different than zero (using int.TryParse).*
 - **VisibleIfCollapse** (used by VisibleIf): *If set to true, the element is collapsed instead of hidden.*

## Example:

    <Button Content="Create Instance" easywpf:VisibilityHelper.VisibleIf="{Binding Instance}" easywpf:VisibilityHelper.VisibleIfOption="IsNull" />  
   This Button will be visible as long as the property Instance is null.

