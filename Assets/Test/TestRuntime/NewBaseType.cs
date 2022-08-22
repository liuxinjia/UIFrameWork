using System.Collections;
using Cr7Sund.MyCoroutine;
using NUnit.Framework;
using UnityEngine;

public class NewBaseType : MonoBehaviour
{
    // A Test behaves as an ordinary method
    public void Start()
    {
        var handle = CoroutineManager.Instance.Run(PushNestedRoutine());
        var handle1 = CoroutineManager.Instance.Run(PushRoutine());

        Debug.Log("start");
    }

/// <summary>
/// Can sure the execute order
/// </summary>
/// <returns></returns>
    private IEnumerator PushRoutine()
    {
        yield return new WaitForFixedUpdate(); 
        Debug.Log("test");
        // yield return 2;
        // Debug.Log("test2");
        // yield return 3;
        // Debug.Log("test3");
    }

    private IEnumerator PushRoutineDealy()
    {
        yield return new WaitForFixedUpdate();
        Debug.Log("test delay");
        yield return new WaitForFixedUpdate();
        Debug.Log("test2");
        yield return new WaitForFixedUpdate();
        Debug.Log("test3");
    }

    private IEnumerator PushNestedRoutineDealy()
    {
        yield return new WaitForFixedUpdate();
        Debug.Log("test nested  nested delay");

    }

    private IEnumerator PushRoutineDealy1()
    {
        yield return CoroutineManager.Instance.Run(PushNestedRoutineDealy());

        yield return new WaitForFixedUpdate();
        Debug.Log("test2 Nested");
        yield return new WaitForFixedUpdate();
        Debug.Log("test3 Nested");
    }




    private IEnumerator PushNestedRoutine()
    {
        yield return CoroutineManager.Instance.Run(PushRoutineDealy());
        yield return CoroutineManager.Instance.Run(PushRoutineDealy1());

        Debug.Log("start test Nested");

        yield return new WaitForFixedUpdate();
        Debug.Log("end test Nested");


    }
}
