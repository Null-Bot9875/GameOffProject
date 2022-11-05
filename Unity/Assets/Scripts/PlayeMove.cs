using System.Collections;
using System.Collections.Generic;
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
            mouseV2 = _camera.ScreenToWorldPoint(Input.mousePosition);
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            
            

            rb.velocity = new Vector2(x*moveSpeed, y*moveSpeed);

        }
    }
}
