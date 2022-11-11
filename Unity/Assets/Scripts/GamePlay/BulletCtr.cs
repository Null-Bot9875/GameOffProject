using DG.Tweening;
using Game.GameEvent;
using UnityEngine;

namespace Game
{
    public class BulletCtr : MonoBehaviour
    {
        [SerializeField, Header("发射速度")] private float shootSpeed;
        [SerializeField, Header("折返速度")] private float backSpeed;
        private bool isghost;
        public bool isback;

        #region 组件

        private Rigidbody2D rb;

        #endregion

        public bool QueryIsghost()
        {
            return isghost;
        }

        private void OnEnable()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void SetFire(Vector2 direction, bool isghost = false, bool isback = false)
        {
            this.isghost = isghost;
            this.isback = isback;
            transform.up = direction;
            if (isback)
            {
                rb.velocity = direction * backSpeed;
            }
            else
            {
                rb.velocity = direction * shootSpeed;
            }

            if (!isghost)
            {
                SetEffect();
                if (isback)
                {
                    TypeEventSystem.Global.Send<GameBulletShotOutWallEvt>();
                }
            }
        }

        private void Update()
        {
            transform.up = rb.velocity.normalized;
        }

        void SetEffect()
        {
            Camera.main.DOShakePosition(.05f, .05f);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player")) // 子弹遇到玩家 停下并销毁子弹，如果是再实际场景并且非返回状态下遇到玩家，玩家死亡
            {
                if (!isghost && !isback)
                {
                    Debug.Log("playerDie");
                    TypeEventSystem.Global.Send<GamePlayerDieEvt>();
                }
                rb.velocity = Vector2.zero;
                Destroy(gameObject);
            }

        }
    }
}