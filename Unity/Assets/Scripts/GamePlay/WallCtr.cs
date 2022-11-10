using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameEvent;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class WallCtr : MonoBehaviour
    {
        private Collider2D col;
        private GameObject bulletOnWallObj;
        private GameObject instanceOnWallObj;

        private void Start()
        {
            bulletOnWallObj = Resources.Load<GameObject>("Prefabs/Item/BulletOnwall");
            TypeEventSystem.Global.Register<GameBulletShotOutWallEvt>(DestroyWallBullet).UnRegisterWhenGameObjectDestroyed(gameObject);
            TypeEventSystem.Global.Register<GameBulletShotOnWallEvt>(CreatWallBullet).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        void DestroyWallBullet(GameBulletShotOutWallEvt gameBulletShotOutWallEvt)
        {
            Destroy(instanceOnWallObj);
        }

        void CreatWallBullet(GameBulletShotOnWallEvt gameBulletShotOnWallEvt)
        {

        }
        
        

        private void OnCollisionEnter2D(Collision2D col1)
        {
            var go = col1.gameObject;
            if (go.CompareTag("Bullet") || go.CompareTag("BackBullet"))
            {
                if (go.GetComponent<BulletCtr>().QueryIsghost())
                {
                    TypeEventSystem.Global.Send<GameOpenEndPointEvt>();
                    Destroy(go);
                    go.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    return;
                }else
                {
                   //todo 播放子弹上墙动画
                        TypeEventSystem.Global.Send(new GameBulletShotOnWallEvt
                        {
                            bulletPos = go.transform.position
                        } );
                        go.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        instanceOnWallObj = Instantiate(bulletOnWallObj, go.transform.position, Quaternion.identity);
                        Destroy(go);
                }
            }
        
        }

        // private void OnTriggerEnter2D(Collider2D col1)
        // {
        //     var go = col1.gameObject;
        //     if (go.CompareTag("Bullet") || go.CompareTag("BackBullet"))
        //     {
        //         if (go.GetComponent<BulletCtr>().QueryIsghost())
        //         {
        //             Destroy(go);
        //             go.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //             return;
        //         }else
        //         {
        //             //todo 播放子弹上墙动画
        //                 TypeEventSystem.Global.Send(new GameBulletShotOnWallEvt
        //                 {
        //                     bulletPos = go.transform.position
        //                 } );
        //                 go.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //                 instanceOnWallObj = Instantiate(bulletOnWallObj,go.transform.position, Quaternion.identity);
        //                 Destroy(go);
        //         }
        //     }
        // }

        
    }
}
