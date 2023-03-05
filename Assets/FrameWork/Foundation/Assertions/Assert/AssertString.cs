// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Diagnostics;
using UnityEngine;
using Cr7Sund.Assertions.Comparers;

namespace Cr7Sund.Assertions
{
    public static partial class Assert
    {
        [Conditional(UNITY_ASSERTIONS)]
        public static void IsNotNullOrEmpty(string value, string message = null)
        {
            if (string.IsNullOrEmpty(value))
                Fail(AssertionMessageUtil.NullFailureMessage(value, false), message);
        }
    }
}
