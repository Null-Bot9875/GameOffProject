using System;
using UnityEngine;

namespace Game
{
    public class BulletCtr : MonoBehaviour
    {
        [SerializeField, Header("发射速度")] private float shootSpeed;
        [SerializeField, Header("折返速度")] private float backSpeed;
        public bool IsGhost { get; set; } = false;
        public bool IsBack { get; set; } = false;

        #region 组件

        private Rigidbody2D rb;

        #endregion

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            AudioManager.Instance.PlayAudioLoop(GamePath.BulletVFX);
        }

        private void OnDestroy()
        {
            AudioManager.Instance.StopAudioLoop(GamePath.BulletVFX);
        }

        public void SetFire(Vector2 direction)
        {
            rb.velocity = IsBack ? direction * backSpeed : direction * shootSpeed;
        }

        private void Update()
        {
            transform.up = rb.velocity.normalized;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var go = col.gameObject;
            //模拟玩家没有挂脚本,特殊处理
            if (IsGhost && go.CompareTag("Player"))
            {
                DestroyGo();
            }
            
            if (go.TryGetComponent(out IBulletTrigger trigger))
            {
                trigger.OnBulletTrigger(this);
            }
        }

        public void DestroyGo()
        {
            rb.velocity = Vector2.zero;
            GameObject.Destroy(gameObject);
        }
    }
}