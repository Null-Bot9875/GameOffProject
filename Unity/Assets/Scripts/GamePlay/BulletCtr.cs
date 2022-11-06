using System;
using UnityEngine;

namespace Game
{
    public class BulletCtr : MonoBehaviour
    {
        [SerializeField] private float shootSpeed;
        [SerializeField] private float backSpeed;
        #region 组件
        private Rigidbody2D rb;
        private Collider2D cld;
        #endregion
        

        private void OnEnable()
        {
            rb = GetComponent<Rigidbody2D>();
            cld = GetComponent<Collider2D>();
        }

        public void SetFire(Vector2 direction)
        {
            transform.up = direction;
            rb.velocity = direction * shootSpeed;
        }
        
    }
}
