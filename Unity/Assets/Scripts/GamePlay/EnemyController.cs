using Game.GameEvent;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Game
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Light2D _SightLight2D;
        [SerializeField, Header("距离")] private float _lookDistance = 1f;

        [SerializeField, Header("角度"), Range(30, 180f)]
        private float _lookAngle = 45f;

        [SerializeField, Header("射线数")] private int _lookCount = 5;

        [SerializeField, Header("当前警戒值")] private int _currentAlertValue;

        [SerializeField, Header("最高值,超过该值就报警")]
        private int _maxAlertValue = 30;

        [SerializeField, Header("没发现玩家的时候每秒减少的值")]
        private int _reducePerSecondAlertValue = 1;

        [SerializeField, Header("发现玩家的时候每秒增加的值")]
        private int _addPerSecondAlertValue = -1;

        [SerializeField] private LayerMask _layerMask;
        public int EnemyId;
        private Transform _player;

        private bool _isFoundPlayer;

        private void OnValidate()
        {
            _SightLight2D.pointLightOuterAngle = _lookAngle;
            _SightLight2D.pointLightOuterRadius = _lookDistance;
        }

        private void OnDrawGizmos()
        {
            //多一个精确度就多两条对称的射线,每条射线夹角是总角度除与精度
            var subAngle = (_lookAngle / 2) / _lookCount;
            for (int i = 0; i < _lookCount; i++)
            {
                var angleLeft = Quaternion.Euler(0, 0, -1 * subAngle * (i + 1));
                var angleRight = Quaternion.Euler(0, 0, 1 * subAngle * (i + 1));
                Debug.DrawRay(transform.position, angleLeft * transform.up.normalized * _lookDistance, Color.cyan);
                Debug.DrawRay(transform.position, angleRight * transform.up.normalized * _lookDistance, Color.cyan);
            }
        }

        private void Awake()
        {
            _SightLight2D.pointLightOuterAngle = _lookAngle;
            _SightLight2D.pointLightOuterRadius = _lookDistance;
            _player = GameDataCache.Instance.Player.transform;

            InvokeRepeating(nameof(EnemyDetectPerSec), 1f, 1f);
        }

        private void Update()
        {
            _isFoundPlayer = false;
            if (Vector3.Distance(_player.transform.position, transform.position) > _lookDistance)
            {
                return;
            }

            //多一个精确度就多两条对称的射线,每条射线夹角是总角度除与精度
            var subAngle = (_lookAngle / 2) / _lookCount;
            for (int i = 0; i < _lookCount; i++)
            {
                var angleLeft = Quaternion.Euler(0, 0, -1 * subAngle * (i + 1));
                var angleRight = Quaternion.Euler(0, 0, 1 * subAngle * (i + 1));
                if (IsLookPlayer(angleLeft) || IsLookPlayer(angleRight))
                {
                    _isFoundPlayer = true;
                }
            }

            //射出射线检测是否有Player
            bool IsLookPlayer(Quaternion eulerAnger)
            {
                var hit = Physics2D.Raycast(transform.position, eulerAnger * transform.up.normalized * _lookDistance,
                    _lookDistance, _layerMask);
                return hit.collider != null && hit.transform.CompareTag("Player");
            }
        }

        private void EnemyDetectPerSec()
        {
            _currentAlertValue += _isFoundPlayer ? _addPerSecondAlertValue : _reducePerSecondAlertValue;
            _currentAlertValue = Mathf.Clamp(_currentAlertValue, 0, _maxAlertValue);

            TypeEventSystem.Global.Send(new GameEnemyAlertEvt(EnemyId, _currentAlertValue));
            if (_currentAlertValue >= _maxAlertValue)
            {
                TypeEventSystem.Global.Send(new GameOverEvt());
            }
        }
    }
}