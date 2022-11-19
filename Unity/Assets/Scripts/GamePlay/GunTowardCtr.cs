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
        private AnimationClip toPlayClip;
        private AnimancerComponent animancer;
        private Vector2 _vector2;

        [SerializeField] private GameObject muzzleGo;
        [SerializeField] private float _radius;

        private void Start()
        {
            animancer = GetComponent<AnimancerComponent>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position,_radius);
        }

        private void FixedUpdate()
        {
           
            _vector2 = (Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position).normalized;
            // Debug.Log(Vector2.Angle(transform.up,_vector2));
            ChangeWeaponForce();
            ChangeMuzzleForce();
            
        }
        
        void ChangeWeaponForce()
        {
            var x = Vector2.Angle(transform.up, _vector2);
            float Difference = x;
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)//右侧
            {
                for (int i = 10; i <= 170; i+=20)
                {
                    foreach (var item in _Clips)
                    {
                        if (item.name == i.ToString())
                        {
                            var value = x - int.Parse(item.name);
                            if (Difference > Mathf.Abs(value))
                            {
                                Difference = value;
                                toPlayClip = item;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 10; i >= -170; i-=20)
                {
                    foreach (var item in _Clips)
                    {
                        if (item.name == i.ToString())
                        {
                            var value = x + int.Parse(item.name);
                            if (Difference > Mathf.Abs(value))
                            {
                                Difference = value;
                                toPlayClip = item;
                            }
                        }
                    }
                }
            }
            animancer.Play(toPlayClip);
            
            

            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > transform.position.y)
            {
                GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            else
            {
                GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }

        void ChangeMuzzleForce()
        {
            muzzleGo.transform.position = (_vector2 * _radius) + (Vector2)transform.position ;
        }
    }
}
