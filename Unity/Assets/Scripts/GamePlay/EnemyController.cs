using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Game
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Light2D _SightLight2D;
        [SerializeField, Header("距离")] private float _lookDistance = 1f;
        [SerializeField, Header("角度")] private float _lookAngle = 45f;
        [SerializeField, Header("射线数")] private int _lookCount = 2;

        private Transform _player;

        private void Awake()
        {
            _SightLight2D.pointLightOuterAngle = _lookAngle;
            _SightLight2D.pointLightOuterRadius = _lookDistance;
            _player = GameDataCache.Instance.Player.transform;
        }

        private void Update()
        {
            Look();
        }

        //放射线检测
        private bool Look()
        {
            //多一个精确度就多两条对称的射线,每条射线夹角是总角度除与精度
            var subAngle = (_lookAngle / 2) / _lookCount;
            for (int i = 0; i < _lookCount; i++)
            {
                if (LookAround(Quaternion.Euler(0, -1 * subAngle * (i + 1), 0)) ||
                    LookAround(Quaternion.Euler(0, subAngle * (i + 1), 0)))
                    return true;
            }

            return false;
        }

        //射出射线检测是否有Player
        public bool LookAround(Quaternion eulerAnger)
        {
            Debug.DrawRay(transform.position, eulerAnger * transform.up.normalized * _lookDistance, Color.cyan);
            return Physics.Raycast(transform.position, eulerAnger * transform.up.normalized * _lookDistance,
                out var hit,
                _lookDistance) && hit.collider.CompareTag("Player");
        }
    }
}