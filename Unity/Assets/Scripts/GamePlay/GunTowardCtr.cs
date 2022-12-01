using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace Game
{
    public class GunTowardCtr : MonoBehaviour
    {
        private Dictionary<int, AnimationClip> _clipDic = new Dictionary<int, AnimationClip>();
        private AnimancerComponent animancer;
        private Vector2 _GunNor;
        private Vector2 _playerNor;

        [SerializeField] private GameObject muzzleGo;
        [SerializeField] private float _Muzzleradius;
        [SerializeField] private float _Gunradius;
        [SerializeField] private PlayerController _player;

        private void Start()
        {
            var clips = Resources.LoadAll<AnimationClip>(GamePath.GunClip);

            foreach (var clip in clips)
            {
                _clipDic.Add(int.Parse(clip.name), clip);
            }

            animancer = GetComponent<AnimancerComponent>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _Muzzleradius);
        }

        private void FixedUpdate()
        {
            if (!_player.IsMove)
                return;

            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mousePos = new Vector3(pos.x, pos.y);
            _GunNor = (mousePos - transform.position).normalized;
            _playerNor = (mousePos - _player.transform.position).normalized;
            ChangeWeaponPos();
            ChangeWeaponForce();
            ChangeMuzzleForce();
        }

        void ChangeWeaponForce()
        {
            var angle = Vector2.Angle(transform.up, _playerNor);
            var isRight = Camera.main.ScreenToWorldPoint(Input.mousePosition).x > _player.transform.position.x;
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


            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > _player.transform.position.y)
            {
                GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            else
            {
                GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
        }

        void ChangeMuzzleForce()
        {
            muzzleGo.transform.position = (_GunNor * _Muzzleradius) + (Vector2)transform.position;
        }

        void ChangeWeaponPos()
        {
            transform.position = (_playerNor * _Gunradius) + (Vector2)_player.transform.position;
        }

        public bool IsInWall()
        {
            var dir = muzzleGo.transform.position - transform.position;
            var hit = Physics2D.Raycast(transform.position, dir, dir.magnitude, LayerMask.GetMask("Wall"));
            return hit.transform != null;
        }
    }
}