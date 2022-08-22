namespace Cr7Sund.Transition.UI
{
    using System;
    using System.Collections.Generic;
    using Cr7Sund.Runtime.Util;
    using UnityEngine;
    [System.Serializable]
    public class PageTransitionContainer
    {
        //PLAN Supoort diffent animation with different pages
        [SerializeField] private TransitionAnimation PageEnterTransition;
        [SerializeField] private TransitionAnimation PageExitTransition;



        public ITransitionAnimation GetAnimation(bool push, bool enter)
        => (enter ? PageEnterTransition.GetAnimation() : PageExitTransition.GetAnimation());
    }

    public enum TransitionAssetType
    {
        MonoBehaviour,
        ScriptableObject

    }

    [Serializable]
    public sealed class TransitionAnimation
    {

        [SerializeField] private TransitionAssetType _assetType;

        [SerializeField]
        [EnabledIf(nameof(_assetType), (int)TransitionAssetType.MonoBehaviour)]
        private TransitionBehaviour _animationBehaviour;

        [SerializeField]
        [EnabledIf(nameof(_assetType), (int)TransitionAssetType.ScriptableObject)]
        private TransitionObject _animationObject;


        public TransitionAssetType AssetType
        {
            get => _assetType;
            set => _assetType = value;
        }

        public ITransitionAnimation GetAnimation()
        {
            switch (_assetType)
            {
                case TransitionAssetType.MonoBehaviour:
                    return _animationBehaviour;
                case TransitionAssetType.ScriptableObject:
                    return _animationObject;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

}