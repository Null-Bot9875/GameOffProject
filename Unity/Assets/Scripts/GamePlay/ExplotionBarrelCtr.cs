using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameEvent;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class ExplotionBarrelCtr : MonoBehaviour
    {
        [SerializeField, Header("爆炸特效")] private GameObject explosionClip;
        private GameObject explosionGo;
        [SerializeField, Header("爆炸半径")] private float explosionRadius;

        private void OnTriggerEnter2D(Collider2D col)
        {
            var go = col.gameObject;
            if (go.CompareTag("Bullet"))
            {
                if (!go.GetComponent<BulletCtr>().QueryGhost())
                {
                    Explosion();
                }
                
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,explosionRadius);
        }

        public void Explosion()
        {
            explosionGo = Instantiate(explosionClip, transform.position, transform.rotation);
            

            if (Vector2.Distance(transform.position,GameDataCache.Instance.Player.transform.position) < explosionRadius)
            {
                TypeEventSystem.Global.Send<GameOverEvt>();
            }
            foreach (var item in GameDataCache.Instance.EnemyList)
            {
                if (Vector2.Distance(transform.position,item.transform.position) < explosionRadius)
                {
                    //todo 敌人死了
                }
            }
            Destroy(gameObject);
        }
    }
}
