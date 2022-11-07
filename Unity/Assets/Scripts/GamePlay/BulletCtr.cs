using System;
using DG.Tweening;
using Game.GameEvent;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public class BulletCtr : MonoBehaviour
    {
        [SerializeField,Header("发射速度")] private float shootSpeed;
        [SerializeField,Header("折返速度")] private float backSpeed;
        private bool isghost = false;
        #region 组件
        private Rigidbody2D rb;
        private Collider2D cld;
        [SerializeField] private GameObject bulletOnWallObj;
        #endregion

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (isghost && col.gameObject.CompareTag("OuterWall"))
            {
                Destroy(gameObject);
                rb.velocity = Vector2.zero;
                return;
            }
            if (col.gameObject.CompareTag("OuterWall"))
            {
                //todo 播放子弹上墙动画
                TypeEventSystem.Global.Send(new BulletShotOnWall()
                {
                    bulletPos = transform.position
                });
                rb.velocity = Vector2.zero;
                Instantiate(bulletOnWallObj, transform.position, quaternion.identity);
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            rb = GetComponent<Rigidbody2D>();
            cld = GetComponent<Collider2D>();
        }

        public void SetFire(Vector2 direction,bool isghost =false)
        {
            this.isghost = isghost;
            transform.up = direction;
            rb.velocity = direction * shootSpeed;
            if (!this.isghost) SetEffect();
        }

        private void Update()
        {
            transform.up = rb.velocity.normalized;
        }

        void SetEffect()
        {
            Camera.main.DOShakePosition(.05f, .05f);
        }
        
    }
}
