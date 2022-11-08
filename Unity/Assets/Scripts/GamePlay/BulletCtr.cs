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
        public bool isback = false;
        #region 组件
        private Rigidbody2D rb;
        private Collider2D col;
        #endregion

        public bool QueryIsghost()
        {
            return isghost;
        }
        private void OnEnable()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
        }

        public void SetFire(Vector2 direction,bool isghost =false,bool isback = false)
        {
            this.isghost = isghost;
            this.isback = isback;
            ChangeTagLayer();
            transform.up = direction;
            rb.velocity = direction * shootSpeed;
            if (!this.isghost) SetEffect();
            if (isback && !isghost)
            {
                TypeEventSystem.Global.Send<GameBulletShotOutWallEvt>();
            }
        }

        private void Update()
        {
            transform.up = rb.velocity.normalized;
            
        }

        void ChangeTagLayer()
        {
            switch (isback)
            {
                case true:
                    transform.tag = "BackBullet";
                    gameObject.layer = LayerMask.NameToLayer("BackBullet");
                    break;
                case false:
                    transform.tag = "Bullet";
                    gameObject.layer = LayerMask.NameToLayer("Bullet");
                    break;
            }
        }

        void SetEffect()
        {
            Camera.main.DOShakePosition(.05f, .05f);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player") && isghost)
            {
                rb.velocity = Vector2.zero;
                Destroy(gameObject);
            }
        }
    }
}
