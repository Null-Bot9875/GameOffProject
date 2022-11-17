using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace Game
{
    public class EffectAnimationCtr : MonoBehaviour
    {
        private AnimancerComponent animancer;

        private void Start()
        {
           animancer =  GetComponent<AnimancerComponent>();
        }
    }
}
