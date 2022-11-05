using Animancer;
using UnityEngine;

namespace Game
{
    public class PlayeMove : MonoBehaviour
    {
        private float x;
        private float y;
        [SerializeField]private float moveSpeed;
        private Vector2 mouseV2;

        #region 组件
        private Rigidbody2D rb;
        private Collider2D cld;
        [SerializeField] private GameObject gunGo;
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
            #endregion
            #region 角色移动
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(x*moveSpeed, y*moveSpeed);
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

    }
}
