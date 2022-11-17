using Game.GameEvent;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour, IExplosion
    {
        private float x;
        private float y;
        [SerializeField, Header("玩家移动速度")] private float moveSpeed;
        [SerializeField, Header("偏移系数")] private float offsetCoefficient;
        private Vector2 _mouseV2;
        private bool _isForwardShoot;
        private bool _canShoot;
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
        private bool _isCameraNotNull;
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
            _nowShootTime = -.5f; //todo 5秒cd
        }

        void OnBulletOnPlaceEvt(GameBulletShotOnPlaceEvt gameBulletShotOnPlaceEvt)
        {
            bulletOnPlacePos = gameBulletShotOnPlaceEvt.bulletPos;
            _canShoot = true;
        }

        void Update()
        {
            #region 鼠标跟随

            _mouseV2 = _camera.ScreenToWorldPoint(Input.mousePosition);

            ChangeWeaponForce();

            #endregion

            #region 角色移动

            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(x * moveSpeed, y * moveSpeed);

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
                    SimulateBullet();

                    if (Input.GetMouseButtonDown(0))
                        ShootBullet();
                }
            }

            if (Input.GetMouseButtonUp(1) || !_canShoot)
            {
                _line.gameObject.SetActive(false);
                _line.gameObject.GetComponent<Projection>().Disable();
            }

            #endregion
        }

        public void OnRecycleBullet()
        {
            TypeEventSystem.Global.Send<GamePlayerGetBackBulletEvt>();
            _isForwardShoot = true;
            _canShoot = true;
            CountShootCD();
        }
        
        private void ShootBullet()
        {
            if (_isForwardShoot)
            {
                var go = Instantiate(bullet, muzzle.transform.position, gunGo.transform.rotation);
                go.GetComponent<BulletCtr>().SetFire(GetDirection_ToGun());
                _isForwardShoot = false;
                _canShoot = false;
            }
            else
            {
                TypeEventSystem.Global.Send<GamePlayerWantRetrievesBulletEvt>();
                var position = bulletOnPlacePos - GetDirection_WallBulletToPlayer() * offsetCoefficient;
                var go = Instantiate(bullet, position, Quaternion.identity);
                go.GetComponent<BulletCtr>().SetFire(GetDirection_GoToPlayer(go.transform.position));
                go.GetComponent<BulletCtr>().SetBack();
                _canShoot = false;
                //停止移动
                this.enabled = false;
                rb.velocity = Vector2.zero;
            }
        }

        private void SimulateBullet()
        {
            var go = Instantiate(bullet);
            var bulletCtr = go.GetComponent<BulletCtr>();
            bulletCtr.SetGhost();
            if (!_isForwardShoot)
            {
                go.transform.position = bulletOnPlacePos - GetDirection_WallBulletToPlayer() * offsetCoefficient;
                go.transform.rotation = Quaternion.identity;
                bulletCtr.SetBack();
                _projection.SimulateTrajectory(bulletCtr, GetDirection_WallBulletToPlayer());
            }
            else
            {
                go.transform.position = muzzle.transform.position;
                go.transform.rotation = gunGo.transform.rotation;
                _projection.SimulateTrajectory(bulletCtr, GetDirection_ToGun());
            }

            Destroy(go);
        }

        void CountShootCD()
        {
            _nowShootTime = Time.time;
        }

        public Vector2 GetMouseInfo()
        {
            return (_mouseV2 - (Vector2)gunGo.transform.position).normalized;
        }

        public Vector2 GetPlayerMoveInfo()
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            return new Vector2(x * moveSpeed, y * moveSpeed);
        }

        void ChangeWeaponForce()
        {
            gunGo.transform.right = GetDirection_ToGun();
            gunGo.GetComponent<SpriteRenderer>().flipY = (_mouseV2.x < transform.position.x);

            if (_mouseV2.y > transform.position.y)
            {
                gunGo.GetComponent<SpriteRenderer>().sortingOrder = -1;
            }
            else
            {
                gunGo.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
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

        public void OnExplosion()
        {
            Die();
        }

        public void Die()
        {
            Debug.Log("playerDie");
            TypeEventSystem.Global.Send<GameOverEvt>();
        }
    }
}