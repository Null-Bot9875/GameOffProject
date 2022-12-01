using System;
using Animancer;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Game
{
    public class LightAniCtr : MonoBehaviour
    {
        [SerializeField] private AnimationClip _doorOpenClip;
        [SerializeField] private AnimancerComponent _animancerComponent;

        public void PlayDoorLight()
        {
            _animancerComponent.Play(_doorOpenClip);
        }
    }
}
