using System.Collections;
using System.Collections.Generic;
using Cr7Sund.MyCoroutine;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestCoroutineManager
{


    private IEnumerator PushRoutine()
    {
        yield return new WaitForFixedUpdate();
        Debug.Log("test");
        yield return new WaitForFixedUpdate();
        Debug.Log("test2");
    }
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestCoroutineManagerWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return CoroutineManager.Instance.Run(PushRoutine());

        


    }

    [Test]
    public void SimulateTestCoroutine(){
        var handle = CoroutineManager.Instance.Run(PushRoutine());
//         while(!handle.IsTerminated){
// var cur = handle.MoveNext();
//         }
    }
}
