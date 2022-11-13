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

        [SerializeField, Header("嵌入墙GameObject")]
        private GameObject BulletOnHoverObj;

        private GameObject instanceHoverGo;
        private Collider2D col;

        private void Start()
        {
            col = GetComponent<Collider2D>();

            #region 注册事件

            TypeEventSystem.Global.Register<GamePlayerWantRetrievesBulletEvt>(HoverBulletShoot)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            TypeEventSystem.Global.Register<GameBulletShotOnPlaceEvt>(SetColliderFromWall)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            TypeEventSystem.Global.Register<GamePlayerGetBackBulletEvt>(SetColliderFromPlayer)
                .UnRegisterWhenGameObjectDestroyed(gameObject);

            #endregion
        }

        void HoverBulletShoot(GamePlayerWantRetrievesBulletEvt playerWantRetrievesBulletEvt)
        {
            if (!isInHover)
                return;

            isInHover = false;
            Destroy(instanceHoverGo);
        }

        void SetColliderFromWall(GameBulletShotOnPlaceEvt gameBulletShotOnPlaceEvt)
        {
            col.enabled = true;
        }

        void SetColliderFromPlayer(GamePlayerGetBackBulletEvt getBackBulletEvt)
        {
            col.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D col1)
        {
            var go = col1.gameObject;
            if (col1.transform.CompareTag("Bullet") && !isInHover && !go.GetComponent<BulletCtr>().QueryGhost())
            {
                isInHover = true;
                go.transform.DOMove(transform.position, 2f).SetEase(Ease.InCirc).OnComplete(OnBulletMoveComplete);
            }

            void OnBulletMoveComplete()
            {
                GameObject.Destroy(go);
                TypeEventSystem.Global.Send(new GameBulletShotOnPlaceEvt
                {
                    bulletPos = go.transform.position
                });
                col.enabled = false;
                instanceHoverGo = Instantiate(BulletOnHoverObj, go.transform.position, go.transform.rotation);
            }
        }


        // IEnumerator SlowBullet(GameObject go)
        // {
        //     DOTween.To(() => go.GetComponent<Rigidbody2D>().velocity , (x) => go.GetComponent<Rigidbody2D>().velocity = x,new Vector2(0, 0), 1f);
        //     yield return new WaitForSeconds(1f);
        //     Destroy(go);
        // }
    }
}