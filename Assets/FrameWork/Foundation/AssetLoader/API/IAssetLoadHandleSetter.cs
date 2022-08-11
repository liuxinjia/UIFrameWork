using System;

namespace Cr7Sund.AssetLoader
{
    public interface IAssetLoadHandleSetter<T>
    {
        void SetStatus(AssetLoadStatus status);
        void SetResult(T result);
        void SetPercentCompleteFunc(Func<float> complete);
        void SetOperationException(Exception ex);
    }

    public static class IAssetLoadHandleSetterExtension
    {
        public static void SetLoadResult<T>(this IAssetLoadHandleSetter<T> self, string key, T result) 
        {
            self.SetResult(result);
            var status = result != null ? AssetLoadStatus.Success : AssetLoadStatus.Failed;
            self.SetStatus(status);
            if (result == null)
            {
                var exception = new InvalidOperationException($"$Requestd asset (Key: {key} was not found");
                self.SetOperationException(exception);
            }
        }
    }
}