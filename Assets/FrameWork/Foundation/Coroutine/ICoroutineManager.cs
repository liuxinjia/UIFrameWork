using System.Collections;

namespace Cr7Sund.MyCoroutine
{
    internal interface ICoroutineManager
    {
        AsyncProcessHandle Run(IEnumerator routine);
        void Stop(AsyncProcessHandle handle);
    }
}