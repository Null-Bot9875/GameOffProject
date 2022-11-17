using Game.GameEvent;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour, IExplosion, IBulletTrigger
    {
        [SerializeField, Header("玩家移动速度")] public float moveSpeed;
        [SerializeField, Header("偏移系数")] private float offsetCoefficient;
        private Vector2 _mouseV2;
        private bool _isForwardShoot;
        private bool _canShoot;
        public bool _canMove;
        private Vector2 bulletOnPlacePos;

        #region 子弹回收

        [SerializeField, Header("子弹回收后再次射出CD")]
        private float _shootCD = 0.5f; //todo 5秒CD

        private float _nowShootTime;
        private bool _canShootCd;

        #endregion

        #region 组件

        private Rigidbody2D rb;
        [SerializeField] private GameObject gunGo;
        [SerializeField] private Transform muzzle;
        private Camera _camera;
        [SerializeField] private Projection _projection;
        private GameObject bullet;
        [SerializeField] private LineRenderer _line;

        #endregion

        private void Awake()
        {
            bullet = Resources.Load("Prefabs/Item/Bullet") as GameObject;
        }

        void Start()
        {
            #region 事件注册

            TypeEventSystem.Global.Register<GameBulletShotOnPlaceEvt>(OnBulletOnPlaceEvt)
                .UnRegisterWhenGameObjectDestroyed(gameObject);

            #endregion

            _camera = Camera.main;
            rb = GetComponent<Rigidbody2D>();
            _isForwardShoot = true;
            _canShoot = true;
            _canMove = true;
            _nowShootTime = -.5f; //todo 5秒cd
        }

        void OnBulletOnPlaceEvt(GameBulletShotOnPlaceEvt gameBulletShotOnPlaceEvt)
        {
            bulletOnPlacePos = gameBulletShotOnPlaceEvt.bulletPos;
            _canShoot = true;
            _canMove = true;
        }

        void Update()
        {
            #region 鼠标跟随

            if (_canMove)
            {
                _mouseV2 = _camera.ScreenToWorldPoint(Input.mousePosition);
            }

            #endregion

            #region 角色移动

            if (_canMove)
            {
                var x = Input.GetAxis("Horizontal");
                var y = Input.GetAxis("Vertical");
                rb.velocity = new Vector2(x * moveSpeed, y * moveSpeed);
            }

            #endregion

            #region 瞄准射击

            _canShootCd = Time.time > _shootCD + _nowShootTime;

            if (_canShoot && _canShootCd)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _line.gameObject.SetActive(true);
                    _line.gameObject.GetComponent<Projection>().Enable();
                }

                if (Input.GetMouseButton(1))
                {
                    CreatSimulateBullet();

                    if (Input.GetMouseButtonDown(0))
                    {
                        var go = InstantiateBullet();
                        if (_isForwardShoot)
                        {
                            go.GetComponent<BulletCtr>().SetFire(GetDirection_ToGun());
                            _isForwardShoot = false;
                            _canShoot = false;
                        }
                        else
                        {
                            TypeEventSystem.Global.Send<GamePlayerWantRetrievesBulletEvt>();
                            go.GetComponent<BulletCtr>().SetFire(GetDirection_GoToPlayer(go.transform.position));
                            _canShoot = false;
                            _canMove = false;
                            rb.velocity = Vector2.zero;
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(1) || !_canShoot)
            {
                _line.gameObject.SetActive(false);
                _line.gameObject.GetComponent<Projection>().Disable();
            }

            #endregion
        }

        void CreatSimulateBullet()
        {
            var go = InstantiateBullet();
            var bulletCtr = go.GetComponent<BulletCtr>();
            bulletCtr.IsGhost = true;
            var dir = _isForwardShoot ? GetDirection_ToGun() : GetDirection_WallBulletToPlayer();
            _projection.SimulateLinePosition(bulletCtr, dir);
            Destroy(go);
        }

        GameObject InstantiateBullet()
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
                _canShoot = true;
                _canMove = true;
                _isForwardShoot = true;
                _nowShootTime = Time.time;
                TypeEventSystem.Global.Send<GamePlayerGetBackBulletEvt>();
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

        Vector2 GetDirection_ToGun()
        {
            return (_mouseV2 - (Vector2)gunGo.transform.position).normalized;
        }

        Vector2 GetDirection_WallBulletToPlayer()
        {
            return ((Vector2)transform.position - bulletOnPlacePos).normalized;
        }

        Vector2 GetDirection_GoToPlayer(Vector2 GoPos)
        {
            return ((Vector2)transform.position - GoPos).normalized;
        }

        public Vector2 GetMouseInfo()
        {
            return (_mouseV2 - (Vector2)gunGo.transform.position).normalized;
        }
    }
}