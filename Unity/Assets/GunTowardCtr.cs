using System;
using System.Collections.Generic;
using Animancer;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class GunTowardCtr : MonoBehaviour
    {
        [SerializeField] private AnimationClip[] _Clips;
        private Dictionary<int, AnimationClip> _animationClips = new Dictionary<int, AnimationClip>();
        private AnimancerComponent animancer;
        private DirectionalAnimationSet _set;
        private Vector2 _vector2;

        private void Start()
        {
            foreach (var item in _Clips)
            {
                _animationClips.Add(Int32.Parse(item.name),item);
            }
        }

        private void FixedUpdate()
        {
            _vector2 = (Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position).normalized;
            Debug.Log(Vector2.Angle(transform.up,_vector2));
            ChangeWeaponForce();
        }
        
        void ChangeWeaponForce()
        {
            // if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)//右侧
            // {
            //      switch (Vector2.Angle(transform.up,_vector2))
            //     {
            //         case 
            //     }
            // }
            // else
            // {
            //     switch (Vector2.Angle(transform.up,_vector2))
            //     {
            //         
            //     }
            // }
            
            

            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > transform.position.y)
            {
                GetComponent<SpriteRenderer>().sortingOrder = -1;
            }
            else
            {
                GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }
    }
}
