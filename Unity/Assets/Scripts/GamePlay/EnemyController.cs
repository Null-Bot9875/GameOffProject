using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using Game.GameEvent;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Game
{
    [System.Serializable]
    public class EnemyPatrolInfo
    {
        public int CrtIdx { get; set; } = 1;
        public List<Transform> List;

        public Vector3 TargetPosition => List[CrtIdx].position;

        public float Speed = 1f;

        public void InitEnemyPatrol(EnemyController controller)
        {
            if (List.Count == 0)
                return;

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

    public class EnemyController : MonoBehaviour, IExplosion, IBulletTrigger
    {
        [SerializeField] private Light2D _SightLight2D;
        [SerializeField, Header("距离")] private float _lookDistance = 1f;

        [SerializeField, Header("角度"), Range(30, 180f)]
        private float _lookAngle = 45f;

        [SerializeField, Header("射线数")] private int _lookCount = 5;

        [SerializeField] private LayerMask _layerMask;

        [SerializeField] private EnemyPatrolInfo _enemyPatrol;
        [SerializeField] private AnimancerComponent _animancer;

        private bool _isInvalid;
        private Transform _player;
        private Transform _light;
        private bool _isFoundPlayer;
        private Dictionary<string, AnimationClip> _clipDic = new Dictionary<string, AnimationClip>();

        [SerializeField, Header("初始化的朝向,原地位置使用")]
        private bool _isInitForward;

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
                Debug.DrawRay(transform.position,
                    angleLeft * transform.Find("ViewLight").right.normalized * _lookDistance, Color.cyan);
                Debug.DrawRay(transform.position,
                    angleRight * transform.Find("ViewLight").right.normalized * _lookDistance, Color.cyan);
            }
        }

        private void Start()
        {
            var clips = Resources.LoadAll<AnimationClip>(GamePath.EnemyClip);
            foreach (var clip in clips)
            {
                _clipDic.Add(clip.name, clip);
            }

            _SightLight2D.pointLightOuterAngle = _lookAngle;
            _SightLight2D.pointLightOuterRadius = _lookDistance;
            _player = GameDataCache.Instance.Player.transform;
            _enemyPatrol.InitEnemyPatrol(this);
            _animancer.Play(_isInitForward ? _clipDic["ForwardClip"] : _clipDic["BackClip"]);
            _light = transform.Find("ViewLight");
            InvokeRepeating(nameof(EnemyDetectPerSec), 1f, .5f);
            EnemyPatrol();
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        private void EnemyPatrol()
        {
            if (_enemyPatrol.List.Count == 0)
                return;
            var targetDir = _enemyPatrol.TargetPosition - _light.position;
            _light.right = targetDir;
            var isBack = _enemyPatrol.TargetPosition.y > transform.position.y;
            _animancer.Play(isBack ? _clipDic["BackClip"] : _clipDic["ForwardClip"]);

            var isRight = _enemyPatrol.TargetPosition.x > transform.position.x;
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = !isRight;
            transform.DOMove(_enemyPatrol.TargetPosition, _enemyPatrol.Speed).SetSpeedBased().OnComplete(() =>
            {
                _enemyPatrol.SetNextIdx();
                EnemyPatrol();
            });
        }

        private void Update()
        {
            if (_isInvalid)
                return;
            _isFoundPlayer = false;
            if (Vector3.Distance(_player.transform.position, transform.position) > _lookDistance)
                return;

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
                var hit = Physics2D.Raycast(transform.position,
                    eulerAnger * _light.right.normalized * _lookDistance,
                    _lookDistance, _layerMask);
                return hit.transform != null && hit.transform.CompareTag("Player");
            }
        }

        private void EnemyDetectPerSec()
        {
            if (_isFoundPlayer)
            {
                CancelInvoke(nameof(EnemyDetectPerSec));
                _isInvalid = true;
                _player.GetComponent<PlayerController>().IsMove = false;
                _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                var state = _animancer.Play(_clipDic["AttackClip"]);
                state.Events.OnEnd += Attack;
                transform.DOKill();
            }

            void Attack()
            {
                GetComponent<SpriteRenderer>().sortingOrder = 9999;
                _animancer.Play(_clipDic["ForwardClip"]);
                transform.DOMove(_player.transform.position, .5f).OnComplete(() =>
                {
                    TypeEventSystem.Global.Send(new GameOverEvt(DieReason.Enemy));
                });
                AudioManager.Instance.PlayAudioOnce(GamePath.SFXEnemyAttack);
            }
        }

        public void OnExplosion()
        {
            //死亡
            Die();
        }

        public void OnBulletTrigger(BulletCtr ctr)
        {
            //不生成敌人模拟对象，不需要判断ghost
            AudioManager.Instance.PlayAudioOnce(GamePath.SFXBulletHit);
            Die();
        }

        private void Die()
        {
            if (_isInvalid)
                return;
            _isInvalid = true;
            this.enabled = false;
            transform.DOKill();
            GetComponent<Collider2D>().enabled = false;
            var state = _animancer.Play(_clipDic["DieClip"]);
            GameDataCache.Instance.EnemyList.Remove(this);
            state.Events.OnEnd += () => GameObject.Destroy(gameObject);
            AudioManager.Instance.PlayAudioOnce(GamePath.SFXEnemyDie);
            Camera.main.DOShakePosition(0.1f, 0.12f, 7, 90f);
        }
    }
}