namespace Cr7Sund.Runtime.Util
{
    using UnityEngine;
    using System;
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EnabledIfAttribute : PropertyAttribute
    {
        public enum HideMode { Invisible, Disabled }
        public enum SwitchType { Bool, Enum }

        public int enableIfValues;
        public HideMode hideMode;
        public string switchFieldName;
        public SwitchType switchType;

        public EnabledIfAttribute(string switcherFieldName, bool enableIfValueIs, HideMode hideMode = HideMode.Disabled)
       : this(SwitchType.Bool, switcherFieldName, enableIfValueIs ? 1 : 0, hideMode)
        {
        }

        public EnabledIfAttribute(string switcherFieldName, int enableIfValues, HideMode hideMode = HideMode.Disabled)
            : this(SwitchType.Enum, switcherFieldName, enableIfValues, hideMode)
        {
        }

        private EnabledIfAttribute(SwitchType switcherType, string switcherFieldName, int enableIfValues, HideMode hideMode)
        {
            this.switchType = switcherType;
            this.hideMode = hideMode;
            this.switchFieldName = switcherFieldName;
            this.enableIfValues = enableIfValues;
        }
    }
}