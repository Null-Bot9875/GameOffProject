using System.Collections.Generic;
using DG.Tweening;
using Game.GameEvent;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Game
{
    [System.Serializable]
    public class EnemyPatrolInfo
    {
        public bool IsInvalid;
        public int CrtIdx { get; set; } = 1;
        public List<Transform> List;

        public Vector3 TargetPosition => List[CrtIdx].position;

        public float Speed = 1f;

        public void InitEnemyPatrol(EnemyController controller)
        {
            if (List.Count == 0)
            {
                IsInvalid = true;
                return;
            }

            controller.transform.position = List[0].position;
            foreach (var item in List)
            {
                item.gameObject.SetActive(false);
            }
        }

        public void SetNextIdx()
        {
            CrtIdx += 1;
            if (CrtIdx >= List.Count)
            {
                CrtIdx = 0;
            }
        }
    }

    public class EnemyController : MonoBehaviour
    {
        public int EnemyId;
        [SerializeField] private Light2D _SightLight2D;
        [SerializeField, Header("距离")] private float _lookDistance = 1f;

        [SerializeField, Header("角度"), Range(30, 180f)]
        private float _lookAngle = 45f;

        [SerializeField, Header("射线数")] private int _lookCount = 5;

        [SerializeField] private LayerMask _layerMask;

        [SerializeField] private EnemyPatrolInfo _enemyPatrol;
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
                Debug.DrawRay(transform.position, angleLeft * transform.right.normalized * _lookDistance, Color.cyan);
                Debug.DrawRay(transform.position, angleRight * transform.right.normalized * _lookDistance, Color.cyan);
            }
        }

        private void Start()
        {
            _SightLight2D.pointLightOuterAngle = _lookAngle;
            _SightLight2D.pointLightOuterRadius = _lookDistance;
            _player = GameDataCache.Instance.Player.transform;
            _enemyPatrol.InitEnemyPatrol(this);
            EnemyPatrol();
            InvokeRepeating(nameof(EnemyDetectPerSec), 1f, .5f);
        }

        private void EnemyPatrol()
        {
            if (_enemyPatrol.IsInvalid)
                return;
            var targetDir = _enemyPatrol.TargetPosition - transform.position;
            //指定哪根轴朝向目标
            var fromDir = transform.rotation * Vector3.right;
            //计算垂直于当前方向和目标方向的轴
            var axis = Vector3.Cross(fromDir, targetDir).normalized;
            //计算当前方向和目标方向的夹角
            var angle = Vector3.Angle(fromDir, targetDir);
            //将当前朝向向目标方向旋转一定角度，这个角度值可以做插值
            var rotation = Quaternion.AngleAxis(angle, axis) * transform.rotation;


            transform.DORotate(rotation.eulerAngles, .5f);
            transform.DOMove(_enemyPatrol.TargetPosition, _enemyPatrol.Speed).SetSpeedBased().OnComplete(() =>
            {
                _enemyPatrol.SetNextIdx();
                EnemyPatrol();
            });
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
                var hit = Physics2D.Raycast(transform.position, eulerAnger * transform.right.normalized * _lookDistance,
                    _lookDistance, _layerMask);
                return hit.collider != null && hit.transform.CompareTag("Player");
            }
        }

        private void EnemyDetectPerSec()
        {
            if (_isFoundPlayer)
            {
                _player.GetComponent<PlayerController>().enabled = false;
                transform.DOMove(_player.transform.position, .5f).OnComplete(() =>
                {
                    TypeEventSystem.Global.Send(new GameOverEvt());
                });
            }
        }
    }
}