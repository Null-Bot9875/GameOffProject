using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class BulletCtr : MonoBehaviour
    {
        [SerializeField,Header("发射速度")] private float shootSpeed;
        [SerializeField,Header("折返速度")] private float backSpeed;
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
            // Camera.main.DOShakePosition(.05f, .05f);
            transform.up = direction;
            rb.velocity = direction * shootSpeed;
        }
        
    }
}
