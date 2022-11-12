using DG.Tweening;
using Game.GameEvent;
using UnityEngine;

namespace Game
{
    public class BulletCtr : MonoBehaviour
    {
        [SerializeField, Header("发射速度")] private float shootSpeed;
        [SerializeField, Header("折返速度")] private float backSpeed;
        public bool isGhost;
        public bool isBack;

        #region 组件

        private Rigidbody2D rb;

        #endregion

        public bool QueryIsghost()
        {
            return isGhost;
        }

        private void OnEnable()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void SetFire(Vector2 direction, bool isghost = false, bool isback = false)
        {
            this.isGhost = isghost;
            this.isBack = isback;
            transform.up = direction;
            if (isback)
            {
                rb.velocity = direction * backSpeed;
            }
            else
            {
                rb.velocity = direction * shootSpeed;
            }
        }
        
        

        private void Update()
        {
            transform.up = rb.velocity.normalized;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player")) // 子弹遇到玩家 停下并销毁子弹，如果是再实际场景并且非返回状态下遇到玩家，玩家死亡
            {
                if (!isGhost && !isBack)
                {
                    Debug.Log("playerDie");
                    TypeEventSystem.Global.Send<GamePlayerDieEvt>();
                }
                if (isGhost && isBack)
                {
                    rb.velocity = Vector2.zero;
                    Destroy(gameObject);
                }
                
            }

        }
    }
}