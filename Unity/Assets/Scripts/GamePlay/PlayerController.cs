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

        #region 组件

        private Rigidbody2D rb;
        private Collider2D cld;
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

        // Start is called before the first frame update
        void Start()
        {
            #region 事件注册

            TypeEventSystem.Global.Register<GameBulletShotOnWallEvt>(GetBulletOnWallPos)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            TypeEventSystem.Global.Register<GameBulletShotOutWallEvt>(DosthBulletOutWall)
                .UnRegisterWhenGameObjectDestroyed(gameObject);

            #endregion

            _camera = Camera.main;
            rb = GetComponent<Rigidbody2D>();
            cld = GetComponent<Collider2D>();
            _isForwardShoot = true;
            canShoot = true;
        }

        void GetBulletOnWallPos(GameBulletShotOnWallEvt gameBulletShotOnWallEvt)
        {
            bulletOnWallPos = gameBulletShotOnWallEvt.bulletPos;
            canShoot = true;
        }

        void DosthBulletOutWall(GameBulletShotOutWallEvt gameBulletShotOutWallEvt)
        {
            canShoot = true;
        }

        // Update is called once per frame
        void Update()
        {
            #region 鼠标跟随

            mouseV2 = _camera.ScreenToWorldPoint(Input.mousePosition);
            gunGo.transform.right = (mouseV2 - (Vector2)gunGo.transform.position).normalized;
            ChangeWeaponForce();

            #endregion

            #region 角色移动

            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(x * moveSpeed, y * moveSpeed);

            #endregion

            if (canShoot)
            {

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
                        var go = Instantiate(bullet, bulletOnWallPos - GetDirection_WallBulletToPlayer()*offsetCoefficient,
                            Quaternion.identity);
                        go.GetComponent<BulletCtr>().SetFire(
                            GetDirection_GoToPlayer(go.transform.position), false, true);
                        canShoot = false;
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    _line.gameObject.SetActive(true);
                    _line.gameObject.GetComponent<Projection>().Enable();
                }
                if (Input.GetMouseButton(1))
                {
                    if (_isForwardShoot)
                    {
                        CreatSimulateBullet(false);
                    }
                    else
                    {
                        CreatSimulateBullet(true);
                    }
                }
            }

            if (Input.GetMouseButtonUp(1) || !canShoot)
            {
                _line.gameObject.SetActive(false);
                _line.gameObject.GetComponent<Projection>().Disable();
            }
        }

        public Vector2 GetMouseInfo()
        {
            return (mouseV2 - (Vector2)gunGo.transform.position).normalized;
        }

        public Vector2 GetMoveInfo()
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            return new Vector2(x * moveSpeed, y * moveSpeed);
        }

        void ChangeWeaponForce()
        {
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
            if (col.transform.CompareTag("BackBullet"))
            {
                _isForwardShoot = true;
                canShoot = true;
                Destroy(col.gameObject);
            }
        }

        void CreatSimulateBullet(bool isback)
        {
            GameObject go;
            if (isback)
            {
                 go = Instantiate(bullet, bulletOnWallPos + GetDirection_WallBulletToPlayer()*offsetCoefficient, Quaternion.identity);
                go.GetComponent<BulletCtr>().isback = true;
                _projection.SimulateTrajectory(go.GetComponent<BulletCtr>(),
                    bulletOnWallPos - GetDirection_WallBulletToPlayer()*offsetCoefficient, Quaternion.identity, 
                    GetDirection_WallBulletToPlayer());
                Destroy(go.gameObject);
            }
            else
            {
                go = Instantiate(bullet, muzzle.transform.position, gunGo.transform.rotation);
                _projection.SimulateTrajectory(
                    go.GetComponent<BulletCtr>(),
                    muzzle.transform.position,
                    gunGo.transform.rotation, GetDirection_ToGun());
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