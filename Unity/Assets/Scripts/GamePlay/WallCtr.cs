using Game.GameEvent;
using UnityEngine;

namespace Game
{
    public class WallCtr : MonoBehaviour, IBulletTrigger
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

        public void OnNormalBulletTrigger(BulletCtr ctr)
        {
            if (ctr.IsGhost)
            {
                ctr.DestroyGo();
                return;
            }

            var go = ctr.gameObject;
            //todo 播放子弹上墙动画
            TypeEventSystem.Global.Send(new GameBulletShotOnPlaceEvt
            {
                bulletPos = go.transform.position
            });
            isInWall = true;
            instanceOnWallObj = Instantiate(bulletOnWallObj, go.transform.position, go.transform.rotation);
            ctr.DestroyGo();
        }
    }
}