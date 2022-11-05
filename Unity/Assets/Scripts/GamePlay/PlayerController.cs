using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        private float x;
        private float y;
        [SerializeField,Header("玩家移动速度")]private float moveSpeed;
        private Vector2 mouseV2;
        public float tset;

        #region 组件
        private Rigidbody2D rb;
        private Collider2D cld;
        [SerializeField] private GameObject gunGo;
        [SerializeField] private Transform muzzle;
        private Camera _camera;
        private bool _isCameraNotNull;

        #endregion
        // Start is called before the first frame update
        void Start()
        {
            _camera = Camera.main;
            rb = GetComponent<Rigidbody2D>();
            cld = GetComponent<Collider2D>();
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
            rb.velocity = new Vector2(x*moveSpeed, y*moveSpeed);
            #endregion

            #region 射击
            if (Input.GetMouseButtonDown(0))
            {
                var go = Instantiate(Resources.Load("Prefabs/Item/Bullet"),
                    muzzle.transform.position,gunGo.transform.rotation)
                     as GameObject;
                // go.transform.up  = (mouseV2 - (Vector2)gunGo.transform.position).normalized;
                go.GetComponent<BulletCtr>().SetFire((mouseV2 - (Vector2)gunGo.transform.position).normalized);
            }
            #endregion
        }

        public Vector2 GetMouseInfo()
        {
            return (mouseV2 - (Vector2)gunGo.transform.position).normalized;
        }

        public Vector2 GetMoveInfo()
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            return new Vector2(x*moveSpeed, y*moveSpeed);
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
    }
}
