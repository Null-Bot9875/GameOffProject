using Game.GameEvent;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        private float x;
        private float y;
        [SerializeField, Header("玩家移动速度")] private float moveSpeed;
        [SerializeField, Header("偏移系数")] private float offsetCoefficient;
        private Vector2 mouseV2;
        private bool _isForwardShoot;
        private bool canShoot;
        private Vector2 bulletOnWallPos;

        #region 子弹回收

        private float _shootCD = .5f; //todo 5秒CD
        private float _nowShootTime;

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

            TypeEventSystem.Global.Register<GameBulletShotOnWallEvt>(OnBulletOnWallEvt)
                .UnRegisterWhenGameObjectDestroyed(gameObject);

            #endregion

            _camera = Camera.main;
            rb = GetComponent<Rigidbody2D>();
            _isForwardShoot = true;
            canShoot = true;
            _nowShootTime = -5f;
        }

        void OnBulletOnWallEvt(GameBulletShotOnWallEvt gameBulletShotOnWallEvt)
        {
            bulletOnWallPos = gameBulletShotOnWallEvt.bulletPos;
            canShoot = true;
        }

        void Update()
        {
            #region 鼠标跟随

            mouseV2 = _camera.ScreenToWorldPoint(Input.mousePosition);

            ChangeWeaponForce();

            #endregion

            #region 角色移动

            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(x * moveSpeed, y * moveSpeed);

            #endregion

            #region 瞄准射击

            var canShootCD = Time.time > _shootCD + _nowShootTime;

            if (canShoot && canShootCD)
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
                        if (_isForwardShoot)
                        {
                            var go = Instantiate(bullet, muzzle.transform.position, gunGo.transform.rotation);
                            go.GetComponent<BulletCtr>().SetFire(GetDirection_ToGun());
                            _isForwardShoot = false;
                            canShoot = false;
                        }
                        else
                        {
                            var position = bulletOnWallPos - GetDirection_WallBulletToPlayer() * offsetCoefficient;
                            var go = Instantiate(bullet, position, Quaternion.identity);
                            go.GetComponent<BulletCtr>()
                                .SetFire(GetDirection_GoToPlayer(go.transform.position), false, true);
                            canShoot = false;
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(1) || !canShoot)
            {
                _line.gameObject.SetActive(false);
                _line.gameObject.GetComponent<Projection>().Disable();
            }

            #endregion
        }

        void CountShootCD()
        {
            _nowShootTime = Time.time;
        }

        public Vector2 GetMouseInfo()
        {
            return (mouseV2 - (Vector2)gunGo.transform.position).normalized;
        }

        public Vector2 GetPlayerMoveInfo()
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            return new Vector2(x * moveSpeed, y * moveSpeed);
        }

        void ChangeWeaponForce()
        {
            gunGo.transform.right = (mouseV2 - (Vector2)gunGo.transform.position).normalized;
            gunGo.GetComponent<SpriteRenderer>().flipY = (mouseV2.x < transform.position.x);

            if (mouseV2.y > transform.position.y)
            {
                gunGo.GetComponent<SpriteRenderer>().sortingOrder = -1;
            }
            else
            {
                gunGo.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.transform.CompareTag("Bullet")) //子弹返回玩家身上
            {
                if (col.gameObject.GetComponent<BulletCtr>().isback)
                {
                    _isForwardShoot = true;
                    canShoot = true;
                    CountShootCD();
                    Destroy(col.gameObject);
                }
                
            }
        }

        void CreatSimulateBullet()
        {
            var go = Instantiate(bullet);
            var bulletCtr = go.GetComponent<BulletCtr>();
            if (!_isForwardShoot)
            {
                go.transform.position = bulletOnWallPos - GetDirection_WallBulletToPlayer() * offsetCoefficient;
                go.transform.rotation = Quaternion.identity;
                bulletCtr.isback = true;
                _projection.SimulateTrajectory(bulletCtr, GetDirection_WallBulletToPlayer());
            }
            else
            {
                go.transform.position = muzzle.transform.position;
                go.transform.rotation =  gunGo.transform.rotation;
                _projection.SimulateTrajectory(bulletCtr, GetDirection_ToGun());
            }

            Destroy(go);
        }

        Vector2 GetDirection_ToGun()
        {
            return (mouseV2 - (Vector2)gunGo.transform.position).normalized;
        }

        Vector2 GetDirection_WallBulletToPlayer()
        {
            return ((Vector2)transform.position - bulletOnWallPos).normalized;
        }

        Vector2 GetDirection_GoToPlayer(Vector2 GoPos)
        {
            return ((Vector2)transform.position - GoPos).normalized;
        }
    }
}