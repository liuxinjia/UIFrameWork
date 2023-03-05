using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;

public class TestUIManager
{
    [Test, Performance]
    public void Vector2_operations()
    {
        var a = Vector2.one;
        var b = Vector2.zero;

        Measure.Method(() =>
        {
            Vector2.MoveTowards(a, b, 0.5f);
            Vector2.ClampMagnitude(a, 0.5f);
            Vector2.Reflect(a, b);
            Vector2.SignedAngle(a, b);
        })
            .MeasurementCount(20)
            .Run();
    }
}
