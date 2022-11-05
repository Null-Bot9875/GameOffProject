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

        #region 组件
        private Rigidbody2D rb;
        private Collider2D cld;
        #endregion
        
        
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            cld = GetComponent<Collider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            

            rb.velocity = new Vector2(x*moveSpeed, y*moveSpeed);

        }
    }
}
