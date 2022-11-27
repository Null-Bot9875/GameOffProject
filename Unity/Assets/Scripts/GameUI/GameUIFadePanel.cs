using System;
using UnityEngine;

namespace Game
{
    public class GameUIFadePanel : MonoBehaviour
    {
        [SerializeField] private bool isFadeIn;
        [SerializeField] private AnimationClip _inClip;
        [SerializeField] private AnimationClip _outClip;
        [SerializeField] private Animancer.AnimancerComponent _animancer;
        public bool animcerDone = false;

        private void Awake()
        {
            if (isFadeIn)
            {
                var state = _animancer.Play(_inClip);
                state.Events.OnEnd = () => animcerDone = true;
            }
            else
            {
                var state1 = _animancer.Play(_outClip);
                state1.Events.OnEnd = () => Destroy(gameObject);
            }
        }
    }
}