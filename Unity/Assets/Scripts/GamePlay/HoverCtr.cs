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
        [SerializeField,Header("嵌入墙GameObject")] private GameObject _sprite;
        private GameObject go;

        private void Start()
        {
            TypeEventSystem.Global.Register<GamePlayerBackShootEvt>(DestroyGo)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (isInHover)
            {
                return;
            }

            isInHover = true;
            if (col.transform.CompareTag("Bullet") || col.transform.CompareTag("BackBullet"))
            {
                col.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                col.transform.DOMove(transform.position, 0.1f).OnComplete(() =>
                {
                    if (!col.gameObject.GetComponent<BulletCtr>().QueryIsghost())
                    {
                        go = Instantiate(_sprite, col.transform.position, Quaternion.identity);
                        TypeEventSystem.Global.Send(new GameBulletShotOnWallEvt
                        {
                            bulletPos = col.transform.position
                        } );
                    }
                    Destroy(col.gameObject);
                });

            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            isInHover = false;
        }

        void DestroyGo(GamePlayerBackShootEvt gamePlayerBackShootEvt)
        {
            Destroy(go);
        }
    }
}
