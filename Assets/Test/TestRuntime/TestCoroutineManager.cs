using System;
using System.Collections;
using System.Collections.Generic;
using Cr7Sund.MyCoroutine;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestCoroutineManager
{
    public int expectedNum = 0;
    public int actualNum = 0;
    private IEnumerator Warp(Func<IEnumerator> wrapAction)
    {
        yield return wrapAction?.Invoke();
        Assert.AreEqual(expectedNum, actualNum);
    }

    // Attention:
    // 1 infinite loop : same handler different ID

    [UnityTest]
    public IEnumerator TestSimpleRun()
    {
        actualNum = 0;
        expectedNum = 12;
        yield return Warp(() => CoroutineManager.Instance.Run(PushRoutine()));
    }


    [UnityTest]
    public IEnumerator TestSimpleStop()
    {
        actualNum = 0;
        expectedNum = 1;
        yield return Warp(() =>
        {
            var asyncHandle = CoroutineManager.Instance.Run(PushRoutine());
            CoroutineManager.Instance.Stop(asyncHandle);
            return asyncHandle;
        });
    }


    [UnityTest]
    public IEnumerator TestRunAddStop()
    {
        actualNum = 0;
        expectedNum = 25;
        yield return Warp(() =>
        {
            var asyncProcessHandle1 = CoroutineManager.Instance.Run(PushRoutine());
            CoroutineManager.Instance.Stop(asyncProcessHandle1);
            CoroutineManager.Instance.Run(PushRoutine());
            return CoroutineManager.Instance.Run(PushRoutine());
        });
    }

    [UnityTest]
    public IEnumerator TestComplexRun()
    {
        actualNum = 0;
        expectedNum = 24 + 1 + 10 * 1;
        yield return Warp(() =>
                {
                    CoroutineManager.Instance.Run(PushRoutine());
                    var processHandle = CoroutineManager.Instance.Run(TestWhile());
                    CoroutineManager.Instance.Stop(processHandle);
                    return CoroutineManager.Instance.Run(PushRoutine());
                });
    }

    [UnityTest]
    public IEnumerator TestAllStopInRun()
    {
        actualNum = 0;
        expectedNum = 1 + 1 + 10 + 12;
        yield return Warp(() =>
       {
           CoroutineManager.Instance.Run(PushRoutine());
            CoroutineManager.Instance.Run(TestWhile());
           //    CoroutineManager.Instance.Run(StopAll());
           CoroutineManager.Instance.StopAllCoroutines();
           return CoroutineManager.Instance.Run(PushRoutine());

       });
    }

    #region Test Methods
    private IEnumerator PushRoutine()
    {
        actualNum += 1;
        yield return null;
        actualNum += 1;
        yield return null;
        actualNum += 10;
    }


    private IEnumerator TestWhile()
    {
        int a = 0;
        actualNum += 1;
        while (true)
        {
            a++;
            actualNum += 10;
            yield return null;
        }
    }



    #endregion
}
