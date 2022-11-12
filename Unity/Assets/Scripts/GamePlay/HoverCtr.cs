using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.GameEvent;
using UnityEngine;

namespace Game
{
    public class HoverCtr : MonoBehaviour
    {
        private bool isInHover;
        [SerializeField,Header("嵌入墙GameObject")] private GameObject BulletOnHoverObj;
        private GameObject instanceHoverGo;

        private void Start()
        {
            TypeEventSystem.Global.Register<GamePlayerWantRetrievesBulletEvt>(HoverBulletShoot)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }
        
        void HoverBulletShoot(GamePlayerWantRetrievesBulletEvt playerWantRetrievesBulletEvt)
        {
            if (isInHover == false)
            {
                return;
            }

            GetComponent<Collider2D>().enabled = false;
            Destroy(instanceHoverGo);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var go = col.gameObject;
            if (col.transform.CompareTag("Bullet"))
            {
                if (!go.GetComponent<BulletCtr>().isGhost)
                {
                    
                    TypeEventSystem.Global.Send(new GameBulletShotOnHoverEvt()
                    {
                        bulletPos = go.transform.position
                    });
                }

                isInHover = true;
                go.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                instanceHoverGo = Instantiate(BulletOnHoverObj, go.transform);
                Destroy(go);
            }
        }
    }
}
