using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace Game
{
    public class GunTowardCtr : MonoBehaviour
    {
        private Dictionary<int, AnimationClip> _clipDic = new Dictionary<int, AnimationClip>();
        private AnimancerComponent animancer;
        private Vector2 _vector2;

        [SerializeField] private GameObject muzzleGo;
        [SerializeField] private float _radius;

        private void Start()
        {
            var clips = Resources.LoadAll<AnimationClip>(GamePath.GunClipPath);
            foreach (var clip in clips)
            {
                _clipDic.Add(int.Parse(clip.name), clip);
            }

            animancer = GetComponent<AnimancerComponent>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        private void FixedUpdate()
        {

            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mousePos = new Vector3(pos.x, pos.y);
            _vector2 = (mousePos - transform.position).normalized;
            ChangeWeaponForce();
            ChangeMuzzleForce();
        }

        void ChangeWeaponForce()
        {
            var angle = Vector2.Angle(transform.up, _vector2);
            var isRight = Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x;
            angle = isRight ? angle : -angle;
            var difference = float.MaxValue;
            var num = 0;
            for (int i = -170; i <= 170; i += 20)
            {
                var maxValue = Mathf.Max(angle, i);
                var minValue = Mathf.Min(angle, i);
                var value = maxValue - minValue;
                if (value < difference)
                {
                    difference = value;
                    num = i;
                }
            }

            animancer.Play(_clipDic[num]);


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
            //Debug.Log(_vector2.magnitude);
            muzzleGo.transform.position = (_vector2 * _radius) + (Vector2)transform.position;
        }
    }
}