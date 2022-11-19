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
        private bool _isCanMove = true;
        private Vector2 bulletOnPlacePos;

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

        private GameObject bullet;
        private Rigidbody2D rb;
        private Camera _camera;

        #endregion

        private void Awake()
        {
            bullet = Resources.Load(GamePath.BulletPath) as GameObject;
            _camera = Camera.main;
            rb = GetComponent<Rigidbody2D>();
            InvokeRepeating(nameof(RepeatCountCd), 0, .1f);
            TypeEventSystem.Global.Register<GameBulletShotOnPlaceEvt>(OnBulletOnPlaceEvt);
            TypeEventSystem.Global.Register<GameRecycleBulletGhost>(OnRecycleBulletGhostEvt);
        }

        private void OnDestroy()
        {
            CancelInvoke(nameof(RepeatCountCd));
            TypeEventSystem.Global.UnRegister<GameBulletShotOnPlaceEvt>(OnBulletOnPlaceEvt);
            TypeEventSystem.Global.UnRegister<GameRecycleBulletGhost>(OnRecycleBulletGhostEvt);
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
            _isCanMove = true;
        }

        private void OnRecycleBulletGhostEvt(GameRecycleBulletGhost evt)
        {
            _isAimSelf = evt.IsAimSelf;
        }

        void Update()
        {
            if (_isCanMove)
            {
                //鼠标跟随
                _mouseV2 = _camera.ScreenToWorldPoint(Input.mousePosition);
                //角色移动
                var x = Input.GetAxis("Horizontal");
                var y = Input.GetAxis("Vertical");
                rb.velocity = new Vector2(x * moveSpeed, y * moveSpeed);
            }

            if (Input.GetMouseButtonDown(1))
            {
                _line.gameObject.GetComponent<Projection>().Enable();
            }

            if (Input.GetMouseButtonUp(1))
            {
                _line.gameObject.GetComponent<Projection>().Disable();
            }

            if (Input.GetMouseButton(1))
            {
                //预测
                CreatSimulateBullet();

                if (!IsCanShoot())
                    return;

                //开火
                if (Input.GetMouseButtonDown(0))
                {
                    _line.gameObject.GetComponent<Projection>().Disable();
                    var go = InstantiateBullet();
                    go.GetComponent<BulletCtr>().SetFire(GetBulletDir());
                    if (_isForwardShoot)
                    {
                        _isForwardShoot = false;
                        _isHaveBullet = false;
                    }
                    else
                    {
                        _isCanMove = false;
                        _isAimSelf = false;
                        _isBulletOnWall = false;
                        rb.velocity = Vector2.zero;
                        TypeEventSystem.Global.Send<GameRecycleBulletRequestEvt>();
                    }
                }
            }
        }

        private bool IsCanShoot()
        {
            var isCanShoot = true;
            //回收的时候无视CD限制
            var isShootCd = _countCd == 0;
            if (_isForwardShoot)
            {
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
            var go = Instantiate(bullet);
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
                _isCanMove = true;
                _isForwardShoot = true;
                _countCd = _shootCD;
                TypeEventSystem.Global.Send<GameRecycleBulletTriggerEvt>();
            }
            else
            {
                Die();
            }
        }

        void Die()
        {
            Debug.Log("playerDie");
            TypeEventSystem.Global.Send<GameOverEvt>();
        }

        public Vector2 GetMouseInfo()
        {
            return (_mouseV2 - (Vector2)gunGo.transform.position).normalized;
        }

        private Vector2 GetBulletDir()
        {
            return _isForwardShoot ? GetDirection_ToGun() : GetDirection_WallBulletToPlayer();
        }

        private Vector2 GetDirection_ToGun()
        {
            return (_mouseV2 - (Vector2)gunGo.transform.position).normalized;
        }

        private Vector2 GetDirection_WallBulletToPlayer()
        {
            return ((Vector2)transform.position - bulletOnPlacePos).normalized;
        }
    }
}