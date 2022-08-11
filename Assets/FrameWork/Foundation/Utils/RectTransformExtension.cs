using UnityEngine;

namespace Cr7Sund.Runtime.Util
{
    public static class RectTransformExtension
    {
        public static void FillParent(this RectTransform self, RectTransform parent)
        {
            self.SetParent(parent);
            self.localPosition = Vector3.zero;
            self.anchorMin = Vector2.zero;
            self.anchorMax = Vector2.one;
            self.offsetMax = Vector2.zero;
            self.offsetMin = Vector2.zero;
            self.pivot  = new Vector2(.5f, .5f);
            self.rotation = Quaternion.identity;
            self.localScale = Vector3.one;
        }
    }
}