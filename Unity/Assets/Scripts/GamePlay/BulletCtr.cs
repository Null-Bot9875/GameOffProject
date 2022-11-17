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
            var go = col.gameObject;
            switch (go.tag)
            {
                case "Player":
                {
                    rb.velocity = Vector2.zero;
                    Destroy(gameObject);

                    if (isGhost)
                        return;

                    if (!isBack)
                    {
                        var player = go.GetComponent<PlayerController>();
                        player.Die();
                    }
                    else
                    {
                        var player = go.GetComponent<PlayerController>();
                        player.OnRecycleBullet();
                    }

                    break;
                }

                case "Enemy":
                {
                    var enemy = go.GetComponent<EnemyController>();
                    enemy.Die();
                    break;
                }

                default:
                    break;
            }
        }
    }
}