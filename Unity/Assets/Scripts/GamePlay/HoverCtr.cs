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
            if (col1.transform.CompareTag("Bullet") && !isInHover)
            {
                if (!go.GetComponent<BulletCtr>().QueryGhost())
                {
                    isInHover = true;
                    go.transform.DOMove(transform.position, 2f).SetEase(Ease.InCirc).OnComplete(OnBulletMoveComplete);
                }
                else
                {
                    go.transform.position = transform.position;
                    go.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    GameObject.Destroy(go);
                }
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
    }
}