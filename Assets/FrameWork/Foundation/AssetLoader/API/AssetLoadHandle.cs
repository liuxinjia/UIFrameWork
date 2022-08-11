using System;
using UnityEngine;

namespace Cr7Sund.AssetLoader
{
    using Object = UnityEngine.Object;

    public enum AssetLoadStatus
    {
        None,
        Success,
        Failed
    }

    public interface IAssetLoadHandle : IDisposable
    {
        // public int ControlId { get; set; }
        // public bool IsDone { get; }

        // public AssetLoadStatus Status { get;  }
        // public Exception OperationException { get;  }

        float Complete();
    }

    public sealed class AssetLoadHandle<T> : IAssetLoadHandle, IAssetLoadHandleSetter<T> where T : Object
    {
        private Func<float> CompleteFunc;

        public void SetPercentCompleteFunc(Func<float> complete)
        {
            CompleteFunc = complete;
        }

        public void SetOperationException(Exception ex)
        {
            OperationException = ex;
        }

        public void SetResult(T result)
        {
            Result = result;
        }

        public void SetStatus(AssetLoadStatus status)
        {
            Status = status;
        }



        public float Complete() => (float)(CompleteFunc?.Invoke());

        public void Dispose()
        {
            Result = null;
            CompleteFunc = null;
            throw new NotImplementedException();
        }

        #region  Properties
        public T Result { get; private set; }

        public bool IsDone => Status != AssetLoadStatus.None;
        public int ControlId { get; set; }

        public AssetLoadStatus Status { get; private set; }
        public Exception OperationException { get; private set; }

        #endregion


    }
}