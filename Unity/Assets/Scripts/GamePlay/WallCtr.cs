using Game.GameEvent;
using UnityEngine;

namespace Game
{
    public class WallCtr : MonoBehaviour
    {
        private GameObject bulletOnWallObj;
        private GameObject instanceOnWallObj;
        private bool isInWall;

        private void Start()
        {
            bulletOnWallObj = Resources.Load<GameObject>("Prefabs/Item/BulletOnwall");
            TypeEventSystem.Global.Register<GamePlayerWantRetrievesBulletEvt>(WallBulletShoot)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        void WallBulletShoot(GamePlayerWantRetrievesBulletEvt playerWantRetrievesBulletEvt)
        {
            if (isInWall == false)
            {
                return;
            }

            Destroy(instanceOnWallObj);
        }

        private void OnCollisionEnter2D(Collision2D col1)
        {
            var go = col1.gameObject;
            if (go.CompareTag("Bullet"))
            {
                if (!go.GetComponent<BulletCtr>().QueryGhost())
                {
                    //todo 播放子弹上墙动画
                    TypeEventSystem.Global.Send(new GameBulletShotOnPlaceEvt
                    {
                        bulletPos = go.transform.position
                    });
                    isInWall = true;
                    instanceOnWallObj = Instantiate(bulletOnWallObj, go.transform.position, go.transform.rotation);
                }

                go.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Destroy(go);
            }
        }
    }
}