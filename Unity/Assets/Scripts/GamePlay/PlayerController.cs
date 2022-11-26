using Game.GameEvent;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour, IExplosion, IBulletTrigger
    {
        [SerializeField, Header("玩家移动速度")] public float moveSpeed;
        [SerializeField, Header("偏移系数")] private float offsetCoefficient;
        private Vector2 _mouseV2;

        private bool _isForwardShoot = true;
        private bool _isHaveBullet = true;
        private bool _isBulletOnWall;
        private bool _isAimSelf;
        private Vector2 bulletOnPlacePos;

        private GameObject _fireEffect;
        private GameObject _recycleEffect;

        public bool IsMove { get; set; } = true;

        #region 子弹回收

        [SerializeField, Header("子弹回收后再次射出CD")]
        private float _shootCD = 0.5f; //todo 5秒CD

        private float _countCd;

        #endregion

        #region 组件

        [SerializeField] private GameObject gunGo;
        [SerializeField] private Transform muzzle;
        [SerializeField] private Projection _projection;
        [SerializeField] private LineRenderer _line;

        private GameObject _bullet;
        private Rigidbody2D _rb;
        private Camera _camera;

        #endregion

        private void Awake()
        {
            _bullet = Resources.Load(GamePath.BulletPfb) as GameObject;
            _camera = Camera.main;
            _fireEffect = Resources.Load<GameObject>(GamePath.FireEffectPfb);
            _recycleEffect = Resources.Load<GameObject>(GamePath.RecycleEffectPfb);
            _rb = GetComponent<Rigidbody2D>();
            InvokeRepeating(nameof(RepeatCountCd), 0, .1f);
            TypeEventSystem.Global.Register<GameBulletShotOnPlaceEvt>(OnBulletOnPlaceEvt);
        }

        private void OnDestroy()
        {
            CancelInvoke(nameof(RepeatCountCd));
            TypeEventSystem.Global.UnRegister<GameBulletShotOnPlaceEvt>(OnBulletOnPlaceEvt);
        }

        void RepeatCountCd()
        {
            _countCd -= .1f;
            if (_countCd <= 0)
                _countCd = 0;
        }

        void OnBulletOnPlaceEvt(GameBulletShotOnPlaceEvt gameBulletShotOnPlaceEvt)
        {
            bulletOnPlacePos = gameBulletShotOnPlaceEvt.bulletPos;
            _isBulletOnWall = true;
            IsMove = true;
        }

        void Update()
        {
            if (IsMove)
            {
                //鼠标跟随
                _mouseV2 = _camera.ScreenToWorldPoint(Input.mousePosition);
                //角色移动
                var x = Input.GetAxis("Horizontal");
                var y = Input.GetAxis("Vertical");
                _rb.velocity = new Vector2(x * moveSpeed, y * moveSpeed);
            }

            if (!_isForwardShoot)
            {
                var angle = Vector2.Angle(GetDirection_MouseToPlayer(), GetDirection_WallBulletToPlayer());
                _isAimSelf = (180 - angle) < 10;
            }

            if (Input.GetMouseButtonDown(1))
            {
                _projection.Enable();
            }

            if (Input.GetMouseButtonUp(1))
            {
                _projection.Disable();
            }

            if (Input.GetMouseButton(1))
            {
                if (!IsCanShoot())
                {
                    _projection.Disable();
                    return;
                }

                _projection.Enable();
                //预测
                CreatSimulateBullet();

                //开火
                if (Input.GetMouseButtonDown(0))
                {
                    _projection.Disable();
                    var go = InstantiateBullet();
                    go.GetComponent<BulletCtr>().SetFire(GetBulletDir());
                    if (_isForwardShoot)
                    {
                        _isForwardShoot = false;
                        _isHaveBullet = false;
                        var effectGo = Instantiate(_fireEffect);
                        effectGo.transform.position = muzzle.transform.position;
                        GameDataCache.Instance.ShootCount += 1;
                        TypeEventSystem.Global.Send(new GameShootBulletRequestEvt());
                    }
                    else
                    {
                        IsMove = false;
                        _isAimSelf = false;
                        _isBulletOnWall = false;
                        _rb.velocity = Vector2.zero;
                        TypeEventSystem.Global.Send<GameRecycleBulletRequestEvt>();
                    }
                }
            }
        }

        private bool IsCanShoot()
        {
            var isCanShoot = true;
            //不能移动的时候不能射击
            isCanShoot &= IsMove;
            if (_isForwardShoot)
            {
                var isWall = gunGo.GetComponent<GunTowardCtr>().IsInWall();
                //子弹不在墙里可以射击
                isCanShoot &= !isWall;
                //回收的时候无视CD限制
                var isShootCd = _countCd == 0;
                isCanShoot &= _isHaveBullet;
                isCanShoot &= isShootCd;
            }
            else
            {
                isCanShoot &= _isBulletOnWall;
                isCanShoot &= _isAimSelf;
            }

            return isCanShoot;
        }

        private void CreatSimulateBullet()
        {
            var go = InstantiateBullet();
            var bulletCtr = go.GetComponent<BulletCtr>();
            bulletCtr.IsGhost = true;
            _projection.SimulateLinePosition(bulletCtr, GetBulletDir());
            Destroy(go);
        }

        private GameObject InstantiateBullet()
        {
            var go = Instantiate(_bullet);
            var bulletCtr = go.GetComponent<BulletCtr>();
            if (_isForwardShoot)
            {
                go.transform.position = muzzle.transform.position;
                go.transform.rotation = gunGo.transform.rotation;
            }
            else
            {
                go.transform.position = bulletOnPlacePos - GetDirection_WallBulletToPlayer() * offsetCoefficient;
                go.transform.rotation = Quaternion.identity;
                bulletCtr.IsBack = true;
            }

            return go;
        }

        public void OnExplosion()
        {
            Die();
        }

        public void OnBulletTrigger(BulletCtr ctr)
        {
            ctr.DestroyGo();
            if (ctr.IsBack)
            {
                _isHaveBullet = true;
                IsMove = true;
                _isForwardShoot = true;
                _countCd = _shootCD;
                var effect = GameObject.Instantiate(_recycleEffect, muzzle.transform);
                effect.transform.position = muzzle.transform.position;
                TypeEventSystem.Global.Send<GameRecycleBulletTriggerEvt>();
            }
            else
            {
                Die();
            }
        }

        void Die()
        {
            GameObject.Destroy(gameObject);
            TypeEventSystem.Global.Send<GameOverEvt>();
        }

        private Vector2 GetBulletDir()
        {
            return _isForwardShoot ? GetDirection_MouseToPlayer() : GetDirection_WallBulletToPlayer();
        }

        public Vector2 GetDirection_MouseToPlayer()
        {
            return (_mouseV2 - (Vector2)transform.position).normalized;
        }

        private Vector2 GetDirection_WallBulletToPlayer()
        {
            return ((Vector2)transform.position - bulletOnPlacePos).normalized;
        }
    }
}