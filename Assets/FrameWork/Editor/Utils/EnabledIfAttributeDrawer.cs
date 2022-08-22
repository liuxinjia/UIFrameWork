namespace Cr7Sund.Editor.Util
{
    using Cr7Sund.Runtime.Util;

    using UnityEngine;
    using UnityEditor;
    using UnityEngine.UIElements;
    using System;
    using UnityEditor.UIElements;

    [CustomPropertyDrawer(typeof(EnabledIfAttribute))]
    public class EnabledIfAttributeDrawer : PropertyDrawer
    {
        // public override VisualElement CreatePropertyGUI(SerializedProperty property)
        // {
        //     var attr = attribute as EnabledIfAttribute;
        //     var isEnabled = GetIsEnabled(attr, property);

        //     if (attr.hideMode == EnabledIfAttribute.HideMode.Disabled)
        //     {
        //         GUI.enabled &= isEnabled;
        //     }

        //     if (GetIsVisible(attr, property))
        //     {
        //         return new PropertyField(property);
        //     }
        //     if (attr.hideMode == EnabledIfAttribute.HideMode.Disabled)
        //     {
        //         GUI.enabled = true;
        //     }

        //     return null;
        // }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as EnabledIfAttribute;
            var isEnabled = GetIsEnabled(attr, property);

            if (attr.hideMode == EnabledIfAttribute.HideMode.Disabled)
            {
                GUI.enabled &= isEnabled;
                
            }

            if (GetIsVisible(attr, property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            if (attr.hideMode == EnabledIfAttribute.HideMode.Disabled)
            {
                GUI.enabled = true;
            }
        }

        // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        // {
        //     var attr = attribute as EnabledIfAttribute;
        //     return GetIsVisible(attr, property) ? EditorGUI.GetPropertyHeight(property) : -2;
        // }


        private bool GetIsVisible(EnabledIfAttribute attribute, SerializedProperty property)
        {
            if (GetIsEnabled(attribute, property))
            {
                return true;
            }

            if (attribute.hideMode != EnabledIfAttribute.HideMode.Invisible)
            {
                return true;
            }

            return false;
        }

        private bool GetIsEnabled(EnabledIfAttribute attribute, SerializedProperty property)
            => attribute.enableIfValues == GetSwitchPropertyValue(attribute, property);

        private int GetSwitchPropertyValue(EnabledIfAttribute attribute, SerializedProperty property)
        {
            var propertyNameIndex = property.propertyPath.LastIndexOf(property.name, StringComparison.Ordinal);
            var switchPropertyName =
                    property.propertyPath.Substring(0, propertyNameIndex) + attribute.switchFieldName;
            var switchProperty = property.serializedObject.FindProperty(switchPropertyName);
            switch (switchProperty.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    return switchProperty.boolValue ? 1 : 0;
                case SerializedPropertyType.Enum:
                    return switchProperty.intValue;
                default:
                    throw new Exception("unsupported type.");
            }
        }
    }
}