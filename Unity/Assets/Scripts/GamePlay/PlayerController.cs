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

        #endregion

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
                switch (_isForwardShoot)
                {
                    #region 射击与瞄准

                    case true:
                        if (Input.GetMouseButtonDown(0))
                        {
                            var go = Instantiate(Resources.Load("Prefabs/Item/Bullet"),
                                    muzzle.transform.position, gunGo.transform.rotation)
                                as GameObject;
                            // go.transform.up  = (mouseV2 - (Vector2)gunGo.transform.position).normalized;
                            go.GetComponent<BulletCtr>()
                                .SetFire((mouseV2 - (Vector2)gunGo.transform.position).normalized);
                            _isForwardShoot = false;
                            canShoot = false;
                        }

                        if (Input.GetMouseButtonDown(1))
                        {
                            GetComponent<LineRenderer>().enabled = true;
                        }

                        if (Input.GetMouseButton(1))
                        {
                            var go = Instantiate(Resources.Load("Prefabs/Item/Bullet"),
                                muzzle.transform.position, gunGo.transform.rotation) as GameObject;
                            _projection.SimulateTrajectory(
                                go.GetComponent<BulletCtr>(),
                                muzzle.transform.position,
                                gunGo.transform.rotation, (mouseV2 - (Vector2)gunGo.transform.position).normalized);
                            Destroy(go.gameObject);
                        }

                        break;

                    #endregion

                    #region 预瞄与返回

                    case false:
                        if (Input.GetMouseButtonDown(1)) GetComponent<LineRenderer>().enabled = true;
                        if (Input.GetMouseButton(1))
                        {
                            Vector2 nomToPlayer = new Vector2(transform.position.x - bulletOnWallPos.x,
                                transform.position.y - bulletOnWallPos.y).normalized;
                            var go = Instantiate(Resources.Load("Prefabs/Item/Bullet"),
                                new Vector2(
                                    bulletOnWallPos.x + ( nomToPlayer.x) * offsetCoefficient,
                                    bulletOnWallPos.y + ( nomToPlayer.y) * offsetCoefficient), Quaternion.identity) as GameObject;
                            go.GetComponent<BulletCtr>().isback = true;
                            _projection.SimulateTrajectory(
                                go.GetComponent<BulletCtr>(),
                                new Vector2(
                                    bulletOnWallPos.x + ( nomToPlayer.x) * offsetCoefficient,
                                    bulletOnWallPos.y + ( nomToPlayer.y) * offsetCoefficient),
                                Quaternion.identity,
                                new Vector2(transform.position.x - go.transform.position.x,
                                    transform.position.y - go.transform.position.y).normalized);
                            Destroy(go.gameObject);
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            Vector2 nomToPlayer = new Vector2(transform.position.x - bulletOnWallPos.x,
                                transform.position.y - bulletOnWallPos.y).normalized;
                            var go = Instantiate(Resources.Load("Prefabs/Item/Bullet"),
                                new Vector2(
                                    bulletOnWallPos.x + (nomToPlayer.x) * offsetCoefficient,
                                    bulletOnWallPos.y + ( nomToPlayer.y) * offsetCoefficient),
                                Quaternion.identity) as GameObject;
                            go.GetComponent<BulletCtr>().SetFire(
                                new Vector2(transform.position.x - go.transform.position.x,
                                    transform.position.y - go.transform.position.y).normalized, false, true);
                            canShoot = false;
                        }

                        break;

                    #endregion
                }
            }

            if (Input.GetMouseButtonUp(1) || !canShoot) GetComponent<LineRenderer>().enabled = false;
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
    }
}