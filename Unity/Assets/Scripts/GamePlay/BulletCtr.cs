using DG.Tweening;
using Game.GameEvent;
using UnityEngine;

namespace Game
{
    public class BulletCtr : MonoBehaviour
    {
        [SerializeField, Header("发射速度")] private float shootSpeed;
        [SerializeField, Header("折返速度")] private float backSpeed;
        public bool isGhost = false;
        public bool isBack = false;

        #region 组件

        private Rigidbody2D rb;

        #endregion
        private void OnEnable()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void SetFire(Vector2 direction)
        {
            rb.velocity = isBack ? direction * backSpeed : direction * shootSpeed;
        }

        public void SetGhost()
        {
            isGhost = true;
        }

        public void SetBack()
        {
            isBack = true;
        }

        public bool QueryGhost()
        {
            return isGhost;
        }

        public bool QueryBack()
        {
            return isBack;
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
                    TypeEventSystem.Global.Send<GameOverEvt>();
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